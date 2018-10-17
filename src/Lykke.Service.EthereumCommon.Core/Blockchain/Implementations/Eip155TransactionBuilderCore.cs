using System;
using System.Numerics;
using Nethereum.Signer;

namespace Lykke.Service.Ethereum.Core.Blockchain.Implementations
{
    public class Eip155TransactionBuilderCore : ITransactionBuilderCore
    {
        private readonly BigInteger _chainId;
        
        
        public Eip155TransactionBuilderCore(
            BlockchainType blockchainType,
            bool isMainNet)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (blockchainType)
            {
                case BlockchainType.Ethereum:
                    _chainId = isMainNet ? new BigInteger(1) : new BigInteger(3);
                    break;
                case BlockchainType.EthereumClassic:
                    _chainId = isMainNet ? new BigInteger(61) : new BigInteger(62);
                    break;
                case BlockchainType.Rootstock:
                    _chainId = isMainNet ? new BigInteger(30) : new BigInteger(31);
                    break;
                default:
                    throw new ArgumentOutOfRangeException
                    (
                        nameof(blockchainType),
                        blockchainType,
                        $"Specified blockchain [{blockchainType.ToString()}] is not supported by {nameof(Eip155TransactionBuilderCore)}."
                    );
            }
            
        }
        
        public byte[] BuildRawTransaction(
            byte[][] transactionElements,
            byte[] privateKey)
        {
            var signer = new RLPSigner(transactionElements);
            var key = new EthECKey(privateKey, true);
            
            signer.Sign(key, _chainId);

            return signer.GetRLPEncoded();
        }
    }
}
