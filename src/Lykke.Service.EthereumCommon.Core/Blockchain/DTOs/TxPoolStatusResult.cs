using System.Numerics;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Core.Blockchain.DTOs
{
    public sealed class TxPoolStatusResult
    {
        public TxPoolStatusResult(
            BigInteger pendingTransactionsCount,
            BigInteger queuedTransactionsCount)
        {
            PendingTransactionsCount = pendingTransactionsCount;
            QueuedTransactionsCount = queuedTransactionsCount;
        }
        
        [JsonProperty("pending")]
        public BigInteger PendingTransactionsCount { get; }
        
        [JsonProperty("queued")]
        public BigInteger QueuedTransactionsCount { get; }
    }
}
