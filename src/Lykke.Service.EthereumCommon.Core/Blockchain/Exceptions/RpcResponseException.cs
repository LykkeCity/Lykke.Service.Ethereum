using System;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;

namespace Lykke.Service.Ethereum.Core.Blockchain.Exceptions
{
    public class RpcResponseException : Exception
    {
        public RpcResponseException(
            RpcError error)
        
            : base("RPC request resulted with error.")
        {
            ErrorCode = error.Code;
            ErrorData = error.Data.ToString();
            ErrorMessage = error.Message;
        }
        
        public int ErrorCode { get; }
        
        public string ErrorData { get; }
        
        public string ErrorMessage { get; }
    }
}
