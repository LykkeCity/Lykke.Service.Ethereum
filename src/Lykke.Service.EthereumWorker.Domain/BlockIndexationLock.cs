using System;
using System.Numerics;

namespace Lykke.Service.Ethereum.Domain
{
    public class BlockIndexationLock
    {
        public BigInteger BlockNumber { get; set; }
        
        public DateTime LockedOn { get; set; }
    }
}
