using System.Numerics;
using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    [PublicAPI]
    public abstract class BlockSelector
    {
        public static BlockSelector GetGenesisBlockSelector()
            => new GenesisBlockSelector();
        
        public static BlockSelector GetLatestBlockSelector()
            => new LatestBlockSelector();
        
        public static BlockSelector GetNumberedBlockSelector(BigInteger blockNumber)
            => new NumberedBlockSelector(blockNumber.ToString());
        
        public static BlockSelector GetPendingBlockSelector()
            => new PendingBlockSelector();


        private readonly string _blockSelector;

        protected BlockSelector(
            string blockSelector)
        {
            _blockSelector = blockSelector;
        }
        
        
        /// <inheritdoc />
        public override string ToString()
        {
            return _blockSelector;
        }

        /// <summary>
        ///    Returns the string that represents the current block selector with respect to blockchain specifics.
        /// </summary>
        /// <returns>
        ///    A string that represents the current block selector.
        /// </returns>
        public virtual string ToString(
            BlockchainType blockchainType)
        {
            return _blockSelector;
        }


        public sealed class GenesisBlockSelector : BlockSelector
        {
            internal GenesisBlockSelector() 
                : base("genesis")
            {
                
            }
            
            public override string ToString(BlockchainType blockchainType)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (blockchainType)
                {
                    case BlockchainType.VeChainThor:
                        return "0";
                    default:
                        return base.ToString(blockchainType);
                }
            }
        }
        
        public sealed class LatestBlockSelector : BlockSelector
        {
            internal LatestBlockSelector() 
                : base("latest")
            {
                
            }

            public override string ToString(
                BlockchainType blockchainType)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (blockchainType)
                {
                    case BlockchainType.VeChainThor:
                        return "best";
                    default:
                        return base.ToString(blockchainType);
                }
            }
        }
        
        public sealed class NumberedBlockSelector : BlockSelector
        {
            internal NumberedBlockSelector(string blockSelector) 
                : base(blockSelector)
            {
                
            }
        }
        
        public class PendingBlockSelector : BlockSelector
        {
            internal PendingBlockSelector() 
                : base("pending")
            {
                
            }
        }
    }
}
