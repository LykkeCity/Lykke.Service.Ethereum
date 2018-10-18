using System.Numerics;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface IEthApiClient
    {
        Task<BigInteger> GetBalanceAsync(
            string address);
        
        Task<BigInteger> GetBalanceAsync(
            string address,
            BigInteger blockNumber);

        Task<BigInteger> GetBestBlockNumberAsync();

        Task<BlockResult> GetBlockAsync(
            bool includeTransactions);
        
        Task<BlockResult> GetBlockAsync(
            string blockHash,
            bool includeTransactions);

        Task<BlockResult> GetBlockAsync(
            BigInteger blockNumber,
            bool includeTransactions);
        
        Task<string> GetCodeAsync(
            string address);
        
        Task<BigInteger> GetGasPriceAsync();

        Task<TransactionResult> GetTransactionAsync(
            string transactionHash);
        
        Task<TransactionReceiptResult> GetTransactionReceiptAsync(
            string transactionHash);

        Task<string> SendRawTransactionAsync(
            string transactionData);
    }
}
