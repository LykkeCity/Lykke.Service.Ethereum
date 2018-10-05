using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface IParityClient : IWeb3Client
    {
        Task<object> GetNextNonceAsync(
            string address);

        Task<object> TraceTransactionAsync(
            string transactionHash);
    }
}
