using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    [TestClass]
    public class Eip155TransactionBuilderCoreTests
    {
        [DataTestMethod]
        [DataRow(BlockchainType.Ethereum, true)]
        [DataRow(BlockchainType.Ethereum, false)]
        [DataRow(BlockchainType.EthereumClassic, true)]
        [DataRow(BlockchainType.EthereumClassic, false)]
        [DataRow(BlockchainType.Rootstock, true)]
        [DataRow(BlockchainType.Rootstock, false)]
        public void BuildRawTransactionAsync(
            BlockchainType blockchainType,
            bool isMainNet)
        {
            var testDataFileName = isMainNet 
                ? $"{blockchainType.ToString()}MainNetTransaction.json"
                : $"{blockchainType.ToString()}TestNetTransaction.json";
            
            
            var (transactionElements, privateKey, expectedRawTransaction) = TransactionBuilderTestData.Restore(testDataFileName);
            
            var transactionBuilderCore = new Eip155TransactionBuilderCore(blockchainType, isMainNet);

            transactionBuilderCore.BuildRawTransaction(transactionElements, privateKey)
                .Should()
                .Equal(expectedRawTransaction);
        }
    }
}
