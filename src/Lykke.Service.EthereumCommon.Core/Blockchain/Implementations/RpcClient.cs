using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Lykke.Service.Ethereum.Core.Blockchain.Extensions;
using Newtonsoft.Json.Linq;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class RpcClient : IRpcClient
    {
        private readonly IRpcClientCore _core;

        
        public RpcClient(
            IRpcClientCore core)
        {
            _core = core;
        }


        protected virtual string BestBlockIdentifier { get; } = "latest";


        /// <inheritdoc />
        public virtual Task<BigInteger> GetBalanceAsync(
            string address)
        {
            return GetBalanceAsync(address, BestBlockIdentifier);
        }
        
        /// <inheritdoc />
        public virtual Task<BigInteger> GetBalanceAsync(
            string address,
            BigInteger blockNumber)
        {
            return GetBalanceAsync(address, blockNumber.ToString());
        }
        
        protected virtual async Task<BigInteger> GetBalanceAsync(
            string address,
            string blockIdentifier)
        {
            var request = new RpcRequest(method: "eth_getBalance", address, blockIdentifier);
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();
            
            return response.ResultValue<BigInteger>();
        }

        /// <inheritdoc />
        public virtual async Task<BigInteger> GetBestBlockNumberAsync()
        {
            var request = new RpcRequest(method: "eth_blockNumber");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();
            
            return response.ResultValue<BigInteger>();
        }

        /// <inheritdoc />
        public virtual async Task<BlockResult> GetBlockAsync(
            bool includeTransactions)
        {
            var request = new RpcRequest(method: "eth_getBlockByNumber", BestBlockIdentifier, $"{includeTransactions}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();
            
            return GetBlock(response, includeTransactions);
        }
        
        /// <inheritdoc />
        public virtual async Task<BlockResult> GetBlockAsync(
            string blockHash,
            bool includeTransactions)
        {
            var request = new RpcRequest(method: "eth_getBlockByHash", blockHash, $"{includeTransactions}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();
            
            return GetBlock(response, includeTransactions);
        }
        
        /// <inheritdoc />
        public virtual async Task<BlockResult> GetBlockAsync(
            BigInteger blockNumber,
            bool includeTransactions)
        {
            var request = new RpcRequest(method: "eth_getBlockByNumber", $"{blockNumber}", $"{includeTransactions}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();

            return GetBlock(response, includeTransactions);
        }
        
        protected virtual BlockResult GetBlock(
            RpcResponse response,
            bool includeTransactions)
        {
            var transactionTokens = response.Result.Values("transactions").ToList();

            IEnumerable<TransactionResult> GetTransactions()
            {
                if (includeTransactions)
                {
                    return transactionTokens.Select(x => new TransactionResult
                    (
                        blockHash: x.Value<string>("blockHash"),
                        blockNumber: x.Value<BigInteger?>("blockNumber"),
                        from: x.Value<string>("from"),
                        gas: x.Value<BigInteger>("gas"),
                        gasPrice: x.Value<BigInteger>("gasPrice"),
                        input: x.Value<string>("input"),
                        nonce: x.Value<BigInteger>("nonce"),
                        to: x.Value<string>("to"),
                        transactionIndex: x.Value<BigInteger?>("transactionIndex"),
                        transactionHash: x.Value<string>("hash"),
                        value: x.Value<BigInteger>("value")
                    ));
                }
                else
                {
                    return null;
                }
            }
            
            IEnumerable<string> GetTransactionHashes()
            {
                return transactionTokens.Select(x => x.Value<string>("hash"));
            }
            
            return new BlockResult
            (
                author: response.ResultValue<string>("author"),
                blockHash: response.ResultValue<string>("blockHash"),
                difficulty: response.ResultValue<BigInteger>("difficulty"),
                extraData: response.ResultValue<string>("extraData"),
                gasLimit: response.ResultValue<BigInteger>("gasLimit"),
                gasUsed: response.ResultValue<BigInteger>("gasUsed"),
                logsBloom: response.ResultValue<string>("logsBloom"),
                miner: response.ResultValue<string>("miner"),
                nonce: response.ResultValue<string>("nonce"),
                number: response.ResultValue<BigInteger?>("number"),
                parentHash: response.ResultValue<string>("parentHash"),
                receiptsRoot: response.ResultValue<string>("receiptsRoot"),
                sealFields: response.Result.Values<string>("sealFields"),
                sha3Uncles: response.ResultValue<string>("sha3Uncles"),
                size: response.ResultValue<BigInteger>("size"),
                stateRoot: response.ResultValue<string>("stateRoot"),
                timestamp: response.ResultValue<BigInteger>("timestamp"),
                totalDifficulty: response.ResultValue<BigInteger>("totalDifficulty"),
                transactionHashes: GetTransactionHashes(),
                transactions: GetTransactions(),
                transactionsRoot: response.ResultValue<string>("transactionsRoot"),
                uncles: response.Result.Values<string>("uncles")
            );
        }

        /// <inheritdoc />
        public virtual Task<string> GetCodeAsync(
            string address)
        {
            return GetCodeAsync(address, BestBlockIdentifier);
        }
        
        protected virtual async Task<string> GetCodeAsync(
            string address,
            string blockIdentifier)
        {
            var request = new RpcRequest(method: "eth_getCode", address, blockIdentifier);
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();
            
            return response.ResultValue<string>();
        }

        /// <inheritdoc />
        public async Task<BigInteger> GetGasPriceAsync()
        {
            var request = new RpcRequest(method: "eth_gasPrice");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();
            
            return response.ResultValue<BigInteger>();
        }

        /// <inheritdoc />
        public virtual async Task<TransactionResult> GetTransactionAsync(
            string transactionHash)
        {
            var request = new RpcRequest(method: "eth_sendRawTransaction", transactionHash);
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();

            return new TransactionResult
            (
                blockHash: response.ResultValue<string>("blockHash"),
                blockNumber: response.ResultValue<BigInteger?>("blockNumber"),
                from: response.ResultValue<string>("from"),
                gas: response.ResultValue<BigInteger>("gas"),
                gasPrice: response.ResultValue<BigInteger>("gasPrice"),
                input: response.ResultValue<string>("input"),
                nonce: response.ResultValue<BigInteger>("nonce"),
                to: response.ResultValue<string>("to"),
                transactionIndex: response.ResultValue<BigInteger?>("transactionIndex"),
                transactionHash: response.ResultValue<string>("hash"),
                value: response.ResultValue<BigInteger>("value")
            );
        }

        /// <inheritdoc />
        public virtual async Task<TransactionReceiptResult> GetTransactionReceiptAsync(
            string transactionHash)
        {
            var request = new RpcRequest(method: "eth_getTransactionReceipt", transactionHash);
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();

            var result = response.Result;
            
            return new TransactionReceiptResult
            (
                blockHash: response.ResultValue<string>("blockHash"),
                blockNumber: response.ResultValue<BigInteger>("blockNumber"),
                contractAddress: response.ResultValue<string>("contractAddress"),
                cumulativeGasUsed: response.ResultValue<BigInteger>("cumulativeGasUsed"),
                gasUsed: response.ResultValue<BigInteger>("gasUsed"),
                status: response.ResultValue<BigInteger>("status"),
                transactionIndex: response.ResultValue<BigInteger>("transactionIndex"),
                transactionHash: response.ResultValue<string>("transactionHash")
            );
        }

        /// <inheritdoc />
        public virtual async Task<string> SendRawTransactionAsync(
            string transactionData)
        {
            var request = new RpcRequest(method: "eth_sendRawTransaction", $"{{\"data\":\"{transactionData}\"}}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessResult();
            
            return response.ResultValue<string>();
        }
    }
}
