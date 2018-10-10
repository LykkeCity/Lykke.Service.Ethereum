using System.Numerics;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Core.Blockchain.DTOs
{
    public sealed class TransactionReceiptResult
    {
        public TransactionReceiptResult(
            string blockHash,
            BigInteger blockNumber,
            string contractAddress,
            BigInteger cumulativeGasUsed,
            BigInteger gasUsed,
            BigInteger status,
            BigInteger transactionIndex,
            string transactionHash)
        {
            BlockHash = blockHash;
            BlockNumber = blockNumber;
            ContractAddress = contractAddress;
            CumulativeGasUsed = cumulativeGasUsed;
            GasUsed = gasUsed;
            Status = status;
            TransactionIndex = transactionIndex;
            TransactionHash = transactionHash;
        }
        
        /// <summary>
        ///    Hash of the block where this transaction was included.
        /// </summary>
        [JsonProperty(PropertyName = "blockHash")]
        public string BlockHash { get; }

        /// <summary>
        ///    Block number where this transaction was included.
        /// </summary>
        [JsonProperty(PropertyName = "blockNumber")]
        public BigInteger BlockNumber { get; }

        /// <summary>
        ///    The contract address created, if the transaction was a contract creation, otherwise null.
        /// </summary>
        [JsonProperty(PropertyName = "contractAddress")]
        public string ContractAddress { get; }

        /// <summary>
        ///    The total amount of gas used when this transaction was executed in the block.
        /// </summary>
        [JsonProperty(PropertyName = "cumulativeGasUsed")]
        public BigInteger CumulativeGasUsed { get; }

        /// <summary>
        ///    The amount of gas used by this specific transaction alone.
        /// </summary>
        [JsonProperty(PropertyName = "gasUsed")]
        public BigInteger GasUsed { get; }

        /// <summary>
        ///    Transaction Success 1, Transaction Failed 0
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public BigInteger Status { get; }

        /// <summary>
        ///    Integer of the transactions index position in the block.
        /// </summary>
        [JsonProperty(PropertyName = "transactionIndex")]
        public BigInteger TransactionIndex { get; }

        /// <summary>
        ///    Hash of the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "transactionHash")]
        public string TransactionHash { get; }
    }
}
