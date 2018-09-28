using System;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Domain.Services
{
    public interface ITransactionService
    {
        [ItemNotNull]
        Task<BuildTransactionResult> BuildTransactionAsync(
            Guid transactionId,
            [NotNull] string from,
            [NotNull] string to,
            BigInteger amount,
            bool includeFee);

        [ItemNotNull]
        Task<BroadcastTransactionResult> BroadcastTransactionAsync(
            Guid transactionId,
            [NotNull] string signedTxData);

        Task<bool> DeleteTransactionIfExistsAsync(
            Guid transactionId);
        
        Task<Transaction> TryGetTransactionAsync(
            Guid transactionId);
    }
}
