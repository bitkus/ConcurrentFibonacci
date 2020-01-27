using System;

namespace Fibonacci
{
    public static class Helpers
    {
        public static double Root5 = Math.Sqrt(5);
        public static double Phi = (1 + Root5) / 2;

        public static int GetFibonacciIndex(long fib)
        {
            double index = Math.Log(fib * Root5 + 0.5d) / Math.Log(Phi);
            return (int)Math.Round(index, MidpointRounding.ToEven);
        }

        public static bool IsNumberFibonacci(long number)
        {
            var index = GetFibonacciIndex(number);
            var fib = Math.Round(Math.Pow(Phi, index) / Root5, MidpointRounding.ToEven);
            return number == fib;
        }
    }
}
