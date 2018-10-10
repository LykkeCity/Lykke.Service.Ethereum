using System;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;

namespace Lykke.Service.Ethereum.Core.Blockchain.Exceptions
{
    public class RpcClientException : Exception
    {
        public RpcClientException(
            string message,
            RpcRequest request,
            Exception inner)
        
            : base(message, inner)
        {
            
        }
    }
}
