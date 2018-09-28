using Autofac;
using Common;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Service.Ethereum.AzureRepositories;
using Lykke.Service.Ethereum.Domain.Repositories;
using Lykke.Service.Ethereum.Domain.Services;
using Lykke.Service.Ethereum.QueueConsumers;
using Lykke.Service.Ethereum.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Ethereum.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(
            IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        
        
        private WorkerSettings ServiceSettings
            => _appSettings.CurrentValue.WorkerService;
        

        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .RegisterChaosKitty(ServiceSettings.Chaos);
            
            LoadQueueConsumers(builder);
            
            LoadRepositories(builder);
            
            LoadServices(builder);
        }

        private void LoadQueueConsumers(
            ContainerBuilder builder)
        {
            // BalanceObservationQueueConsumer

            builder
                .RegisterType<BalanceObservationQueueConsumerBase>()
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();
            
            builder
                .RegisterInstance(new BalanceObservationQueueConsumerBase.Settings
                {
                    MaxDegreeOfParallelism = ServiceSettings.BalanceObservationMaxDegreeOfParallelism
                })
                .AsSelf();
            
            // BlockchainIndexationQueueConsumer

            builder
                .RegisterType<BlockchainIndexationQueueConsumerBase>()
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();
            
            builder
                .RegisterInstance(new BlockchainIndexationQueueConsumerBase.Settings
                {
                    MaxDegreeOfParallelism = ServiceSettings.BlockchainIndexingMaxDegreeOfParallelism
                })
                .AsSelf();
            
            // TransactionMonitoringQueueConsumer

            builder
                .RegisterType<TransactionMonitoringQueueConsumerBase>()
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();
            
            builder
                .RegisterInstance(new TransactionMonitoringQueueConsumerBase.Settings
                {
                    MaxDegreeOfParallelism = ServiceSettings.TransactionMonitoringMaxDegreeOfParallelism
                })
                .AsSelf();
        }
        
        private void LoadRepositories(
            ContainerBuilder builder)
        {
            var connectionString = _appSettings.ConnectionString(x => x.WorkerService.Db.DataConnString);
            
            // BalanceObservationTaskRepository
            
            builder
                .Register(x => BalanceObservationTaskRepository.Create
                (
                    connectionString: connectionString
                ))
                .As<IBalanceObservationTaskRepository>()
                .SingleInstance();
            
            // BlockchainIndexationStateRepository
            
            builder
                .Register(x => BlockchainIndexationStateRepository.Create
                (
                    connectionString: connectionString
                ))
                .As<IBlockchainIndexationStateRepository>()
                .SingleInstance();
            
            // BlockIndexationLockRepository
            
            builder
                .Register(x => BlockIndexationLockRepository.Create
                (
                    connectionString: connectionString,
                    logFactory: x.Resolve<ILogFactory>()
                ))
                .As<IBlockIndexationLockRepository>()
                .SingleInstance();
            
            // ObservableBalanceRepository
            
            builder
                .Register(x => ObservableBalanceRepository.Create
                (
                    connectionString: connectionString,
                    logFactory: x.Resolve<ILogFactory>()
                ))
                .As<IObservableBalanceRepository>()
                .SingleInstance();
            
            // TransactionMonitoringTaskRepository
            
            builder
                .Register(x => TransactionMonitoringTaskRepository.Create
                (
                    connectionString: connectionString
                ))
                .As<ITransactionMonitoringTaskRepository>()
                .SingleInstance();
            
            // TransactionReceiptRepository
            
            builder
                .Register(x => TransactionReceiptRepository.Create
                (
                    connectionString: connectionString,
                    logFactory: x.Resolve<ILogFactory>()
                ))
                .As<ITransactionReceiptRepository>()
                .SingleInstance();
            
            // TransactionRepository
            
            builder
                .Register(x => TransactionRepository.Create
                (
                    connectionString: connectionString,
                    logFactory: x.Resolve<ILogFactory>()
                ))
                .As<ITransactionRepository>()
                .SingleInstance();
            
        }
        
        private void LoadServices(
            ContainerBuilder builder)
        {
            // BalanceObservationService
            
            builder
                .RegisterType<BalanceObservationService>()
                .As<IBalanceObservationService>()
                .SingleInstance();
            
            // BlockchainIndexingService
            
            builder
                .RegisterType<BlockchainIndexingService>()
                .As<IBlockchainIndexingService>()
                .SingleInstance();
            
            builder
                .RegisterInstance(new BlockchainIndexingService.Settings
                {
                    BlockLockDuration = ServiceSettings.BlockLockDuration
                })
                .AsSelf();
            
            // BlockchainService
            
            builder
                .RegisterType<BlockchainService>()
                .As<IBlockchainService>()
                .SingleInstance();
            
            builder
                .RegisterInstance(new BlockchainService.Settings
                {
                    ConfirmationLevel = ServiceSettings.ConfirmationLevel,
                    ParityNodeUrl = ServiceSettings.ParityNodeUrl
                })
                .AsSelf();
            
            // TransactionMonitoringService
            
            builder
                .RegisterType<TransactionMonitoringService>()
                .As<ITransactionMonitoringService>()
                .SingleInstance();
        }
    }
}
