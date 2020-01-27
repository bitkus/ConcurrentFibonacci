namespace Fibonacci
{
    public class NearestFibonacciCalculator : INearestFibonacciCalculator
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
                isFib = Helpers.IsNumberFibonacci(candidate);
                if (!isFib)
                {
                    candidate += increment;
                }
            } while (!isFib);

            return candidate;
        }
    }
}