using System;
using System.Collections.Generic;

namespace Fibonacci
{
    class Program
    {
        private static readonly FibonacciState State = new FibonacciState();

        static void Main(string[] args)
        {
            var nearestFibonacciCalculator = new NearestFibonacciCalculator();
            var fibonacciSequenceCalculator = new FibonacciSequenceCalculator();
            Console.WriteLine(nearestFibonacciCalculator.GetNearestFib(11));

            for (int i = 0; i < 10; i++)
            {
                var fib = fibonacciSequenceCalculator.GetNext(State);
                UpdateState(fib);
                Console.WriteLine(fib.Value);
            }
        }

        private static void UpdateState(KeyValuePair<int, long> fib)
        {
            State.Add(fib.Key, fib.Value);
        }
    }
}
