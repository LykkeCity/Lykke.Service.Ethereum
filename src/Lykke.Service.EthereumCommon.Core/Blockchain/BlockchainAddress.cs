using System;
using JetBrains.Annotations;


namespace Lykke.Service.Ethereum.Core.Blockchain
{
    [PublicAPI]
    public abstract class BlockchainAddress
    {
        protected readonly byte[] _addressBytes;

        protected BlockchainAddress(
            byte[] addressBytes)
        {
            _addressBytes = addressBytes;
        }


        public abstract string ToString(
            bool addChecksum);
        
        
        public static BlockchainAddress Parse(
            string s,
            BlockchainType blockchainType)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (blockchainType)
            {
                case BlockchainType.Ethereum:
                case BlockchainType.EthereumClassic:
                    return EthereumAddress.Parse(s);
                
                case BlockchainType.RootstockMainNet:
                    return RootstockAddress.Parse(s, true);
                
                case BlockchainType.RootstockTestNet:
                    return RootstockAddress.Parse(s, false);
                
                case BlockchainType.VeChainThor:
                    return VeChainThorAddress.Parse(s);
                
                default:
                    throw new NotSupportedException
                        ($"Parsing of addresses for specified [{blockchainType.ToString()}] blockchain is not supported.");
            }
        }
        
        public static bool TryParse(
            string s,
            BlockchainType blockchainType,
            out BlockchainAddress result)
        {
            if (Validate(s, false, blockchainType))
            {
                result = Parse(s, blockchainType);

                return true;
            }
            else
            {
                result = null;

                return false;
            }
        }

        public static bool Validate(
            string s,
            bool validateChecksum,
            BlockchainType blockchainType)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (blockchainType)
            {
                case BlockchainType.Ethereum:
                case BlockchainType.EthereumClassic:
                    return EthereumAddress.Validate(s, validateChecksum);
                
                case BlockchainType.RootstockMainNet:
                    return RootstockAddress.Validate(s, validateChecksum, true);
                
                case BlockchainType.RootstockTestNet:
                    return RootstockAddress.Validate(s, validateChecksum, false);
                
                case BlockchainType.VeChainThor:
                    return VeChainThorAddress.Validate(s, validateChecksum);
                
                default:
                    throw new NotSupportedException
                        ($"Validation of addresses for specified [{blockchainType.ToString()}] blockchain is not supported.");
            }
        }
        
        
        public sealed class EthereumAddress : BlockchainAddress
        {
            private EthereumAddress(
                byte[] addressBytes) 
                
                : base(addressBytes)
            {
                
            }
            
            
            
            internal static EthereumAddress Parse(
                string s)
            {
                throw new NotImplementedException();
            }
            
            internal static bool Validate(
                string s,
                bool validateChecksum)
            {
                throw new NotImplementedException();
            }

            public override string ToString(bool addChecksum)
            {
                throw new NotImplementedException();
            }
        }

        public sealed class RootstockAddress : BlockchainAddress
        {
            private readonly bool _isMainNet;
            
            private RootstockAddress(
                byte[] addressBytes,
                bool isMainNet) 
                
                : base(addressBytes)
            {
                _isMainNet = isMainNet;
            }
            
            internal static RootstockAddress Parse(
                string s,
                bool isMainNet)
            {
                throw new NotImplementedException();
            }
            
            internal static bool Validate(
                string s,
                bool validateChecksum,
                bool isMainNet)
            {
                throw new NotImplementedException();
            }

            public override string ToString(bool addChecksum)
            {
                throw new NotImplementedException();
            }
        }
        
        public sealed class VeChainThorAddress : BlockchainAddress
        {
            private VeChainThorAddress(
                byte[] addressBytes) 
                
                : base(addressBytes)
            {
                
            }
            
            internal static VeChainThorAddress Parse(
                string s)
            {
                throw new NotImplementedException();
            }
            
            internal static bool Validate(
                string s,
                bool validateChecksum)
            {
                throw new NotImplementedException();
            }

            public override string ToString(bool addChecksum)
            {
                throw new NotImplementedException();
            }
        }
    }
}
