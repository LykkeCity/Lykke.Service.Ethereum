using Lykke.Service.Ethereum.Core.Blockchain.DTOs;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    public static class TransactionReceiptResultExtensions
    {
        public static bool IsFailed(
            this TransactionReceiptResult transactionReceipt)
        {
            return transactionReceipt.Status == 0;
        }
        
        public static bool IsSucceeded(
            this TransactionReceiptResult transactionReceipt)
        {
            return transactionReceipt.Status == 1;
        }
    }
}
