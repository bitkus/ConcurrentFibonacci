using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Fibonacci
{
    public class FibonacciSequenceCalculator : IFibonacciSequenceCalculator
    {
        private FibonacciState _stateCache;

        public KeyValuePair<int, long> GetNext(FibonacciState state)
        {
            UpdateCache(state);
            var index = state.GetLastIndex() + 1;
            var fibonacci = Fibonacci(index);
            return new KeyValuePair<int, long>(index, fibonacci);
        }

        private void UpdateCache(FibonacciState state)
        {
            _stateCache = state;
        }

        private long Fibonacci(int index)
        {
            return index switch
            {
                0 => 1,
                1 => 2,
                _ => GetPenultimateFib(index) + GetUltimateFib(index)
            };
        }

        private long GetPenultimateFib(int index)
        {
            return GetFibFromCacheOrCalculate(index - 2);
        }

        private long GetUltimateFib(int index)
        {
            return GetFibFromCacheOrCalculate(index - 1);
        }

        private long GetFibFromCacheOrCalculate(int index)
        {
            if (_stateCache.TryGet(index, out var ultimateFib))
            {
                return ultimateFib;
            }

            return Fibonacci(index);
        }
    }

    public interface IFibonacciSequenceCalculator
    {
        KeyValuePair<int, long> GetNext(FibonacciState currentState);
    }

    public class FibonacciState
    {
        private readonly ConcurrentDictionary<int, long> _orderedFibonacciSequence = new ConcurrentDictionary<int, long>();
        private static readonly object Lock = new object();

        public int GetLastIndex()
        {
            lock (Lock)
            {
                return _orderedFibonacciSequence.Keys.Any() 
                    ? _orderedFibonacciSequence.Keys.Last() 
                    : 0;
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
                return _orderedFibonacciSequence.TryGetValue(index, out value);
            }
        }

        public void Add(int index, long fib)
        {
            lock (Lock)
            {
                _orderedFibonacciSequence.TryAdd(index, fib);
            }
        }
    }

    //public class FibonacciCalculationState
    //{
    //    public int UltimateIndex;

    //    public long UltimateFib;

    //    public long PenultimateFib;
    //}
}