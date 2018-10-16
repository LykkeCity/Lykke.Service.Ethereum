using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.Crypto;

using static Nethereum.RLP.RLP;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class TransactionBuilderCore : ITransactionBuilderCore
    {
        public virtual async Task<byte[]> BuildRawTransactionAsync(
            byte[][] transactionElements,
            byte[] privateKey)
        {
            var signature = await GetSignatureAsync(transactionElements, privateKey);

            return Encode(transactionElements, signature);
        }

        protected virtual async Task<byte[]> GetSignatureAsync(
            IEnumerable<byte[]> transactionElements,
            byte[] privateKey)
        {
            return Secp256K1.Sign
            (
                messageHash: await GetSigningHashAsync(transactionElements),
                privateKey: privateKey
            );
        }

        protected virtual Task<byte[]> GetSigningHashAsync(
            IEnumerable<byte[]> transactionElements)
        {
            return Keccak256.SumAsync
            (
                Encode(transactionElements, null)
            );
        }

        protected virtual byte[] Encode(
            IEnumerable<byte[]> transactionElements,
            byte[] signature)
        {
            var encodedData = transactionElements
                .Select(EncodeElement)
                .ToList();

            if (signature != null)
            {
                encodedData.Add
                (
                    EncodeElement(signature)
                );
            }

            return EncodeList
            (
                encodedData.ToArray()
            );
        }
    }
}
