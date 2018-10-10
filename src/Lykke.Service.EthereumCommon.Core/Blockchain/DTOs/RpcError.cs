using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Ethereum.Core.Blockchain.DTOs
{
    [JsonObject]
    public class RpcError
    {
        [JsonConstructor]
        private RpcError(
             int code,
             string message,
             JToken data)
        {
            
        }
        
        /// <summary>
        ///    RPC error code.
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; }

        /// <summary>
        ///    Error data.
        /// </summary>
        [JsonProperty("data")]
        public JToken Data { get; }
        
        /// <summary>
        ///    Error message.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; }
    }
}
