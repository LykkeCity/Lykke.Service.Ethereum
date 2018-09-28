using System;
using MessagePack;

namespace Lykke.Service.Ethereum.Domain
{
    [MessagePackObject]
    public class TransactionMonitoringTask
    {
        [Key(0)]
        public Guid TransactionId { get; set; }
    }
}
