using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface ITransactionBuilder
    {
        string BuildRawTransferTransaction(
            string to,
            BigInteger amount,
            BigInteger nonce,
            BigInteger gasPrice,
            BigInteger gasLimit,
            string privateKey);
    }
}
