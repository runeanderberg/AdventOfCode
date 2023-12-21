namespace Day09
{
    internal class Day09
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var patternData = lines
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .Select(CreatePatternData).ToArray();

            var firstSum = patternData.Select(ExtrapolateNextValue).Sum();

            var secondSum = patternData.Select(ExtrapolatePreviousValue).Sum();

            Console.WriteLine($"First sum = {firstSum}, second sum = {secondSum}");
        }

        private static int[][] CreatePatternData(int[] input)
        {
            var list = new List<int[]>();
            list.Insert(0, input);

            var previous = input;

            while (previous.Any(i => i != 0))
            {
                var newArray = new int[previous.Length - 1];

                for (var i = 0; i < newArray.Length; i++)
                {
                    newArray[i] = previous[i + 1] - previous[i];
                }

                list.Insert(0, newArray);
                previous = newArray;
            }

            return list.ToArray();
        }

        private static int ExtrapolateNextValue(int[][] input)
        {
            var value = 0;

            foreach (var array in input)
            {
                value += array[^1];
            }

            return value;
        }

        private static int ExtrapolatePreviousValue(int[][] input)
        {
            var value = 0;

            foreach (var array in input)
            {
                value = -value + array[0];
            }

            return value;
        }
    }
}