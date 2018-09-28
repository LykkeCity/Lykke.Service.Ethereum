using System.Numerics;

namespace Lykke.Service.Ethereum.Domain
{
    public class Balance
    {
        public string Address { get; set; }
        
        public BigInteger Amount { get; set; }
        
        public BigInteger BlockNumber { get; set; }
    }
}
