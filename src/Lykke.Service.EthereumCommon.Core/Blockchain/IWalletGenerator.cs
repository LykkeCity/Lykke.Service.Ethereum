using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface IWalletGenerator
    {
        Task<(string Address, string PrivateKey)> CreateWalletAsync();
    }
}
