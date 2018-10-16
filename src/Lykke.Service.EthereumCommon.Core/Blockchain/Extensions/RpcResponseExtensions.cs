using System.Numerics;
using Lykke.Service.Ethereum.Core.Blockchain.DTOs;
using Lykke.Service.Ethereum.Core.Blockchain.Exceptions;
using Newtonsoft.Json.Linq;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    public static class RpcResponseExtensions
    {
        public static void EnsureSuccessfulResult(
            this RpcResponse rpcResponse)
        {
            if (rpcResponse.Error != null)
            {
                throw new RpcResponseException(rpcResponse.Error);
            }
        }

        public static T ResultValue<T>(
            this RpcResponse rpcResponse)
            => ResultValue<T>(rpcResponse.Result);

        public static T ResultValue<T>(
            this RpcResponse rpcResponse,
            string key)
            => ResultValue<T>(rpcResponse.Result[key]);

        private static T ResultValue<T>(
            JToken result)
        {
            if (typeof(T) == typeof(BigInteger))
            {
                return (T) (object) result.Value<string>().HexToBigInteger();
            }
            else if (typeof(T) == typeof(BigInteger?))
            {
                return (T) (object) result.Value<string>().HexToNullableBigInteger();
            }
            else
            {
                return result.Value<T>();
            }
        }
    }
}
