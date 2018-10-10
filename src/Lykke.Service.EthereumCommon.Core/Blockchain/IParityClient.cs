using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface IParityClient : IRpcClient
    {
        Task<object> GetNextNonceAsync(
            string address);

        Task<object> TraceTransactionAsync(
            string transactionHash);
    }
}
