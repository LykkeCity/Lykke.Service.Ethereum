using System;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Lykke.Service.Ethereum.Core.Blockchain.Exceptions;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    public static class RpcResponseExtensions
    {
        public static void EnsureSuccessResult(
            this RpcResponse rpcResponse)
        {
            if (rpcResponse.Error != null)
            {
                throw new RpcResponseException(rpcResponse.Error);
            }
        }

        public static T ResultValue<T>(
            this RpcResponse rpcResponse)
        {
            var result = rpcResponse.Result;

            if (typeof(T) == typeof(BigInteger))
            {
                var resultStr = result.Value<string>();

                return (T) (object) resultStr.HexToBigInteger();
            }
            
            if (typeof(T) == typeof(BigInteger?))
            {
                var resultStr = result.Value<string>();

                if (!string.IsNullOrEmpty(resultStr))
                {
                    return (T) (object) resultStr.HexToBigInteger();
                }
                else
                {
                    return (T) (object) default(BigInteger?);
                }
            }
            
            return result.Value<T>();
        }
        
        public static T ResultValue<T>(
            this RpcResponse rpcResponse,
            string key)
        {
            var result = rpcResponse.Result;

            if (typeof(T) == typeof(BigInteger))
            {
                var resultStr = result.Value<string>(key);

                return (T) (object) resultStr.HexToBigInteger();
            }
            
            if (typeof(T) == typeof(BigInteger?))
            {
                var resultStr = result.Value<string>(key);

                if (!string.IsNullOrEmpty(resultStr))
                {
                    return (T) (object) resultStr.HexToBigInteger();
                }
                else
                {
                    return (T) (object) default(BigInteger?);
                }
            }
            
            return result.Value<T>(key);
        }
    }
}
