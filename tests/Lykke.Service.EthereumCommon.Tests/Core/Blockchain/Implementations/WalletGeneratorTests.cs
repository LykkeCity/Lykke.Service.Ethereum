using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    [TestClass]
    public class WalletGeneratorTests
    {
        
        [TestMethod]
        public async Task CreateWalletAsync()
        {
            var walletGenerator = new WalletGenerator();

            var (address, privateKey) = await walletGenerator.CreateWalletAsync();

            address
                .Should()
                .StartWith("0x");
            
            address
                .Should()
                .HaveLength(42);

            privateKey
                .Should()
                .StartWith("0x");

            privateKey
                .Should()
                .HaveLength(66);
        }
    }
}
