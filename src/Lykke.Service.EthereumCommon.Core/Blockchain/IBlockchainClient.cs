using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface IBlockchainClient
    {
        Task<BigInteger> GetBalanceAsync(
            BlockchainAddress blockchainAddress,
            BlockSelector block);

        Task<object> GetCodeAsync(
            BlockchainAddress blockchainAddress);
        
        Task<BigInteger> GetGasPriceAsync();

        Task<object> GetTransactionAsync(
            string transactionHash);
        
        Task<object> GetTransactionReceiptAsync(
            string transactionHash);

        Task<object> SendRawTransactionAsync(
            string transactionData);
    }
}
