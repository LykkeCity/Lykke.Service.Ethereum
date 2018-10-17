using System.IO;
using System.Numerics;
using Lykke.Service.Ethereum.Core.Blockchain.Extensions;
using Nethereum.RLP;
using Newtonsoft.Json;

namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    [JsonObject]
    public sealed class TransactionBuilderTestData
    {
        [JsonConstructor]
        public TransactionBuilderTestData(
            string amount,
            string data,
            string expectedRawTransaction,
            string gasPrice,
            string gasLimit,
            string nonce,
            string privateKey,
            string to)
        {
            Amount = amount;
            Data = data;
            ExpectedRawTransaction = expectedRawTransaction;
            GasPrice = gasPrice;
            GasLimit = gasLimit;
            Nonce = nonce;
            PrivateKey = privateKey;
            To = to;
        }

        public void Deconstruct(
            out byte[][] transactionElements,
            out byte[] privateKey,
            out byte[] expectedRawTransaction)
        {
            transactionElements = GetOrderedTransactionElements();
            privateKey = PrivateKey.HexToByteArray();
            expectedRawTransaction = ExpectedRawTransaction.HexToByteArray();
        }

        public static TransactionBuilderTestData Restore(
            string fileName)
        {
            var dataJson = File.ReadAllText($"Core/Blockchain/Implementations/RawTransactions/{fileName}");

            return JsonConvert.DeserializeObject<TransactionBuilderTestData>(dataJson);
        }
        
        [JsonProperty("amount")]
        public string Amount { get; }
        
        [JsonProperty("data")]
        public string Data { get; }
        
        [JsonProperty("expectedRawTransaction")]
        public string ExpectedRawTransaction { get; }
        
        [JsonProperty("gasPrice")]
        public string GasPrice { get; }
        
        [JsonProperty("gasLimit")]
        public string GasLimit { get; }
        
        [JsonProperty("nonce")]
        public string Nonce { get; }
        
        [JsonProperty("privateKey")]
        public string PrivateKey { get; }
        
        [JsonProperty("to")]
        public string To { get; }

        
        private byte[][] GetOrderedTransactionElements()
        {
            return new[]
            {
                BigInteger.Parse(Nonce).ToBytesForRLPEncoding(),
                BigInteger.Parse(GasPrice).ToBytesForRLPEncoding(),
                BigInteger.Parse(GasLimit).ToBytesForRLPEncoding(),
                To.HexToByteArray(),
                BigInteger.Parse(Amount).ToBytesForRLPEncoding(),
                Data.HexToByteArray()
            };
        }
    }
}
