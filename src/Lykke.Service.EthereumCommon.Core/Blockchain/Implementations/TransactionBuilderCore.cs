using Nethereum.Signer;


namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class TransactionBuilderCore : ITransactionBuilderCore
    {
        public byte[] BuildRawTransaction(
            byte[][] transactionElements,
            byte[] privateKey)
        {
            var signer = new RLPSigner(transactionElements);
            var key = new EthECKey(privateKey, true);
            
            signer.Sign(key);

            return signer.GetRLPEncoded();
        }
    }
}
