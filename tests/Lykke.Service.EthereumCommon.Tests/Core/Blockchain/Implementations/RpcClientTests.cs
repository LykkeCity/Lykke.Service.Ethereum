using System.Numerics;
using System.Threading.Tasks;
using FluentAssertions;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    [TestClass]
    public class RpcClientTests
    {
        private readonly RpcClient _rpcClient;
        
        public RpcClientTests()
        {
            _rpcClient = new RpcClient
            (
                new RpcClientCoreMock("RpcClient")
            );
        }


        [TestMethod]
        public async Task GetBalanceAsync()
        {
            (await _rpcClient.GetBalanceAsync("0x57f4037aa7cb557e5371c822715f4d58cfd68850"))
                .Should()
                .Be(BigInteger.Parse("27373492288275529"));
        }
        
        [TestMethod]
        public async Task GetBalanceAsync__Block_Number_Passed()
        {
            (await _rpcClient.GetBalanceAsync("0x57f4037aa7cb557e5371c822715f4d58cfd68850", new BigInteger(42)))
                .Should()
                .Be(BigInteger.Parse("27373492288275528"));
        }

        [TestMethod]
        public async Task GetBestBlockNumberAsync()
        {
            (await _rpcClient.GetBestBlockNumberAsync())
                .Should()
                .Be(BigInteger.Parse("815294"));
        }

        [TestMethod]
        public async Task GetBlockAsync()
        {            
            ValidateBlock
            (
                await _rpcClient.GetBlockAsync(false),
                false
            );
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Transactions_Included()
        {
            ValidateBlock
            (
                await _rpcClient.GetBlockAsync(true),
                true
            );
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Hash_Passed()
        {
            ValidateBlock
            (
                await _rpcClient.GetBlockAsync("0x35c9a41ae1380141d1163c3ada714a924842fb7519f1a07ae2c21a94d2fa84be", false),
                false
            );
        }

        [TestMethod]
        public async Task GetBlockAsync__Block_Hash_Passed_And_Transactions_Included()
        {
            ValidateBlock
            (
                await _rpcClient.GetBlockAsync("0x35c9a41ae1380141d1163c3ada714a924842fb7519f1a07ae2c21a94d2fa84be", true),
                true
            );
        }

        [TestMethod]
        public async Task GetBlockAsync__Block_Hash_Passed_But_Block_Does_Not_Exist()
        {
            var block = await _rpcClient.GetBlockAsync("0xe670ec64341771606e55d6b4ca35a1a6b75ee3d5145a99d05921026d1527331", false);
            
            block
                .Should()
                .BeNull();
        }

        [TestMethod]
        public async Task GetBlockAsync__Block_Number_Passed()
        {
            ValidateBlock
            (
                await _rpcClient.GetBlockAsync(816669, false),
                false
            );
        }

        [TestMethod]
        public async Task GetBlockAsync__Block_Number_Passed_And_Transactions_Included()
        {
            ValidateBlock
            (
                await _rpcClient.GetBlockAsync(816669, true),
                true
            );
        }

        [TestMethod]
        public async Task GetBlockAsync__Block_Number_Passed_But_Block_Does_Not_Exist()
        {
            var block = await _rpcClient.GetBlockAsync(436, false);

            block
                .Should()
                .BeNull();
        }

        [TestMethod]
        public async Task GetCodeAsync()
        {
            (await _rpcClient.GetCodeAsync("0x57f4037aa7cb557e5371c822715f4d58cfd68850"))
                .Should()
                .Be("0x600180600b6000396000f3");
        }

        [TestMethod]
        public async Task GetGasPriceAsync()
        {
            (await _rpcClient.GetGasPriceAsync())
                .Should()
                .Be(BigInteger.Parse("255"));
        }

        [TestMethod]
        public async Task GetTransactionAsync()
        {
            var transaction = await _rpcClient.GetTransactionAsync("0x058c2496f3d1387ef8ec349f0091d1806112f0d9d983c9aa88cbdb0bca8ae073");

            transaction.Value
                .Should()
                .Be(BigInteger.Parse("4290000000000000"));

            transaction.From
                .Should()
                .Be("0x49f10f59d693f0c7f3d2449da03e8f75f1535c68");

            transaction.Gas
                .Should()
                .Be(BigInteger.Parse("50000"));

            transaction.Input
                .Should()
                .Be("0x0c5a9990");

            transaction.Nonce
                .Should()
                .Be(BigInteger.Parse("21"));

            transaction.To
                .Should()
                .Be("0x0000000000000000000000000000000001000006");

            transaction.BlockHash
                .Should()
                .Be("0xb63060040b598878577064d070cf0f3ef52fa70d5ba46223266bd25f59b7f7a1");

            transaction.BlockNumber
                .Should()
                .Be(BigInteger.Parse("6139707"));
            
            transaction.GasPrice
                .Should()
                .Be(BigInteger.Parse("20000000000"));
            
            transaction.TransactionHash
                .Should()
                .Be("0x058c2496f3d1387ef8ec349f0091d1806112f0d9d983c9aa88cbdb0bca8ae073");
            
            transaction.TransactionIndex
                .Should()
                .Be(BigInteger.Parse("0"));
        }

        [TestMethod]
        public async Task GetTransactionAsync__Transaction_Is_Pending()
        {
            var transaction = await _rpcClient.GetTransactionAsync("0x058c2496f3d1387ef8ec349f0091d1806112f0d9d983c9aa88cbdb0bca8ae074");

            transaction.Value
                .Should()
                .Be(BigInteger.Parse("4290000000000000"));

            transaction.From
                .Should()
                .Be("0x49f10f59d693f0c7f3d2449da03e8f75f1535c68");

            transaction.Gas
                .Should()
                .Be(BigInteger.Parse("50000"));

            transaction.Input
                .Should()
                .Be("0x0c5a9990");

            transaction.Nonce
                .Should()
                .Be(BigInteger.Parse("21"));

            transaction.To
                .Should()
                .Be("0x0000000000000000000000000000000001000006");

            transaction.BlockHash
                .Should()
                .BeNull();

            transaction.BlockNumber
                .Should()
                .BeNull();
            
            transaction.GasPrice
                .Should()
                .Be(BigInteger.Parse("20000000000"));
            
            transaction.TransactionHash
                .Should()
                .Be("0x058c2496f3d1387ef8ec349f0091d1806112f0d9d983c9aa88cbdb0bca8ae074");
            
            transaction.TransactionIndex
                .Should()
                .BeNull();
        }

        [TestMethod]
        public async Task GetTransactionReceiptAsync()
        {
            var transactionReceipt = await _rpcClient.GetTransactionReceiptAsync("0x058c2496f3d1387ef8ec349f0091d1806112f0d9d983c9aa88cbdb0bca8ae073");

            transactionReceipt.BlockNumber
                .Should()
                .Be(BigInteger.Parse("815663"));

            transactionReceipt.Status
                .Should()
                .Be(BigInteger.Parse("1"));

            transactionReceipt.BlockHash
                .Should()
                .Be("0xb63060040b598878577064d070cf0f3ef52fa70d5ba46223266bd25f59b7f7a1");

            transactionReceipt.ContractAddress
                .Should()
                .BeNull();

            transactionReceipt.CumulativeGasUsed
                .Should()
                .Be(BigInteger.Parse("69280"));
            
            transactionReceipt.GasUsed
                .Should()
                .Be(BigInteger.Parse("69280"));

            transactionReceipt.TransactionHash
                .Should()
                .Be("0x058c2496f3d1387ef8ec349f0091d1806112f0d9d983c9aa88cbdb0bca8ae073");

            transactionReceipt.TransactionIndex
                .Should()
                .Be(BigInteger.Parse("0"));
        }

        [TestMethod]
        public async Task GetTransactionReceiptAsync__Transaction_Is_Pending()
        {
            var transactionReceipt = await _rpcClient.GetTransactionReceiptAsync("0x058c2496f3d1387ef8ec349f0091d1806112f0d9d983c9aa88cbdb0bca8ae074");

            transactionReceipt
                .Should()
                .BeNull();
        }

        [TestMethod]
        public async Task SendRawTransactionAsync()
        {
            (await _rpcClient.SendRawTransactionAsync("0xd46e8dd67c5d32be8d46e8dd67c5d32be8058bb8eb970870f072445675058bb8eb970870f072445675"))
                .Should()
                .Be("0xe670ec64341771606e55d6b4ca35a1a6b75ee3d5145a99d05921026d1527331");
        }

        private void ValidateBlock(
            BlockResult block,
            bool validateTransactions)
        {
            block.BlockHash
                .Should()
                .Be("0x35c9a41ae1380141d1163c3ada714a924842fb7519f1a07ae2c21a94d2fa84be");

            block.ExtraData
                .Should()
                .Be("0x00");

            block.GasLimit
                .Should()
                .Be(BigInteger.Parse("6800000"));
            
            block.GasUsed
                .Should()
                .Be(BigInteger.Parse("69280"));
            
            block.Difficulty
                .Should()
                .Be(BigInteger.Parse("211494208190328910673"));

            block.LogsBloom
                .Should()
                .Be("0x00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000004000000000000000000000000000000000000000000080000000000000000000000000000000000000000000000800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000");
            
            block.Miner
                .Should()
                .Be("0x12d3178a62ef1f520944534ed04504609f7307a1");

            block.Nonce
                .Should()
                .Be("0xe04d296d2460cfb8472af2c5fd05b5a214109c25688d3704aed5484f9a7792f2");
            
            block.Number
                .Should()
                .Be(BigInteger.Parse("816669"));

            block.ParentHash
                .Should()
                .Be("0x01a73fd568ed1d5ae736dac4a6dd71eb0e4fea311928bcc230385e422ac52b6a");

            block.ReceiptsRoot
                .Should()
                .Be("0x50a78e35c9cc361b311e075eab1b873fa737891b5954d0b62454274af9e1d01c");
            
            block.Sha3Uncles
                .Should()
                .Be("0x218401f4f21651fcf65b94b22acff555b231b2d2c5e240a878a5ed38ff39e30b");
            
            block.Size
                .Should()
                .Be(BigInteger.Parse("6516"));
            
            block.StateRoot
                .Should()
                .Be("0xca4734e3fc55d0e982ae844708a31aca28f21aa4bf01ab69ddb1535662fed6a0");
            
            block.Timestamp
                .Should()
                .Be(BigInteger.Parse("1539233825"));

            block.TotalDifficulty
                .Should()
                .Be(BigInteger.Parse("127161947801100775913345927"));
            
            block.TransactionHashes
                .Should()
                .BeEquivalentTo(
                    "0xd312d4495e37c95c52590b99f27e610d486429fd8438288c7db1032cf2a4c888",
                    "0x301f600adf4edf71b9fa8d8e8d4d1a2de606476eaa1639bcbd5a4cd0f793e152");
            
            block.TransactionsRoot
                .Should()
                .Be("0x484e659de5ad5572d55811be66dd1293af60c67a819aa8b6133f2714bff69256");
            
            if (validateTransactions)
            {
                block.Transactions
                    .Should()
                    .NotBeNull();
                
                block.Transactions?.Length
                    .Should()
                    .Be(2);
            }
            else
            {
                block.Transactions
                    .Should()
                    .BeNull();
            }

            block.Uncles
                .Should()
                .BeEquivalentTo(
                    "0xd51147d05a2ef60966fb9e54ca140dc01384e35e06322e1e276100b29f4733a2",
                    "0x0d4a5432f330641e4c199df16081059cc4eaa783a274f0c19ffccf1b21060220",
                    "0x1a938ccb749d9f486aae37555ad880b857fd780bdf97eb1d3b50aa5480f0b681",
                    "0x25271013b0b137e0b8bac09a1a925111d930640cbb547f43f04658f3a5227974",
                    "0xe4c2dfd814c91c5d760fd9886f895396f81c4ebb8ff2fd18eba50c70b32c5db2");
        }
    }
}
