using System.Numerics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    [TestClass]
    public class HexStringExtensionsTests
    {
        [DataTestMethod]
        [DataRow("0x0", "0")]
        [DataRow("0x5208", "21000")]
        [DataRow("0x9184e72a000", "10000000000000")]
        [DataRow("0", "0")]
        [DataRow("5208", "21000")]
        [DataRow("9184e72a000", "10000000000000")]
        public void HexToBigInteger(
            string actualHexString,
            string expectedBigInteger)
        {
            actualHexString.HexToBigInteger()
                .Should()
                .Be(BigInteger.Parse(expectedBigInteger));
        }
    }
}
