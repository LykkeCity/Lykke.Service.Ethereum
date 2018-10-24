﻿using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Service.EthereumApi.Core.Domain;
using Lykke.Service.EthereumApi.Core.Services;
using Lykke.Service.EthereumCommon.Core;
using Lykke.Service.EthereumCommon.Core.Domain;
using Lykke.Service.EthereumCommon.Core.Repositories;
using Lykke.SettingsReader;

namespace Lykke.Service.EthereumApi.Services
{
    [UsedImplicitly]
    public class TransactionService : ITransactionService
    {
        private readonly IAddressService _addressService;
        private readonly IBlockchainService _blockchainService;
        private readonly IChaosKitty _chaosKitty;
        private readonly SemaphoreSlim _gasAmountLock;
        private readonly ILog _log;
        private readonly IReloadingManager<string> _maxGasAmountManager;
        private readonly BigInteger _minimalTransactionAmount;
        private readonly ITransactionMonitoringTaskRepository _transferTransactionMonitoringTaskRepository;
        private readonly ITransactionRepository _transactionRepository;

        
        private BigInteger _maxGasAmount;
        private DateTime _maxGasAmountExpiration;
        
        
        public TransactionService(
            IAddressService addressService,
            IBlockchainService blockchainService,
            IChaosKitty chaosKitty,
            ILogFactory logFactory,
            ITransactionMonitoringTaskRepository transferTransactionMonitoringTaskRepository,
            ITransactionRepository transactionRepository,
            Settings settings)
        {
            _addressService = addressService;
            _blockchainService = blockchainService;
            _chaosKitty = chaosKitty;
            _gasAmountLock = new SemaphoreSlim(1);
            _log = logFactory.CreateLog(this);
            _maxGasAmount = BigInteger.Parse(_maxGasAmountManager.CurrentValue);
            _maxGasAmountManager = settings.MaxGasAmountManager;
            _minimalTransactionAmount = settings.MinimalTransactionAmount;
            _transferTransactionMonitoringTaskRepository = transferTransactionMonitoringTaskRepository;
            _transactionRepository = transactionRepository;
        }

        
        public async Task<BuildTransactionResult> BuildTransactionAsync(
            Guid transactionId,
            string from,
            string to,
            BigInteger amount,
            bool includeFee)
        {
            if (amount < _minimalTransactionAmount)
            {
                _log.Info($"Failed to build transaction [{transactionId}]: amount is too small.");
                
                return BuildTransactionResult.AmountIsTooSmall;
            }
            
            var transaction = await _transactionRepository.TryGetAsync(transactionId);

            if (transaction == null)
            {
                var gasAmountAndMaxGasAmount = await Task.WhenAll
                (
                    _blockchainService.EstimateGasAmountAsync(from, to, amount),
                    ReloadMaxGasAmountAsync()
                );
                
                var gasAmount = gasAmountAndMaxGasAmount[0];
                var maxGasAmount = gasAmountAndMaxGasAmount[1];

                if (gasAmount > maxGasAmount)
                {
                    await BlacklistAddressIfNecessaryAsync
                    (
                        address: to,
                        gasAmount: gasAmount,
                        maxGasAmount: maxGasAmount
                    );
                    
                    _log.Info($"Failed to build transaction [{transactionId}]: gas amount is too high.");
                    
                    return BuildTransactionResult.GasAmountIsTooHigh;
                }

                var balanceAndGasPrice = await Task.WhenAll
                (
                    _blockchainService.GetBalanceAsync(from),
                    _blockchainService.EstimateGasPriceAsync()
                );
                
                var balance = balanceAndGasPrice[0];
                var gasPrice = balanceAndGasPrice[1];
                var transactionFee = gasPrice * gasAmount;
                
                if (includeFee)
                {
                    amount -= transactionFee;
                }
                
                if (balance < amount + transactionFee)
                {
                    _log.Info($"Failed to build transaction [{transactionId}]: balance is not enough.");
                    
                    return BuildTransactionResult.BalanceIsNotEnough;
                }

                transaction = Transaction.Build
                (
                    transactionId: transactionId,
                    from: from,
                    to: to,
                    amount: amount,
                    gasAmount: gasAmount,
                    gasPrice: gasPrice,
                    includeFee: includeFee,
                    data: await _blockchainService.BuildTransactionAsync(from, to, amount, gasPrice)
                );

                _chaosKitty.Meow(transactionId);
                
                await _transactionRepository.AddAsync(transaction);

                _log.Info($"Transaction [{transactionId}] has been built.");
                
                return BuildTransactionResult.Success(transaction.Data);
            }
            else
            {
                switch (transaction.State)
                {
                    case TransactionState.Built:
                        return BuildTransactionResult.Success(transaction.Data);
                    
                    case TransactionState.InProgress:
                    case TransactionState.Completed:
                    case TransactionState.Failed:
                        return BuildTransactionResult.TransactionHasBeenBroadcasted;
                    
                    case TransactionState.Deleted:
                        return BuildTransactionResult.TransactionHasBeenDeleted;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task<BroadcastTransactionResult> BroadcastTransactionAsync(
            Guid transactionId,
            string signedTxData)
        {
            var transaction = await _transactionRepository.TryGetAsync(transactionId);

            if (transaction != null)
            {
                if (transaction.Amount < _minimalTransactionAmount)
                {
                    return BroadcastTransactionResult.AmountIsTooSmall;
                }

                switch (transaction.State)
                {
                    case TransactionState.Built:

                        var txHash = await _blockchainService.BroadcastTransactionAsync(signedTxData);
                        
                        transaction.OnBroadcasted
                        (
                            hash: txHash,
                            signedData: signedTxData
                        );
                        
                        await _transferTransactionMonitoringTaskRepository.EnqueueAsync
                        (
                            new TransactionMonitoringTask
                            {
                                TransactionId = transactionId
                            },
                            TimeSpan.FromMinutes(1)
                        );
                        
                        _chaosKitty.Meow(transactionId);
                        
                        await _transactionRepository.UpdateAsync(transaction);
                        
                        _chaosKitty.Meow(transactionId);
                        
                        return BroadcastTransactionResult.Success(txHash);
                    
                    case TransactionState.InProgress:
                    case TransactionState.Completed:
                    case TransactionState.Failed:
                        return BroadcastTransactionResult.TransactionHasBeenBroadcasted;
                    
                    case TransactionState.Deleted:
                        return BroadcastTransactionResult.TransactionHasBeenDeleted;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                return BroadcastTransactionResult.OperationHasNotBeenFound;
            }
        }

        public async Task<bool> DeleteTransactionIfExistsAsync(
            Guid transactionId)
        {
            var transaction = await _transactionRepository.TryGetAsync(transactionId);

            if (transaction != null)
            {
                transaction.OnDeleted();

                await _transactionRepository.UpdateAsync(transaction);

                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<Transaction> TryGetTransactionAsync(
            Guid transactionId)
        {
            return _transactionRepository.TryGetAsync(transactionId);
        }
        
        private async Task BlacklistAddressIfNecessaryAsync(
            string address,
            BigInteger gasAmount,
            BigInteger maxGasAmount)
        {
            if (!await _blockchainService.IsWalletAsync(address))
            {
                await _addressService.AddAddressToBlacklistAsync
                (
                    address: address,
                    reason: $"Gas amount [{gasAmount}] exceeds maximal [{maxGasAmount}]."
                );
            }
        }
        
        private async Task<BigInteger> ReloadMaxGasAmountAsync()
        {
            if (_maxGasAmountExpiration <= DateTime.UtcNow)
            {
                await _gasAmountLock.WaitAsync();

                try
                {
                    var previousMaxGasAmount = _maxGasAmount;
                    
                    if (_maxGasAmountExpiration <= DateTime.UtcNow)
                    {
                        await _maxGasAmountManager.Reload();

                        _maxGasAmountExpiration = DateTime.UtcNow.AddMinutes(1);
                    }
                    
                    var newMaxGasAmount = BigInteger.Parse(_maxGasAmountManager.CurrentValue);

                    if (newMaxGasAmount != previousMaxGasAmount)
                    {
                        _maxGasAmount = newMaxGasAmount;
                        
                        _log.Info($"Maximal gas amount set to {newMaxGasAmount}");
                    }
                }
                catch (Exception e)
                {
                    _log.Warning("Failed to update maximal gas amount.", e);
                }
                finally
                {
                    _gasAmountLock.Release();
                }
            }

            return _maxGasAmount;
        }


        public class Settings
        {
            public IReloadingManager<string> MaxGasAmountManager { get; set; }
            
            public BigInteger MinimalTransactionAmount { get; set; }
        }
    }
}
