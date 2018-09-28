using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Domain.Services
{
    public interface IAddressService
    {
        Task<bool> ValidateAsync(
            [NotNull] string address);
    }
}
