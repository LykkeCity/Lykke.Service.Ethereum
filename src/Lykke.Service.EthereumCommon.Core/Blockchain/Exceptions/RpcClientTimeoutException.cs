using System;
using Lykke.Service.Ethereum.Core.Blockchain.Models;

namespace Lykke.Service.Ethereum.Core.Blockchain.Exceptions
{
    public class RpcClientTimeoutException : RpcClientException
    {
        public RpcClientTimeoutException(
            TimeSpan connectionTimeout,
            RpcRequest request,
            Exception inner) 
            
            : base(request, $"Rpc request timeout after {connectionTimeout.TotalMilliseconds} milliseconds.", inner)
        {
            
        }
    }
}
