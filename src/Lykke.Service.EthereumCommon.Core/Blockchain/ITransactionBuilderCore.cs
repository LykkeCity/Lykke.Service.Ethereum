using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface ITransactionBuilderCore
    {
        byte[] BuildRawTransaction(
            byte[][] transactionElements,
            byte[] privateKey);
    }
}
