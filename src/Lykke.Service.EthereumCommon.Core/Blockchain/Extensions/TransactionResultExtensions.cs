using Lykke.Service.Ethereum.Core.Blockchain.DTOs;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    public static class TransactionResultExtensions
    {
        public static bool IsContractCreationTransaction(
            this TransactionResult transaction)
        {
            return transaction.To == null;
        }
        
        public static bool IsPendingTransaction(
            this TransactionResult transaction)
        {
            return !transaction.BlockNumber.HasValue;
        }
    }
}
