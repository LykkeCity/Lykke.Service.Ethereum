using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface ITransactionSigner
    {
        Task<object> SignTransactionAsync(
            object unsignedTransaction,
            string privateKey);
    }
}
