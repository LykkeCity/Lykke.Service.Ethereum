using System;
using AzureStorage.Queue;
using JetBrains.Annotations;
using Lykke.Service.Ethereum.Domain;
using Lykke.Service.Ethereum.Domain.Repositories;
using Lykke.SettingsReader;

namespace Lykke.Service.Ethereum.AzureRepositories
{
    [UsedImplicitly]
    public class TransactionMonitoringTaskRepository : TaskRepositoryBase<TransactionMonitoringTask>, ITransactionMonitoringTaskRepository
    {
        private TransactionMonitoringTaskRepository(
            IQueueExt queue)
            : base(queue)
        {
            
        }

        public static ITransactionMonitoringTaskRepository Create(
            IReloadingManager<string> connectionString)
        {
            var queue = AzureQueueExt.Create
            (
                connectionStringManager: connectionString,
                queueName: "transaction-monitoring-tasks",
                maxExecutionTimeout: TimeSpan.FromDays(7)
            );
            
            return new TransactionMonitoringTaskRepository(queue);
        }
    }
}
