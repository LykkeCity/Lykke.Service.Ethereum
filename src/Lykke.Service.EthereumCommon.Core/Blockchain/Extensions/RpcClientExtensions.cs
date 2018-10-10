using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    public static class RpcClientExtensions
    {
        private static readonly string[] WalletCodeVariants = 
        {
            "0x0", // Ethereum, EthereumClassic
            "0x00" // Rootstock
        };
        
        
        public static async Task<bool> IsContractAsync(
            this IRpcClient rpcClient,
            string address)
        {
            var code = await rpcClient.GetCodeAsync(address);

            return !WalletCodeVariants.Contains(code);
        }
        
        public static async Task<bool> IsWalletAsync(
            this IRpcClient rpcClient,
            string address)
        {
            var code = await rpcClient.GetCodeAsync(address);

            return WalletCodeVariants.Contains(code);
        }
    }
}
