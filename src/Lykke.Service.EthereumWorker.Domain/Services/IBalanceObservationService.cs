using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Domain.Services
{
    public interface IBalanceObservationService
    {
        Task<(BalanceObservationTask Task, string CompletionToken)> TryGetNextObservationTaskAsync();

        Task<bool> CheckAndUpdateBalanceAsync(
            string address,
            BigInteger blockNumber);

        Task CompleteObservationTaskAsync(
            string completionToken);
    }
}
