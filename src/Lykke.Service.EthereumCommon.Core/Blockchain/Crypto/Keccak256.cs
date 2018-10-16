using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.Extensions;
using Multiformats.Hash;
using Multiformats.Hash.Algorithms;

namespace Lykke.Service.Ethereum.Core.Blockchain.Crypto
{
    public static class Keccak256
    {
        public static byte[] Sum(params byte[][] data)
        {
            var multihash = Multihash.Sum<KECCAK_256>
            (
                data: data.Concat()
            );

            return multihash.Digest;
        }
        
        public static async Task<byte[]> SumAsync(params byte[][] data)
        {
            var multihash = await Multihash.SumAsync<KECCAK_256>
            (
                data: data.Concat()
            );

            return multihash.Digest;
        }
    }
}
