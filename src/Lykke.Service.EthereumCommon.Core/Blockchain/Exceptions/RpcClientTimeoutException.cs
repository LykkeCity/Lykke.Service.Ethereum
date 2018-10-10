using System;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;

namespace Lykke.Service.Ethereum.Core.Blockchain.Exceptions
{
    public class RpcClientTimeoutException : RpcClientException
    {
        public RpcClientTimeoutException(
            TimeSpan connectionTimeout,
            RpcRequest request,
            Exception inner) 
            
            : base($"RPC request timeout after {connectionTimeout.TotalMilliseconds} milliseconds.", request, inner)
        {
            
        }
    }
}
