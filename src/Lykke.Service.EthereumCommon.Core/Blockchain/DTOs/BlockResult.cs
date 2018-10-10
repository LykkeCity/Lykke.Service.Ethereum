using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;

namespace Lykke.Service.Ethereum.Core.Blockchain.DTOs
{
    public sealed class BlockResult
    {
        public BlockResult(
            string author,
            string blockHash,
            BigInteger difficulty,
            string extraData,
            BigInteger gasLimit,
            BigInteger gasUsed,
            string logsBloom,
            string miner,
            string nonce,
            BigInteger? number,
            string parentHash,
            string receiptsRoot,
            IEnumerable<string> sealFields,
            string sha3Uncles,
            BigInteger size,
            string stateRoot,
            BigInteger timestamp,
            BigInteger totalDifficulty,
            IEnumerable<string> transactionHashes,
            IEnumerable<TransactionResult> transactions,
            string transactionsRoot,
            IEnumerable<string> uncles)
        {
            Author = author;
            BlockHash = blockHash;
            Difficulty = difficulty;
            ExtraData = extraData;
            GasLimit = gasLimit;
            GasUsed = gasUsed;
            LogsBloom = logsBloom;
            Miner = miner;
            Nonce = nonce;
            Number = number;
            ParentHash = parentHash;
            ReceiptsRoot = receiptsRoot;
            SealFields = sealFields.ToImmutableArray();
            Sha3Uncles = sha3Uncles;
            Size = size;
            StateRoot = stateRoot;
            Timestamp = timestamp;
            TotalDifficulty = totalDifficulty;
            TransactionHashes = transactionHashes.ToImmutableArray();
            Transactions = transactions.ToImmutableArray();
            TransactionsRoot = transactionsRoot;
            Uncles = uncles.ToImmutableArray();
        }

        /// <summary>
        ///    Block author.
        /// </summary>
        public string Author { get; }

        /// <summary>
        ///    Hash of the block.
        /// </summary>
        public string BlockHash { get; }

        /// <summary>
        ///    Integer of the difficulty for this block.
        /// </summary>
        public BigInteger Difficulty { get; }

        /// <summary>
        ///    The "extra data" field of this block.
        /// </summary>
        public string ExtraData { get; }

        /// <summary>
        ///    The maximum gas allowed in this block.
        /// </summary>
        public BigInteger GasLimit { get; }

        /// <summary>
        ///    The total used gas by all transactions in this block.
        /// </summary>
        public BigInteger GasUsed { get; }

        /// <summary>
        ///    The bloom filter for the logs of the block. Null for pending blocks.
        /// </summary>
        public string LogsBloom { get; }

        /// <summary>
        ///    The address of the beneficiary to whom the mining rewards were given.
        /// </summary>
        public string Miner { get; }

        /// <summary>
        ///     Hash of the generated proof-of-work. Null if the block is pending.
        /// </summary>
        public string Nonce { get; }

        /// <summary>
        ///    The block number. Null if the block is pending.
        /// </summary>
        public BigInteger? Number { get; }

        /// <summary>
        ///    Hash of the parent block.
        /// </summary>
        public string ParentHash { get; }

        /// <summary>
        ///    The root of the receipts trie of the block.
        /// </summary>
        public string ReceiptsRoot { get; }

        /// <summary>
        ///    Seal fields.
        /// </summary>
        public ImmutableArray<string> SealFields { get; }

        /// <summary>
        ///    SHA3 of the uncles data in the block.
        /// </summary>
        public string Sha3Uncles { get; }

        /// <summary>
        ///    The size of the block in bytes.
        /// </summary>
        public BigInteger Size { get; }

        /// <summary>
        ///    The root of the final state trie of the block.
        /// </summary>
        public string StateRoot { get; }

        /// <summary>
        ///    The unix timestamp (in seconds) for when the block was collated.
        /// </summary>
        public BigInteger Timestamp { get; }

        /// <summary>
        ///    The total difficulty of the chain until this block.
        /// </summary>
        public BigInteger TotalDifficulty { get; }

        /// <summary>
        ///    List of transaction hashes.
        /// </summary>
        public ImmutableArray<string> TransactionHashes { get; }

        /// <summary>
        ///    List of transactions.
        /// </summary>
        public ImmutableArray<TransactionResult> Transactions { get; }

        /// <summary>
        ///    The root of the transaction trie of the block.
        /// </summary>
        public string TransactionsRoot { get; }

        /// <summary>
        ///    List of uncle hashes.
        /// </summary>
        public ImmutableArray<string> Uncles { get; }
    }
}
