using System.Text;

namespace Day12
{
    internal class Day12
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToList();

            var firstSum = lines.Sum(GetPossibleArrangementsByRow);

            Console.WriteLine($"First sum = {firstSum}");
        }

        

        private static int GetPossibleArrangementsByRow(string row)
        {
            var data = row.Split(' ');
            var pattern = data[0];
            var numbers = data[1].Split(',').Select(int.Parse).ToList();

            // Brute-force approach, generate all possible patterns, even non-correct ones, and then count how many are valid
            var all = Generate(pattern);
            var res = all.Count(p => ValidatePattern(p, numbers));
            return res;
        }

        private static IEnumerable<string> Generate(string pattern)
        {
            var result = new List<string>();

            if (pattern.All(c => c != '?'))
            {
                result.Add(pattern);
                return result;
            }

            var index = pattern.IndexOf('?');
            var new1 = new StringBuilder(pattern);
            var new2 = new StringBuilder(pattern);

            new1[index] = '#';
            new2[index] = '.';

            result.AddRange(Generate(new1.ToString()));
            result.AddRange(Generate(new2.ToString()));

            return result;
        }

        private static bool ValidatePattern(string pattern, IReadOnlyList<int> records)
        {
            var broken = pattern.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (broken.Length != records.Count)
                return false;

            for (var i = 0; i < broken.Length; i++)
            {
                if (broken[i].Length != records[i])
                    return false;
            }

            return true;
        }
    }
}
