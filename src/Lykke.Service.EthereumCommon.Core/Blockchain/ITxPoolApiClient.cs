using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface ITxPoolApiClient
    {
        Task<object> GetContentAsync();

        Task<object> GetStatusAsync();
        
        Task<object> InspectAsync();
    }
}
