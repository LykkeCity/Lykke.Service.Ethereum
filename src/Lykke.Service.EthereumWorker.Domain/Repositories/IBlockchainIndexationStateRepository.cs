using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.DistributedLock;

namespace Lykke.Service.Ethereum.Domain.Repositories
{
    public interface IBlockchainIndexationStateRepository
    {
        Task<BlockchainIndexationState> GetOrCreateAsync();

        Task UpdateAsync(
            BlockchainIndexationState state);

        Task<IDistributedLockToken> WaitLockAsync();
    }
}
