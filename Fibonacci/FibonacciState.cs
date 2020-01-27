using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Fibonacci
{
    public class FibonacciState
    {
        private readonly ConcurrentBag<FibonacciStateItem> _fibonacciBag;
        private static readonly object Lock = new object();

        public FibonacciState()
        {
            _fibonacciBag = new ConcurrentBag<FibonacciStateItem>();
        }

        //public int GetLatestIndex()
        //{
        //    lock (Lock)
        //    {
        //        return _fibonacciBag.IsEmpty
        //            ? 0 
        //            : _fibonacciBag.OrderBy(f => f.Index).Last().Index;
        //    }
        //}

        public bool TryGetAndGrabMissingIndex(string nodeId, out FibonacciStateItem stateItem)
        {
            lock(Lock) 
            {
                var max = _fibonacciBag.OrderBy(f => f.Index).Last().Index;
                var min = _fibonacciBag.OrderBy(f => f.Index).First().Index;

                for (var i = min; i <= max; i++)
                {
                    if (!_fibonacciBag.Any(f => f.Index == i))
                    {
                        stateItem = new FibonacciStateItem
                        {
                            Fibonacci = 0,
                            Index = i,
                            NodeId = nodeId
                        };
                        _fibonacciBag.Add(stateItem);

                        return true;
                    }
                }

                stateItem = default;
                return false;
            }
        }

        public void Update(FibonacciStateItem stateItem)
        {
            lock (Lock)
            {
                var item = _fibonacciBag.First(f => f.Index == stateItem.Index);
                item = stateItem;
            }
        }

        //public FibonacciCalculationState GetLatestCalculationState()
        //{
        //    lock (Lock)
        //    {
        //        var values = _orderedSequence.Values.ToArray();
        //        return new FibonacciCalculationState
        //        {
        //            UltimateFib = values[^1],
        //            PenultimateFib = values[^2],
        //            UltimateIndex = _orderedSequence.Keys.Last()
        //        };
        //    }
        //}

        public bool TryGet(int index, out long value)
        {
            lock (Lock)
            {
                try
                {
                    var fib = _fibonacciBag.First(f => f.Index == index);
                    value = fib.Fibonacci;
                    return true;
                }
                catch (Exception)
                {
                    value = 0;
                    return false;
                }
            }
        }

        public void Add(FibonacciStateItem stateItem)
        {
            lock (Lock)
            {
                _fibonacciBag.Add(stateItem);
            }
        }

        public bool IsNodeCalculating(int index, string nodeId)
        {
            return _fibonacciBag.Any(f => f.Index == index && f.NodeId == nodeId);
        }
    }

    //public class FibonacciCalculationState
    //{
    //    public int UltimateIndex;

    //    public long UltimateFib;

    //    public long PenultimateFib;
    //}
}