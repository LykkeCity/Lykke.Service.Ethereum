using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface IWeb3Client
    {
        Task<BigInteger> GetBalanceAsync(
            string address,
            string block);

        Task<object> GetBestBlockNumberAsync();

        Task<object> GetBlockWithTransactionsAsync(
            string block);

        Task<object> GetCodeAsync(
            string address);
        
        Task<object> GetGasPriceAsync();

        Task<object> GetTransactionAsync(
            string transactionHash);
        
        Task<object> GetTransactionReceiptAsync(
            string transactionHash);

        Task<object> SendRawTransactionAsync(
            string transactionData);
    }
}
