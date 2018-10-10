using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    public static class RpcClientExtensions
    {
        public static async Task<bool> IsContractAsync(
            this IRpcClient rpcClient,
            string address)
        {
            var code = await rpcClient.GetCodeAsync(address);

            return code != "0x";
        }
        
        public static async Task<bool> IsWalletAsync(
            this IRpcClient rpcClient,
            string address)
        {
            var code = await rpcClient.GetCodeAsync(address);

            return code == "0x";
        }
    }
}
