using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class RpcClientCoreMock : IRpcClientCore
    {
        private readonly string _blockchainDirectory;
        
        
        public RpcClientCoreMock(
            string blockchainDirectory)
        {
            _blockchainDirectory = blockchainDirectory;
        }
        
        
        public async Task<RpcResponse> SendRpcRequestAsync(
            RpcRequest request)
        {
            var responseFileDirectory = $"Core/Blockchain/Implementations/RpcResponses/{_blockchainDirectory}";
            
            var responseFileName = request.Parameters.Any() 
                ? $"{request.Method}__{string.Join('_', request.Parameters)}.json" 
                : $"{request.Method}.json";

            var responseContent = await File.ReadAllTextAsync($"{responseFileDirectory}/{responseFileName}");

            return JsonConvert.DeserializeObject<RpcResponse>(responseContent);
        }

        public Task<RpcResponse> SendRpcRequestWithTelemetryAsync(
            RpcRequest request)
        {
            return SendRpcRequestAsync(request);
        }
    }
}
