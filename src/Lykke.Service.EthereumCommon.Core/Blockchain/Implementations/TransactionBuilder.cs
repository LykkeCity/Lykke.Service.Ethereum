using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Service.Ethereum.Core.Blockchain.Extensions;
using Nethereum.RLP;

namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class TransactionBuilder : ITransactionBuilder
    {
        private readonly ITransactionBuilderCore _core;


        public TransactionBuilder(
            ITransactionBuilderCore core)
        {
            _core = core;
        }


        public async Task<string> BuildRawTransferTransactionAsync(
            string to,
            BigInteger amount,
            BigInteger nonce,
            BigInteger gasPrice,
            BigInteger gasLimit,
            string privateKey)
        {
            var transactionElements = GetOrderedTransactionElements
            (
                to: to,
                amount: amount,
                nonce: nonce,
                gasPrice: gasPrice,
                gasLimit: gasLimit,
                data: null
            );

            var rawTransaction = await _core.BuildRawTransactionAsync
            (
                transactionElements,
                privateKey.HexToByteArray()
            );

            return rawTransaction.ToHexString();
        }

        private static IEnumerable<byte[]> GetOrderedTransactionElements(
            string to,
            BigInteger amount,
            BigInteger nonce,
            BigInteger gasPrice,
            BigInteger gasLimit,
            string data)
        {
            return new[]
            {
                nonce.ToBytesForRLPEncoding(),
                gasPrice.ToBytesForRLPEncoding(),
                gasLimit.ToBytesForRLPEncoding(),
                to.HexToByteArray(),
                amount.ToByteArray(),
                data.HexToByteArray()
            };
        }
    }
}
