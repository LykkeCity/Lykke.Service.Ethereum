using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using FluentAssertions;
using Lykke.Service.Ethereum.Core.Blockchain.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nethereum.RLP;


// ReSharper disable ConvertToConstant.Local
namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    [TestClass]
    public class TransactionBuilderTests
    {
        [TestMethod]
        public void BuildRawTransferTransaction()
        {
            var to = "0xC2D7CF95645D33006175B78989035C7c9061d3F9";
            var amount = new BigInteger(3 * Math.Pow(10,18));
            var nonce = new BigInteger(42);
            var gasPrice = new BigInteger(10000);
            var gasLimit = new BigInteger(21000);
            var privateKey = "0x3a1076bf45ab87712ad64ccb3b10217737f7faacbf2872e88fdd9a537d8fe266";
            
            
            
            var transactionBuilderCoreMock = new Mock<ITransactionBuilderCore>();
            
            transactionBuilderCoreMock
                .Setup(x => x.BuildRawTransaction(It.IsAny<byte[][]>(), It.IsAny<byte[]>()))
                .Callback<IEnumerable<byte[]>, byte[]>((actualTransactionElements, actualPrivateKey) =>
                {
                    actualTransactionElements.Concat()
                        .Should()
                        .Equal
                        (
                            new []
                            {
                                nonce.ToBytesForRLPEncoding(),
                                gasPrice.ToBytesForRLPEncoding(),
                                gasLimit.ToBytesForRLPEncoding(),
                                to.HexToByteArray(),
                                amount.ToBytesForRLPEncoding(),
                                Array.Empty<byte>()
                            }.Concat()
                        );

                    actualPrivateKey
                        .Should()
                        .Equal(privateKey.HexToByteArray());
                })
                .Returns(Array.Empty<byte>());
            
            
            new TransactionBuilder(transactionBuilderCoreMock.Object).BuildRawTransferTransaction
            (
                to: to,
                amount: amount,
                nonce: nonce,
                gasPrice: gasPrice,
                gasLimit: gasLimit,
                privateKey: privateKey
            );
        }
    }
}
