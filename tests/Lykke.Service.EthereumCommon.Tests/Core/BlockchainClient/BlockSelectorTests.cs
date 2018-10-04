using System;
using System.Collections.Generic;
using FluentAssertions;
using Lykke.Service.Ethereum.Core;
using Lykke.Service.Ethereum.Core.Blockchain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.Ethereum.Tests.Core.Web3Client
{
    [TestClass]
    public class BlockSelectorTests
    {
        [TestMethod]
        public void GenesisBlockSelector_ToString__Blockchain_Passed__Valid_String_Representation_Returned()
        {
            var genesisBlockSelector = BlockSelector.GetGenesisBlockSelector();
            
            foreach (var (blockchain, blockSelectorString) in GetBlockSelectorStrings(genesisBlockSelector))
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (blockchain)
                {
                    case BlockchainType.VeChainThor:
                        blockSelectorString.Should().Be("0");
                        break;
                    default:
                        blockSelectorString.Should().Be("genesis");
                        break;
                }
            }
        }
        
        [TestMethod]
        public void LatestBlockSelector_ToString__Blockchain_Passed__Valid_String_Representation_Returned()
        {
            var latestBlockSelector = BlockSelector.GetLatestBlockSelector();

            foreach (var (blockchain, blockSelectorString) in GetBlockSelectorStrings(latestBlockSelector))
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (blockchain)
                {
                    case BlockchainType.VeChainThor:
                        blockSelectorString.Should().Be("best");
                        break;
                    default:
                        blockSelectorString.Should().Be("latest");
                        break;
                }
            }
        }
        
        [TestMethod]
        public void NumberedBlockSelector_ToString__Blockchain_Passed__Valid_String_Representation_Returned()
        {
            var numberedBlockSelector = BlockSelector.GetNumberedBlockSelector(42);

            foreach (var (blockchain, blockSelectorString) in GetBlockSelectorStrings(numberedBlockSelector))
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (blockchain)
                {
                    default:
                        blockSelectorString.Should().Be("42");
                        break;
                }
            }
        }
        
        [TestMethod]
        public void PendingBlockSelector_ToString__Blockchain_Passed__Valid_String_Representation_Returned()
        {
            var pendingBlockSelector = BlockSelector.GetPendingBlockSelector();

            foreach (var (blockchain, blockSelectorString) in GetBlockSelectorStrings(pendingBlockSelector))
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (blockchain)
                {
                    default:
                        blockSelectorString.Should().Be("pending");
                        break;
                }
            }
        }

        private static IEnumerable<(BlockchainType Blockchain, string BlockSelectorString)> GetBlockSelectorStrings(
            BlockSelector blockSelector)
        {
            foreach (BlockchainType blockchain in Enum.GetValues(typeof(BlockchainType)))
            {
                yield return (blockchain, blockSelector.ToString(blockchain));
            }
        }
    }
}
