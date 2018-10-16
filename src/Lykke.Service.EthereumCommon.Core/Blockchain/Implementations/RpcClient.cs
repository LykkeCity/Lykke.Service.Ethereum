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

            response.EnsureSuccessfulResult();
            
            return response.ResultValue<BigInteger>();
        }

        /// <inheritdoc />
        public virtual async Task<BigInteger> GetBestBlockNumberAsync()
        {
            var request = new RpcRequest(method: "eth_blockNumber");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();
            
            return response.ResultValue<BigInteger>();
        }

        /// <inheritdoc />
        public virtual async Task<BlockResult> GetBlockAsync(
            bool includeTransactions)
        {
            var request = new RpcRequest(method: "eth_getBlockByNumber", BestBlockIdentifier, $"{includeTransactions}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();
            
            return GetBlock(response, includeTransactions);
        }
        
        /// <inheritdoc />
        public virtual async Task<BlockResult> GetBlockAsync(
            string blockHash,
            bool includeTransactions)
        {
            var request = new RpcRequest(method: "eth_getBlockByHash", blockHash, $"{includeTransactions}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();
            
            return GetBlock(response, includeTransactions);
        }
        
        /// <inheritdoc />
        public virtual async Task<BlockResult> GetBlockAsync(
            BigInteger blockNumber,
            bool includeTransactions)
        {
            var request = new RpcRequest(method: "eth_getBlockByNumber", $"{blockNumber.ToHexString()}", $"{includeTransactions}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();

            return GetBlock(response, includeTransactions);
        }
        
        protected virtual BlockResult GetBlock(
            RpcResponse response,
            bool includeTransactions)
        {
            if (response.Result.Type != JTokenType.Null)
            {
                var transactions = response.Result.Value<JArray>("transactions").ToList();

                IEnumerable<TransactionResult> GetTransactions()
                {
                    return includeTransactions 
                        ? transactions.Select(GetTransaction)
                        : null;
                }
            
                // ReSharper disable once ImplicitlyCapturedClosure
                IEnumerable<string> GetTransactionHashes()
                {
                    return includeTransactions 
                        ? transactions.Select(x => x.Value<string>("hash"))
                        : transactions.Select(x => x.Value<string>());
                }
                
                return new BlockResult
                (
                    blockHash: response.ResultValue<string>("hash"),
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
                    sha3Uncles: response.ResultValue<string>("sha3Uncles"),
                    size: response.ResultValue<BigInteger>("size"),
                    stateRoot: response.ResultValue<string>("stateRoot"),
                    timestamp: response.ResultValue<BigInteger>("timestamp"),
                    totalDifficulty: response.ResultValue<BigInteger>("totalDifficulty"),
                    transactionHashes: GetTransactionHashes(),
                    transactions: GetTransactions(),
                    transactionsRoot: response.ResultValue<string>("transactionsRoot"),
                    uncles: response.Result.Value<JArray>("uncles").Select(x => x.Value<string>())
                );
            }
            else
            {
                return null;
            }
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

            response.EnsureSuccessfulResult();

            return response.ResultValue<string>();
        }

        /// <inheritdoc />
        public async Task<BigInteger> GetGasPriceAsync()
        {
            var request = new RpcRequest(method: "eth_gasPrice");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();
            
            return response.ResultValue<BigInteger>();
        }

        /// <inheritdoc />
        public virtual async Task<TransactionResult> GetTransactionAsync(
            string transactionHash)
        {
            var request = new RpcRequest(method: "eth_getTransactionByHash", transactionHash);
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();

            if (response.Result.Type != JTokenType.Null)
            {
                return GetTransaction(response.Result);
            }
            else
            {
                return null;
            }
        }

        protected virtual TransactionResult GetTransaction(
            JToken jToken)
        {
            return new TransactionResult
            (
                blockHash: jToken.Value<string>("blockHash"),
                blockNumber: jToken.Value<string>("blockNumber").HexToNullableBigInteger(),
                from: jToken.Value<string>("from"),
                gas: jToken.Value<string>("gas").HexToBigInteger(),
                gasPrice: jToken.Value<string>("gasPrice").HexToBigInteger(),
                input: jToken.Value<string>("input"),
                nonce: jToken.Value<string>("nonce").HexToBigInteger(),
                to: jToken.Value<string>("to"),
                transactionIndex: jToken.Value<string>("transactionIndex").HexToNullableBigInteger(),
                transactionHash: jToken.Value<string>("hash"),
                value: jToken.Value<string>("value").HexToBigInteger()
            );
        }

        /// <inheritdoc />
        public virtual async Task<TransactionReceiptResult> GetTransactionReceiptAsync(
            string transactionHash)
        {
            var request = new RpcRequest(method: "eth_getTransactionReceipt", transactionHash);
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();

            if (response.Result.Type != JTokenType.Null)
            {
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
            else
            {
                return null;
            }
        }

        /// <inheritdoc />
        public virtual async Task<string> SendRawTransactionAsync(
            string transactionData)
        {
            var request = new RpcRequest(method: "eth_sendRawTransaction", $"{{\"data\":\"{transactionData}\"}}");
            var response = await _core.SendRpcRequestWithTelemetryAsync(request);

            response.EnsureSuccessfulResult();
            
            return response.ResultValue<string>();
        }
    }
}
