namespace Day13
{
    internal class Day13
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var indexes = lines.Select((line, index) => (line, index)).Where(x => string.IsNullOrEmpty(x.line))
                .Select(x => x.index).ToList();
            indexes.Insert(0, -1);
            indexes.Add(lines.Length);

            var inputs = new List<char[,]>();

            for (var i = 0; i < indexes.Count - 1; i++)
            {
                var inputLines = lines[(indexes[i] + 1)..indexes[i + 1]];
                var input = new char[inputLines.Length, inputLines[0].Length];

                for (var row = 0; row < inputLines.Length; row++)
                {
                    for (var col = 0; col < inputLines[row].Length; col++)
                    {
                        input[row, col] = inputLines[row][col];
                    }
                }

                inputs.Add(input);
            }

            // Search for horizontal reflections
            var potentialHorizontal = inputs.Select(input => (Input: input, Rows: FindPotentialHorizontalReflections(input)));
            var horizontalSum = potentialHorizontal.Sum(potential => potential.Rows.Sum(row => TestHorizontalReflection(potential.Input, row)));

            // Search for vertical reflections
            var potentialVertical = inputs.Select(input => (Input: input, Cols: FindPotentialVerticalReflections(input)));
            var verticalSum = potentialVertical.Sum(potential => potential.Cols.Sum(row => TestVerticalReflection(potential.Input, row)));


            Console.WriteLine($"First sum = {horizontalSum + verticalSum}");
        }

        private static List<int> FindPotentialHorizontalReflections(char[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            var list = new List<int>();

            for (var row = 0; row < numRows - 1; row++)
            {
                var col = 0;
                while (col < numCols && input[row, col] == input[row + 1, col]) col++;

                if (col != numCols)
                    continue;

                list.Add(row);
            }

            return list;
        }

        private static List<int> FindPotentialVerticalReflections(char[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            var list = new List<int>();

            for (var col = 0; col < numCols - 1; col++)
            {
                var row = 0;
                while (row < numRows && input[row, col] == input[row, col + 1]) row++;

                if (row != numRows)
                    continue;

                list.Add(col);
            }

            return list;
        }

        private static int TestHorizontalReflection(char[,] input, int startRow)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            var a = startRow - 1;
            var b = startRow + 2;

            while (a >= 0 && b < numRows)
            {
                var col = 0;
                while (col < numCols && input[a, col] == input[b, col]) col++;

                if (col != numCols)
                {
                    // Found non-matching, not a reflection
                    return 0;
                }

                a--;
                b++;
            }
            
            return 100 * (startRow + 1);
        }

        private static int TestVerticalReflection(char[,] input, int startCol)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            var a = startCol - 1;
            var b = startCol + 2;

            while (a >= 0 && b < numCols)
            {
                var row = 0;
                while (row < numRows && input[row, a] == input[row, b]) row++;

                if (row != numRows)
                {
                    // Found non-matching, not a reflection
                    return 0;
                }

                a--;
                b++;
            }

            return startCol + 1;
        }
    }
}