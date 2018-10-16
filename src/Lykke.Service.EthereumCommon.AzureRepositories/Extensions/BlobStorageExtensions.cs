using System;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Ethereum.Core.DistributedLock;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;


namespace Lykke.Service.Ethereum.AzureRepositories.Extensions
{
    public static class BlobStorageExtensions
    {
        public static async Task<IDistributedLockToken> WaitLockAsync(
            this IBlobStorage blobStorage,
            string container,
            string key,
            TimeSpan? lockDuration)
        {
            while (true)
            {
                try
                {
                    var leaseId = await blobStorage.AcquireLeaseAsync
                    (
                        container: container,
                        key: key,
                        leaseTime: lockDuration
                    );

                    return new LockToken
                    (
                        blobStorage: blobStorage,
                        container: container,
                        key: key,
                        leaseId: leaseId,
                        lockDuration: lockDuration
                    );
                }
                catch (StorageException e) when (e.RequestInformation.HttpStatusCode == StatusCodes.Status409Conflict)
                {
                    await Task.Delay(1000);
                }
            }
        }

        private class LockToken : IDistributedLockToken
        {
            private readonly IBlobStorage _blobStorage;
            private readonly string _container;
            private readonly string _key;
            private readonly string _leaseId;
            private readonly TimeSpan? _lockDuration;

            private DateTime _renewAfter;

            
            public LockToken(
                IBlobStorage blobStorage,
                string container,
                string key,
                string leaseId,
                TimeSpan? lockDuration)
            {
                _blobStorage = blobStorage;
                _container = container;
                _key = key;
                _leaseId = leaseId;
                _lockDuration = lockDuration;
                
                UpdateRenewAfter();
            }
            
            
            public Task ReleaseAsync()
            {
                return _blobStorage.ReleaseLeaseAsync
                (
                    container: _container,
                    key: _key, 
                    leaseId: _leaseId
                );
            }

            public async Task RenewAsync()
            {
                if (DateTime.UtcNow > _renewAfter)
                {
                    await _blobStorage.RenewLeaseAsync
                    (
                        container: _container,
                        key: _key,
                        leaseId: _leaseId
                    );
                    
                    UpdateRenewAfter();
                }
            }

            private void UpdateRenewAfter()
            {
                if (_lockDuration.HasValue)
                {
                    _renewAfter = DateTime.UtcNow + new TimeSpan(_lockDuration.Value.Ticks / 2);
                }
                else
                {
                    _renewAfter = DateTime.MaxValue;
                }
            }
        }
    }
}
