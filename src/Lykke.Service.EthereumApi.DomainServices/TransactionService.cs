using System;
using System.Numerics;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Service.Ethereum.Domain.Repositories;

namespace Lykke.Service.Ethereum.Domain.Services
{
    [UsedImplicitly]
    public class TransactionService : ITransactionService
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IChaosKitty _chaosKitty;
        private readonly ILog _log;
        private readonly BigInteger _minimalTransactionAmount;
        private readonly ITransactionMonitoringTaskRepository _transferTransactionMonitoringTaskRepository;
        private readonly ITransactionRepository _transactionRepository;

        
        public TransactionService(
            IBlockchainService blockchainService,
            IChaosKitty chaosKitty,
            ILogFactory logFactory,
            ITransactionMonitoringTaskRepository transferTransactionMonitoringTaskRepository,
            ITransactionRepository transactionRepository,
            Settings settings)
        {
            _blockchainService = blockchainService;
            _chaosKitty = chaosKitty;
            _log = logFactory.CreateLog(this);
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
                _log.Info($"Failed to build transaction [{transactionId}]: amount [{amount}] is too small.");
                
                return BuildTransactionResult.AmountIsTooSmall;
            }
            
            var transaction = await _transactionRepository.TryGetAsync(transactionId);

            if (transaction == null)
            {
                var balance = await _blockchainService.GetBalanceAsync(from);
                var gasPrice = await _blockchainService.EstimateGasPriceAsync(to, amount);
                var transactionFee = gasPrice * Constants.GasAmount;
                
                if (includeFee)
                {
                    amount -= transactionFee;
                }
                
                if (balance < amount + transactionFee)
                {
                    _log.Info($"Failed to build transaction [{transactionId}]: balance [{balance}] is not enough.");
                    
                    return BuildTransactionResult.BalanceIsNotEnough;
                }

                transaction = Transaction.Build
                (
                    transactionId: transactionId,
                    @from: from,
                    to: to,
                    amount: amount,
                    gasAmount: Constants.GasAmount,
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
                    _log.Info($"Failed to broadcast transaction [{transactionId}]: amount [{transaction.Amount}] is too small.");
                    
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
                            }
                        );
                        
                        _chaosKitty.Meow(transactionId);
                        
                        await _transactionRepository.UpdateAsync(transaction);
                        
                        _chaosKitty.Meow(transactionId);
                        
                        _log.Info($"Transaction [{transactionId}] has been broadcasted.");
                        
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

                _log.Info($"Transaction [{transactionId}] has been marked as deleted.");
                
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


        public class Settings
        {
            public BigInteger MinimalTransactionAmount { get; set; }
        }
    }
}
