using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Domain.Services
{
    public interface IBlockchainService
    {   
        Task<BigInteger> GetBalanceAsync(
            string address,
            BigInteger blockNumber);
        
        Task<BigInteger> GetBestTrustedBlockNumberAsync();

        Task<TransactionResult> GetTransactionResultAsync(
            string hash);
        
        Task<IEnumerable<TransactionReceipt>> GetTransactionReceiptsAsync(
            BigInteger blockNumber);
    }
}
