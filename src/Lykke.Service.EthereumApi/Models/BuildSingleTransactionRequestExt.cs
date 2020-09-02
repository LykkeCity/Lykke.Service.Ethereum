using Lykke.Service.BlockchainApi.Contract.Transactions;
using Newtonsoft.Json;

namespace Lykke.Service.EthereumApi.Models
{
    public class BuildSingleTransactionRequestExt : BuildSingleTransactionRequest
    {
        [JsonProperty("nonce")]
        public long? Nonce { get; set; }
    }
}
