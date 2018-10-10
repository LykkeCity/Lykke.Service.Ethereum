using System;
using System.Numerics;
using System.Threading.Tasks;
using FluentAssertions;
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
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Transactions_Included()
        {
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Hash_Passed()
        {
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Hash_Passed_And_Transactions_Included()
        {
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Number_Passed()
        {
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public async Task GetBlockAsync__Block_Number_Passed_And_Transactions_Included()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
        public async Task SendRawTransactionAsync()
        {
            (await _rpcClient.SendRawTransactionAsync("0xd46e8dd67c5d32be8d46e8dd67c5d32be8058bb8eb970870f072445675058bb8eb970870f072445675"))
                .Should()
                .Be("0xe670ec64341771606e55d6b4ca35a1a6b75ee3d5145a99d05921026d1527331");
        }
    }
}
