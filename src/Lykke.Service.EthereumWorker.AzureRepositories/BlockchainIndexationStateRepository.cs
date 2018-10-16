using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Blob;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Ethereum.AzureRepositories.Extensions;
using Lykke.Service.Ethereum.Core.DistributedLock;
using Lykke.Service.Ethereum.Domain;
using Lykke.Service.Ethereum.Domain.Repositories;
using Lykke.SettingsReader;
using MessagePack;


namespace Lykke.Service.Ethereum.AzureRepositories
{
    public class BlockchainIndexationStateRepository : RepositoryBase, IBlockchainIndexationStateRepository
    {
        private const string Container = "blockchain-indexation-state";
        private const string DataKey = ".bin";
        private const string LockKey = ".lock";


        private readonly IBlobStorage _blobStorage;
        private readonly ILog _log;


        private BlockchainIndexationStateRepository(
            IBlobStorage blobStorage,
            ILogFactory logFactory)
        {
            _blobStorage = blobStorage;
            _log = logFactory.CreateLog(this);
        }

        public static BlockchainIndexationStateRepository Create(
            IReloadingManager<string> connectionString,
            ILogFactory logFactory)
        {
            return new BlockchainIndexationStateRepository
            (
                AzureBlobStorage.Create(connectionString),
                logFactory
            );
        }
        
        public async Task<BlockchainIndexationState> GetOrCreateAsync()
        {
            if (await _blobStorage.HasBlobAsync(Container, DataKey))
            {
                using (var stream = await _blobStorage.GetAsync(Container, DataKey))
                {
                    return BlockchainIndexationState.Restore
                    (
                        await MessagePackSerializer.DeserializeAsync<IEnumerable<BlocksIntervalIndexationState>>(stream)
                    );
                }
            }
            else
            {
                return BlockchainIndexationState.Create();
            }
        }

        public async Task UpdateAsync(
            BlockchainIndexationState state)
        {
            using (var stream = new MemoryStream())
            {
                await MessagePackSerializer.SerializeAsync(stream, (IEnumerable<BlocksIntervalIndexationState>) state);
                
                await _blobStorage.SaveBlobAsync(Container, DataKey, stream);
            }
        }

        public Task<IDistributedLockToken> WaitLockAsync()
        {
            try
            {
                return _blobStorage.WaitLockAsync
                (
                    container: Container,
                    key: LockKey,
                    lockDuration: TimeSpan.FromSeconds(60)
                );
            }
            catch (Exception e)
            {
                _log.Error("Failed to obtain distributed lock.", e);

                throw;
            }
        }
    }
}
