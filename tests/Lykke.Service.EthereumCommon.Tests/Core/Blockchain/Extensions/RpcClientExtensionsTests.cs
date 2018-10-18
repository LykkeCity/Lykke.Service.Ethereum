using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    [TestClass]
    public class RpcClientExtensionsTests
    {
        [DataTestMethod]
        [DataRow("0x0", false, DisplayName = "Common wallet code")]
        [DataRow("0x00", false, DisplayName = "Rootstock wallet code")]
        [DataRow("0x600180600b6000396000f3", true, DisplayName = "Contract code")]
        public async Task IsContractAsync(
            string code,
            bool isContract)
        {
            var clientMock = new Mock<IEthApiClient>();

            clientMock
                .Setup(x => x.GetCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(code);

            var client = clientMock.Object;

            (await client.IsContractAsync("0x00107149f90738cd63eca6ca2a3c979714d41752"))
                .Should()
                .Be(isContract);
        }
        
        [DataTestMethod]
        [DataRow("0x0", true, DisplayName = "Common wallet code")]
        [DataRow("0x00", true, DisplayName = "Rootstock wallet code")]
        [DataRow("0x600180600b6000396000f3", false, DisplayName = "Contract code")]
        public async Task IsWalletAsync(
            string code,
            bool isWallet)
        {
            var clientMock = new Mock<IEthApiClient>();

            clientMock
                .Setup(x => x.GetCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(code);

            var client = clientMock.Object;

            (await client.IsWalletAsync("0x00107149f90738cd63eca6ca2a3c979714d41752"))
                .Should()
                .Be(isWallet);
        }
    }
}
