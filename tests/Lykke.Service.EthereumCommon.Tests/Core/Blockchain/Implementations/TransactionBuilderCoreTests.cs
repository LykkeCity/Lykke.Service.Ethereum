using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    [TestClass]
    public class TransactionBuilderCoreTests
    {
        [TestMethod]
        public async Task BuildRawTransactionAsync()
        {
            byte[][] transactionElements = null;
            byte[] privateKey = null;
            
            var transactionBuilderCore = new TransactionBuilderCore();
            byte[] expectedRawTransaction = null;
            byte[] actualRawTransaction = await transactionBuilderCore.BuildRawTransactionAsync(transactionElements, privateKey);
            
            actualRawTransaction
                .Should()
                .Equal(expectedRawTransaction);
        }
    }
}
