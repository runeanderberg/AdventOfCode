using System.Text;

namespace Day12
{
    internal class Day12
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("test_input.txt").ToList();

            var firstSum = lines.Sum(GetPossibleArrangementsByRow);

            var unfoldedLines = lines.Select(Unfold);

            // Brute-forcing the unfolded lines is essentially impossible, there are just too many combinations to check
            // Ideas to solve the problem:
            //  - Splitting the pattern into sub-problems based on .s and then multiply to get total number of possibilities per row
            //      * Ex. ??.??.?? 1,1,1, can be split into 3 sub-problems (?? 1 => 2) which gives 2^3 as the final result
            //      * One issue is that some lines, ex. ??#??#????##, can't be split that easily
            //      * Another issue is that the number of groups separated by a . is not always the same as the number of contiguous group numbers, ex. ??.??.?? 1,1 is valid
            //  - It is probably wise to try to figure out the matches of groups of only # with it's respective contiguous group before doing other calculations
            //      * Ex. in ????.######..#####. 1,6,5, the matching is obvious and the problem can be reduced to sub-problem ???? 1
            //      * Secondly, in .??..??...?##. 1,1,3, only the last group can match with the 3, which should also be figured out early

            Console.WriteLine($"First sum = {firstSum}, second sum = {0}");
        }

        private static string Unfold(string row)
        {
            var data = row.Split(' ');
            var pattern = new StringBuilder(data[0]);
            var numbers = new StringBuilder(data[1]);

            for (var i = 0; i < 4; i++)
            {
                pattern.Append('?').Append(data[0]);
                numbers.Append(',').Append(data[1]);
            }

            return pattern + " " + numbers;
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

        private static List<string> Generate(string pattern)
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