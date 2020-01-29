using System.Threading.Tasks;

namespace Fibonacci
{
    class Program
    {
        private static readonly FibonacciState State = new FibonacciState();

        static void Main(string[] args)
        {
            var node1 = new FibonacciCalculatorNode(State, "node-1", new NearestFibonacciCalculator());
            var node2 = new FibonacciCalculatorNode(State, "node-2", new NearestFibonacciCalculator());
            var node3 = new FibonacciCalculatorNode(State, "node-3", new NearestFibonacciCalculator());
            var tasks = new[]
            {
                Task.Run(() => node1.Run()),
                Task.Run(() => node2.Run()),
                Task.Run(() => node3.Run())
            };

            Task.WaitAll(tasks);
        }
    }
}
