using System;

namespace Fibonacci
{
    public class NearestFibonacciCalculator
    {
        public long GetNearestFib(long number)
        {
            var positive = GetNearestFibPositive(number);
            var negative = GetNearestFibNegative(number);
            return positive - number > number - negative ? negative : positive;
        }

        private long GetNearestFibNegative(long number)
        {
            return GetNearestFib(number, -1);
        }

        private long GetNearestFibPositive(long number)
        {
            return GetNearestFib(number, 1);
        }

        private long GetNearestFib(long number, int increment)
        {
            var candidate = number;
            var isFib = false;

            do
            {
                isFib = IsFib(candidate);
                if (!isFib)
                {
                    candidate += increment;
                }
            } while (!isFib);

            return candidate;
        }

        private bool IsFib(long number)
        {
            return IsWholeNumberWithPrecision(Math.Sqrt(5 * number * number + 4)) || IsWholeNumberWithPrecision(Math.Sqrt(5 * number * number - 4));
        }

        private bool IsWholeNumberWithPrecision(double number)
        {
            return Math.Abs(number % 1) <= Double.Epsilon;
        }
    }
}