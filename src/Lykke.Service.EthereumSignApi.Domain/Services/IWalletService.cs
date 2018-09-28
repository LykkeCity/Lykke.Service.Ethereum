namespace Lykke.Service.Ethereum.Domain.Services
{
    public interface IWalletService
    {
        (string Address, string PrivateKey) CreateWallet();
    }
}
