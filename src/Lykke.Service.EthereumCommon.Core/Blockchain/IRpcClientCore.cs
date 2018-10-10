using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface IRpcClientCore
    {
        /// <summary>
        ///    Sends RPC request.
        /// </summary>
        Task<RpcResponse> SendRpcRequestAsync(
            RpcRequest request);

        /// <summary>
        ///    Sends RPC request and writes telemetry, if telemetry is enabled, or just sends RPC request otherwise.
        /// </summary>
        Task<RpcResponse> SendRpcRequestWithTelemetryAsync(
            RpcRequest request);
    }
}
