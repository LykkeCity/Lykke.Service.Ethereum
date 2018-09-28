using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.DistributedLock
{
    public interface IDistributedLock
    {
        Task<IDistributedLockToken> WaitAsync();
    }
}
