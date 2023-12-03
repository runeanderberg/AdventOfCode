using System.Text;

namespace Day03
{
    public class Day03
    {
        public static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var numbers = GetNumbers(lines);

            var symbols = GetSymbols(lines);

            var firstSum = numbers.Where(number =>
                    number.Coordinates.Any(coordinate =>
                        symbols.Any(symbol => IsAdjacent(symbol.Coordinate, coordinate))))
                .Sum(number => number.Value);

            var secondSum = symbols.Where(symbol => symbol.Character == '*')
                .Select(symbol =>
                    numbers.Where(number =>
                            number.Coordinates.Any(coordinate => IsAdjacent(symbol.Coordinate, coordinate)))
                        .Select(number => number.Value).ToArray()).Where(matches => matches.Length >= 2)
                .Sum(matches => matches.Aggregate((a, value) => a * value));

            Console.WriteLine($"First sum = {firstSum}, Second sum = {secondSum}");
        }

        private static bool IsAdjacent((int i, int j) p1, (int i, int j) p2)
        {
            return Math.Pow(p2.i - p1.i, 2) + Math.Pow(p2.j - p1.j, 2) <= 2;
        }

        private static List<(int Value, List<(int I, int J)> Coordinates)> GetNumbers(string[] field)
        {
            var result = new List<(int, List<(int, int)>)>();

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

        private static List<(char Character, (int I, int J) Coordinate)> GetSymbols(string[] field)
        {
            var result = new List<(char, (int, int))>();

            for (var i = 0; i < field.Length; i++)
            {
                for (var j = 0; j < field[i].Length; j++)
                {
                    if (!char.IsDigit(field[i][j]) && field[i][j] != '.')
                        result.Add((field[i][j], (i, j)));
                }
            }

            return result;
        }
    }
}