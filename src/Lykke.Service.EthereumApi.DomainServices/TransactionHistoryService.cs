using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Domain.Repositories;

namespace Lykke.Service.Ethereum.Domain.Services
{
    [UsedImplicitly]
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly ITransactionReceiptRepository _transactionReceiptRepository;

        
        public TransactionHistoryService(
            ITransactionReceiptRepository transactionReceiptRepository)
        {
            _transactionReceiptRepository = transactionReceiptRepository;
        }

        
        public Task<IEnumerable<TransactionReceipt>> GetIncomingHistoryAsync(
            string address,
            int take,
            string afterHash)
        {
            return GetHistoryAsync
            (
                address,
                TransactionDirection.Incoming,
                take,
                afterHash
            );
        }

        public Task<IEnumerable<TransactionReceipt>> GetOutgoingHistoryAsync(
            string address,
            int take,
            string afterHash)
        {
            return GetHistoryAsync
            (
                address,
                TransactionDirection.Outgoing,
                take,
                afterHash
            );
        }

        private async Task<IEnumerable<TransactionReceipt>> GetHistoryAsync(
            string address,
            TransactionDirection transactionDirection,
            int take,
            string afterHash)
        {
            var continuationToken = await _transactionReceiptRepository.CreateContinuationTokenAsync
            (
                address,
                transactionDirection,
                afterHash
            );

            var (transactionReceipts, _) = await _transactionReceiptRepository.GetAsync
            (
                address,
                transactionDirection,
                take,
                continuationToken
            );

            return transactionReceipts;
        }
        
        
    }
}
