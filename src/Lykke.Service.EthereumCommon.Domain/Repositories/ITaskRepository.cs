using System;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Domain.Repositories
{
    public interface ITaskRepository<T>
        where T : class, new()
    {
        Task CompleteAsync(
            string completionToken);

        Task EnqueueAsync(
            T task);

        Task<(T Task, string CompletionToken)> TryGetAsync(
            TimeSpan visibilityTimeout);
    }
}
