using JetBrains.Annotations;

namespace Lykke.Service.Ethereum.Domain.Services
{
    public interface ISignService
    {
        [Pure, NotNull]
        string SignTransaction(
            [NotNull] string transactionContext,
            [NotNull] string privateKey);
    }
}
