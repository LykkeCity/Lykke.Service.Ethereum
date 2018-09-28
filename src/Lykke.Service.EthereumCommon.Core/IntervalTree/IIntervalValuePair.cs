using System;

namespace Lykke.Service.Ethereum.Core.IntervalTree
{
    public interface IIntervalValuePair<TKey, out TValue>
        where TKey : struct, IComparable<TKey>
    {
        Interval<TKey> Interval { get; }
        
        TValue Value { get; }
    }
}
