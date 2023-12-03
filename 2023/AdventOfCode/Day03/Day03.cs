using System.Text;

namespace Day03
{
    public class Day03
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var firstSum = PartOne(lines);

            var secondSum = PartTwo(lines);

            Console.WriteLine($"First sum = {firstSum}, Second sum = {secondSum}");
        }

        private static int PartOne(string[] lines)
        {
            var sum = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                var j = 0;
                while (j < lines[i].Length)
                {
                    if (lines[i][j] == '.')
                    {
                        j++;
                        continue;
                    }

                    if (char.IsDigit(lines[i][j]))
                    {
                        var num = new StringBuilder();
                        var chars = new StringBuilder();

                        while (j < lines[i].Length && char.IsDigit(lines[i][j]))
                        {
                            num.Append(lines[i][j]);

                            chars.Append(GetAdjacentCharacters(i, j, lines));

                            j++;
                        }

                        if (chars.ToString().Any(c => c != '.' && !char.IsDigit(c)))
                        {
                            sum += int.Parse(num.ToString());
                        }
                    }

                    j++;
                }
            }

            return sum;
        }

        private static int PartTwo(string[] lines)
        {
            var numbers = GetNumbers(lines);

            var starList = new List<(int i, int j)>();

            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '*')
                        starList.Add((i, j));
                }
            }

            return starList.Select(starLocation => numbers.Where(n => n.Points.Any(p => IsAdjacent(starLocation, p)))
                    .Select(n => n.Number)
                    .ToArray())
                .Where(matches => matches.Length >= 2)
                .Sum(matches => matches.Aggregate((a, v) => a * v));
        }

        private static bool IsAdjacent((int i, int j) p1, (int i, int j) p2)
        {
            return Math.Pow(p2.i - p1.i, 2) + Math.Pow(p2.j - p1.j, 2) <= 2;
        }


        private static string GetAdjacentCharacters(int i, int j, string[] field)
        {
            var maxI = field.Length - 1;
            var maxJ = field[0].Length - 1;

            var result = new StringBuilder();

            result.Append(field[int.Max(0, i - 1)][int.Max(0, j - 1)]);
            result.Append(field[int.Max(0, i - 1)][j]);
            result.Append(field[int.Max(0, i - 1)][int.Min(maxJ, j + 1)]);

            result.Append(field[i][int.Max(0, j - 1)]);
            result.Append(field[i][int.Min(maxJ, j + 1)]);

            result.Append(field[int.Min(maxI, i + 1)][int.Max(0, j - 1)]);
            result.Append(field[int.Min(maxI, i + 1)][j]);
            result.Append(field[int.Min(maxI, i + 1)][int.Min(maxJ, j + 1)]);

            return result.ToString();
        }

        private static List<(int Number, List<(int I, int J)> Points)> GetNumbers(string[] field)
        {
            var result = new List<(int, List<(int i, int j)>)>();

            for (var i = 0; i < field.Length; i++)
            {
                var j = 0;
                while (j < field[i].Length)
                {
                    if (!char.IsDigit(field[i][j]))
                    {
                        j++;
                        continue;
                    }

                    if (char.IsDigit(field[i][j]))
                    {
                        var num = new StringBuilder();
                        var points = new List<(int i, int j)>();

                        while (j < field[i].Length && char.IsDigit(field[i][j]))
                        {
                            num.Append(field[i][j]);
                            points.Add((i, j));
                            j++;
                        }
                        result.Add((int.Parse(num.ToString()), points));
                    }

                    j++;
                }
            }

            return result;
        }
    }
}
