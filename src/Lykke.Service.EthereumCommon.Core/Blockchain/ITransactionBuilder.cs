using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Ethereum.Core.Blockchain
{
    public interface ITransactionBuilder
    {
        Task<string> BuildRawTransferTransactionAsync(
            string to,
            BigInteger amount,
            BigInteger nonce,
            BigInteger gasPrice,
            BigInteger gasLimit,
            string privateKey);
    }
}
