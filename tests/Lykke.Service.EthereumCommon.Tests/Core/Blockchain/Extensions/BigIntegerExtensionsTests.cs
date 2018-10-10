using System.Numerics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    [TestClass]
    public class BigIntegerExtensionsTests
    {
        [DataTestMethod]
        [DataRow("0", "0x0")]
        [DataRow("21000", "0x5208")]
        [DataRow("10000000000000", "0x9184e72a000")]
        public void ToHexString(
            string actualBigInteger,
            string expectedHexString)
        {
            BigInteger.Parse(actualBigInteger).ToHexString()
                .Should()
                .Be(expectedHexString);
        }
    }
}
