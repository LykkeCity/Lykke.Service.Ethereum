using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.DistributedLock
{
    public interface IDistributedLockToken
    {
        Task ReleaseAsync();
    }
}
