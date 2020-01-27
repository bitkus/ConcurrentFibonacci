using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fibonacci
{
    class Program
    {
        private static readonly FibonacciState State = new FibonacciState();

        static void Main(string[] args)
        {
            var nearestFibonacciCalculator = new NearestFibonacciCalculator();
            var node1 = new FibonacciCalculatorNode(State, "node-1", nearestFibonacciCalculator);
            var node2 = new FibonacciCalculatorNode(State, "node-2", nearestFibonacciCalculator);
            var tasks = new[]
            {
                Task.Run(() => node1.Run()),
                Task.Run(() => node2.Run())
            };

            Task.WaitAll(tasks);
        }

        private static void UpdateState(FibonacciStateItem stateItem)
        {
            State.Add(stateItem);
        }
    }
}
