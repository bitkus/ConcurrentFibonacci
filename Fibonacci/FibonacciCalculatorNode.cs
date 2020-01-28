using System;
using System.Threading.Tasks;

namespace Fibonacci
{
    public class FibonacciCalculatorNode : IFibonacciCalculatorNode
    {
        private readonly FibonacciState _state;
        private readonly string _nodeId;
        private readonly INearestFibonacciCalculator _nearestFibonacciCalculator;
        private int _delayTimeMs = 10;

        public FibonacciCalculatorNode(FibonacciState state, string nodeId, INearestFibonacciCalculator nearestFibonacciCalculator)
        {
            _state = state;
            _nodeId = nodeId;
            _nearestFibonacciCalculator = nearestFibonacciCalculator;
            GetNearestFibonacci();
        }

        public void Run()
        {
            AttemptComputation();
        }

        private void AttemptComputation()
        {
            var success = _state.TryGetAndGrabFirstNotComputedIndex(_nodeId, out var stateItem);
            if (success)
            {
                ComputeAndSave(stateItem);
                AttemptComputation();
            }
            else
            {
                Console.WriteLine($"{_nodeId} has finished calucating.");
            }
        }

        private void ComputeAndSave(FibonacciStateItem stateItem)
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
                seed = 5;
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
                NodeId = _nodeId,
                ComputationState = ComputationState.Computed
            });
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
            return GetOrCalculateFibonacci(index - 2);
        }

        private long GetUltimateFib(int index)
        {
            return GetOrCalculateFibonacci(index - 1);
        }

        private long GetOrCalculateFibonacci(int index)
        {
            if (_state.TryGet(index, _nodeId, out var stateItem))
            {
                WaitForValue(stateItem);
                return stateItem.Fibonacci;
            }

            return Fibonacci(index);
        }

        private void WaitForValue(FibonacciStateItem stateItem)
        {
            if (stateItem.ComputationState == ComputationState.Computing)
            {
                Task.Delay(_delayTimeMs);
            }
            else
            {
                return;
            }

            WaitForValue(stateItem);
        }
    }
}