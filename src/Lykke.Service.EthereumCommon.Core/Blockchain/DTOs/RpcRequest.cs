using System;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Core.Blockchain.DTOs
{
    [JsonObject]
    public sealed class RpcRequest
    {
        [JsonConstructor]
        public RpcRequest(
            string id,
            string jsonRpcVersion,
            string method,
            params string[] parameters)
        {
            Id = id;
            JsonRpcVersion = jsonRpcVersion;
            Method = method;
            Parameters = parameters;
        }
        
        public RpcRequest(
            string method,
            params string[] parameters)
        {
            Id = Guid.NewGuid().ToString();
            JsonRpcVersion = "2.0";
            Method = method;
            Parameters = parameters;
        }
        
        
        /// <summary>
        ///    Request id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }
        
        /// <summary>
        ///    Version of the JsonRpc to be used.
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string JsonRpcVersion { get; }
        
        /// <summary>
        ///    Name of the target method.
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; }
        
        /// <summary>
        ///    Parameters to invoke the method with.
        /// </summary>
        [JsonProperty("params")]
        public string[] Parameters { get; }
    }
}
