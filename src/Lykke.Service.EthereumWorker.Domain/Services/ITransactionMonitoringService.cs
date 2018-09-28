using System;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Domain.Services
{
    public interface ITransactionMonitoringService
    {
        Task<bool> CheckAndUpdateStateAsync(
            Guid transactionId);
        
        Task CompleteMonitoringTaskAsync(
            string completionToken);
        
        Task<(TransactionMonitoringTask Task, string CompletionToken)> TryGetNextMonitoringTaskAsync();
    }
}
