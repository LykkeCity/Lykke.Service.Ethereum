using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Domain.Services
{
    [UsedImplicitly]
    public class AddressService : IAddressService
    {
        private readonly IBlockchainService _blockchainService;

        
        public AddressService(
            IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        
        public async Task<bool> ValidateAsync(
            string address)
        {
            return Address.ValidateFormatAndChecksum(address, true, true)
                && await _blockchainService.IsWalletAsync(address);
        }
    }
}
