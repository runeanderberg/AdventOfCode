namespace Helpers
{
    public static class Maths
    {
        public static long LCM(IEnumerable<long> values)
        {
            var lcm = values.First();

            foreach (var value in values.Skip(1))
            {
                var gcdVal = GCD(lcm, value);
                lcm = (lcm * value) / gcdVal;
            }

            return lcm;
        }

        public static long GCD(long a, long b)
        {
            while (true)
            {
                if (b == 0) return a;
                var c = a;
                a = b;
                b = c % b;
            }
        }
    }
}