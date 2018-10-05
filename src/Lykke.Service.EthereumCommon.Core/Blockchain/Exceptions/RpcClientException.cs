using System;
using Lykke.Service.Ethereum.Core.Blockchain.Models;

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
