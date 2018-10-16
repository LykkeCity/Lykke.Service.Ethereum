using System;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;

namespace Lykke.Service.Ethereum.Core.Blockchain.Exceptions
{
    public class RpcResponseException : Exception
    {
        public RpcResponseException(
            RpcError error)
        
            : base($"Rpc request resulted in error [{error.Code}].")
        {
            Error = error;
        }
        
        public RpcError Error { get; }
    }
}
