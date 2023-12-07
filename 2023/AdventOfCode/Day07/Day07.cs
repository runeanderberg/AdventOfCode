namespace Day07
{
    internal class Day07
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var hands = lines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(input => new Hand(input[0].ToCharArray(), int.Parse(input[1]))).ToList();
            hands.Sort();

            long firstSum = 0;
            for (var i = 0; i < hands.Count; i++)
            {
                firstSum += (i + 1) * hands[i].Bid;
            }

            var jokerHands = lines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(input => new Hand(input[0].ToCharArray(), int.Parse(input[1]), true)).ToList();
            jokerHands.Sort();

            long secondSum = 0;
            for (var i = 0; i < jokerHands.Count; i++)
            {
                secondSum += (i + 1) * jokerHands[i].Bid;
            }

            Console.WriteLine($"First sum = {firstSum}, second sum = {secondSum}");
        }
    }

    internal class Hand : IComparable<Hand>
    {
        private static readonly Dictionary<char, int> Strengths = new()
        {
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'T', 10 },
            { 'J', 11 },
            { 'Q', 12 },
            { 'K', 13 },
            { 'A', 14 }
        };

        private static readonly Dictionary<char, int> JokerRuleStrengths = new()
        {
            { 'J', 1 },
            { '2', 2 },
            { '3', 3 },
            { '4', 4 },
            { '5', 5 },
            { '6', 6 },
            { '7', 7 },
            { '8', 8 },
            { '9', 9 },
            { 'T', 10 },
            { 'Q', 11 },
            { 'K', 12 },
            { 'A', 13 }
        };

        public char[] Values { get; }
        public int Bid { get; }
        public int Strength { get; }
        public bool UsesJokerRule { get; }

        public Hand(char[] values, int bid, bool usesJokerRule = false)
        {
            Values = values;
            Bid = bid;
            UsesJokerRule = usesJokerRule;

            var numMatches = Values.Select(c => Values.Count(iC => iC == c || (UsesJokerRule && iC == 'J'))).ToArray();

            var max = numMatches.Max();

            Strength = max switch
            {
                5 or 4 => max + 1,
                3 when UsesJokerRule && values.Any(c => c == 'J') => numMatches.Any(m => m == 2) ? 3 : 4,
                3 => numMatches.Any(m => m == 2) ? 4 : 3,
                2 when UsesJokerRule && values.Any(c => c == 'J') => 1,
                2 => numMatches.Count(m => m == 2) == 4 ? 2 : 1,
                _ => 0
            };
        }

        public int CompareTo(Hand? other)
        {
            if (other is null)
            {
                return -1;
            }

            if (this == other)
            {
                return 0;
            }

            if (Strength != other.Strength)
                return Strength - other.Strength;

            var strengths = UsesJokerRule ? JokerRuleStrengths : Strengths;

            var i = 0;
            while (strengths[Values[i]] == strengths[other.Values[i]])
            {
                i++;
            }

            return strengths[Values[i]] - strengths[other.Values[i]];
        }
    }
}