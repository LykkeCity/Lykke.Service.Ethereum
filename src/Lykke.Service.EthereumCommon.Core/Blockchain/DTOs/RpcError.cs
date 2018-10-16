using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Ethereum.Core.Blockchain.DTOs
{
    [JsonObject]
    public class RpcError
    {
        [JsonConstructor]
        public RpcError(
             int code,
             JToken data,
             string message)
        {
            Code = code;
            Data = data;
            Message = message;
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
