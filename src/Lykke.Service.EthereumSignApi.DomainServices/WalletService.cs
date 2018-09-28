using JetBrains.Annotations;
using Nethereum.Signer;

namespace Lykke.Service.Ethereum.Domain.Services
{
    [UsedImplicitly]
    public class WalletService : IWalletService
    {
        public (string Address, string PrivateKey) CreateWallet()
        {
            var ethECKey = EthECKey.GenerateKey();

            return 
            (
                ethECKey.GetPublicAddress(),
                ethECKey.GetPrivateKey()
            );
        }
    }
}
