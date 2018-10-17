using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    [TestClass]
    public class TransactionBuilderCoreTests
    {
        [TestMethod]
        public void BuildRawTransactionAsync()
        {
            var (transactionElements, privateKey, expectedRawTransaction) = TransactionBuilderTestData.Restore("NonEip155Transaction.json");
            
            var transactionBuilderCore = new TransactionBuilderCore();
            
            transactionBuilderCore.BuildRawTransaction(transactionElements, privateKey)
                .Should()
                .Equal(expectedRawTransaction);
        }
    }
}
