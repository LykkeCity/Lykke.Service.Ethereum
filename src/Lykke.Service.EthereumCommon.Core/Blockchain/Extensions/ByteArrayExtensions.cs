using System;
using System.Collections.Generic;
using System.Linq;
using Nethereum.Hex.HexConvertors.Extensions;


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

        public static byte[] Slice(
            this byte[] data,
            int start,
            int end = int.MaxValue)
        {
            if (end < 0)
            {
                end = data.Length + end;
            }
            
            start = Math.Max(0, start);
            end = Math.Max(start, end);
                
            return data
                .Skip(start)
                .Take(end - start)
                .ToArray();
        }
        
        public static string ToHexString(
            this byte[] data)
        {
            return $"0x{string.Concat(data.Select(b => b.ToString("x2")))}";
        }
    }
}
