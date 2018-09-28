using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Ethereum.Domain;
using Lykke.Service.Ethereum.Domain.Services;

namespace Lykke.Service.Ethereum.QueueConsumers
{
    [UsedImplicitly]
    public class TransactionMonitoringQueueConsumerBase : QueueConsumerBase<(TransactionMonitoringTask Task, string CompletionToken)>
    {
        private readonly ILog _log;
        private readonly ITransactionMonitoringService _transactionMonitoringService;
        
        
        public TransactionMonitoringQueueConsumerBase(
            ILogFactory logFactory,
            Settings settings,
            ITransactionMonitoringService transactionMonitoringService)
        
            : base(maxDegreeOfParallelism: settings.MaxDegreeOfParallelism)
        {
            _log = logFactory.CreateLog(this);
            _transactionMonitoringService = transactionMonitoringService;
        }
        
        
        protected override async Task<(bool, (TransactionMonitoringTask, string))> TryGetNextTaskAsync()
        {
            var taskAndCompletionToken = await _transactionMonitoringService.TryGetNextMonitoringTaskAsync();

            return (taskAndCompletionToken.Task != null, taskAndCompletionToken);
        }

        protected override async Task ProcessTaskAsync(
            (TransactionMonitoringTask Task, string CompletionToken) taskAndCompletionToken)
        {
            var transactionChecked = await _transactionMonitoringService
                .CheckAndUpdateStateAsync(taskAndCompletionToken.Task.TransactionId);
            
            if (transactionChecked)
            {
                await _transactionMonitoringService.CompleteMonitoringTaskAsync(taskAndCompletionToken.CompletionToken);
            }
        }
        
        public override void Start()
        {
            _log.Info("Starting transaction monitoring...");
            
            base.Start();
            
            _log.Info("Transaction monitoring started.");
        }

        public override void Stop()
        {
            _log.Info("Stopping transaction monitoring...");
            
            base.Stop();
            
            _log.Info("Transaction monitoring stopped.");
        }
        
        
        public class Settings
        {
            public int MaxDegreeOfParallelism { get; set; }
        }
    }
}
