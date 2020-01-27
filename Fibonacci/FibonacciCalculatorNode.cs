using System;

namespace Fibonacci
{
    public class FibonacciCalculatorNode : IFibonacciCalculatorNode
    {
        private readonly FibonacciState _state;
        private readonly string _nodeId;
        private readonly INearestFibonacciCalculator _nearestFibonacciCalculator;

        public FibonacciCalculatorNode(FibonacciState state, string nodeId, INearestFibonacciCalculator nearestFibonacciCalculator)
        {
            _state = state;
            _nodeId = nodeId;
            _nearestFibonacciCalculator = nearestFibonacciCalculator;
            GetNearestFibonacci();
        }

        public void Run()
        {
            AttemptCalculation();
        }

        private void AttemptCalculation()
        {
            var success = _state.TryGetAndGrabMissingIndex(_nodeId, out var stateItem);
            if (success)
            {
                UpdateState(stateItem);
                AttemptCalculation();
            }
            else
            {
                Console.WriteLine($"{_nodeId} has finished calucating.");
            }
        }

        private void UpdateState(FibonacciStateItem stateItem)
        {
            stateItem.Fibonacci = Fibonacci(stateItem.Index);
            _state.Update(stateItem);
            Console.WriteLine($"{stateItem.NodeId},{stateItem.Fibonacci}");
        }

        private void GetNearestFibonacci()
        {
            //var seed = new Random().Next();
            //var seed = new Random().Next(100000);
            int seed = 0;
            if (_nodeId == "node-1")
            {
                seed = 0;
            }
            else
            {
                seed = 100000;
            }
            
            Console.WriteLine($"{_nodeId} started with seed {seed}");
            var nearestFib = _nearestFibonacciCalculator.GetNearestFib(seed);
            _state.Add(new FibonacciStateItem
            {
                Fibonacci = nearestFib,
                Index = Helpers.GetFibonacciIndex(nearestFib),
                NodeId = _nodeId
            });
        }

        //public FibonacciStateItem GetNext(FibonacciState state)
        //{
        //    var index = state.GetLatestIndex() + 1;
        //    var fibonacci = Fibonacci(index);
        //    return new FibonacciStateItem
        //    {
        //        Fibonacci = fibonacci,
        //        Index = index,
        //        NodeId = _nodeId
        //    };
        //}

        //private void UpdateCache(FibonacciState state)
        //{
        //    _state = state;
        //}

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
            return GetOrCalculateFibonacci(index - 2);
        }

        private long GetUltimateFib(int index)
        {
            return GetOrCalculateFibonacci(index - 1);
        }

        private long GetOrCalculateFibonacci(int index)
        {
            if (_state.TryGet(index, out var ultimateFib))
            {
                return ultimateFib;
            }

            return Fibonacci(index);
        }
    }

    public interface IFibonacciCalculatorNode
    {
        void Run();

        //FibonacciStateItem GetNext(FibonacciState currentState);
    }

    //public class FibonacciCalculationState
    //{
    //    public int UltimateIndex;

    //    public long UltimateFib;

    //    public long PenultimateFib;
    //}
}