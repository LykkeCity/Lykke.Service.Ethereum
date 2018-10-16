using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Ethereum.Core.Blockchain.Extensions
{
    public static class ByteArrayExtensions
    {
        public static byte[] Concat(
            this IEnumerable<byte[]> data)
        {
            return data
                .SelectMany(x => x)
                .ToArray();
        }

        public static string ToHexString(
            this byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
