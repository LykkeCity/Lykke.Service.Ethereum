using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface ITransactionBuilderCore
    {
        Task<byte[]> BuildRawTransactionAsync(
            IEnumerable<byte[]> transactionElements,
            byte[] privateKey);
    }
}
