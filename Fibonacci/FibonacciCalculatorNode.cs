using System;
using System.Threading;

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
            _state.RegisterNode(_nodeId);
        }

        public void Run()
        {
            FindNearestFibonacci();
            WaitForEveryNode();
            AttemptComputation();
        }

        private void FindNearestFibonacci()
        {
            var random = new Random();
            var seed = random.Next();
            if (_nodeId == "node-1")
            {
                seed = random.Next(10000);
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
            Console.WriteLine($"{_nodeId} has found the nearest fibonacci to be {nearestFib}");
            _state.NoteNodeReady(_nodeId);
        }

        private void WaitForEveryNode()
        {
            if (!_state.IsEveryNodeReady)
            {
                Thread.Sleep(_delayTimeMs);
                WaitForEveryNode();
            }
        }

        private void AttemptComputation()
        {
            var success = _state.TryGetAndGrabNextAvailableIndex(_nodeId, out var index);
            if (success)
            {
                ComputeAndSave(index);
                AttemptComputation();
            }
            else
            {
                Console.WriteLine($"{_nodeId} has finished computing.");
            }
        }

        private void ComputeAndSave(int index)
        {
            var fibonacci = Fibonacci(index);
            _state.Update(index, fibonacci);
            Console.WriteLine($"{_nodeId},{fibonacci}");
        }

        private long Fibonacci(int index)
        {
            return index switch
            {
                0 => 0,
                1 => 1,
                _ => GetOrCalculateFibonacci(index - 2) + GetOrCalculateFibonacci(index - 1)
            };
        }

        private long GetOrCalculateFibonacci(int index)
        {
            if (_state.IsFibonacciMissing(index))
            {
                return Helpers.CalculateFibonacciAnalytically(index); // specifically choosing not use recursion
            }

            WaitForData(index, out var fibonacci);
            return fibonacci;
        }

        private void WaitForData(int index, out long fibonacci)
        {
            if (_state.TryGet(index, out fibonacci))
            {
                return;
            }

            Thread.Sleep(_delayTimeMs);
            WaitForData(index, out fibonacci);
        }
    }
}