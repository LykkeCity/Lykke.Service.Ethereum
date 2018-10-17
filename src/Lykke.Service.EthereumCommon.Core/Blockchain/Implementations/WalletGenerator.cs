using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.Crypto;
using Lykke.Service.Ethereum.Core.Blockchain.Extensions;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class WalletGenerator : IWalletGenerator
    {
        public async Task<(string Address, string PrivateKey)> CreateWalletAsync()
        {
            var privateKey = Secp256K1.GeneratePrivateKey();
            var publicKey = Secp256K1.GetPublicKey(privateKey);
            var address = (await Keccak256.SumAsync(publicKey.Slice(1))).Slice(12, 32);

            return (address.ToHexString(), privateKey.ToHexString());
        }
    }
}
