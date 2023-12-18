namespace Day17
{
    internal class Day17
    {
        private static int _maxRow;
        private static int _maxCol;

        static void Main(string[] args)
        {
            var lines = File.ReadLines("example_input_1.txt").ToArray();

            var input = new int[lines.Length, lines[0].Length];

            for (var row = 0; row < lines.Length; row++)
            {
                for (var col = 0; col < lines[row].Length; col++)
                {
                    input[row, col] = lines[row][col] - '0';
                }
            }

            _maxRow = input.GetLength(0);
            _maxCol = input.GetLength(1);

            // Start, top-left (0, 0), end bottom-right (maxRow - 1, maxCol - 1)

            

            // Printing stuff
            for (var row = 0; row < _maxRow; row++)
            {
                for (var col = 0; col < _maxCol; col++)
                {
                    Console.Write(input[row, col]);
                }

                Console.Write('\n');
                Console.ResetColor();
            }
        }

        private static List<(int Row, int Col)> GetValidNextSteps(List<(int Row, int Col)> previousSteps)
        {
            var possibleSteps = new List<(int Row, int Col)>();

            var (row, col) = previousSteps[^1];

            possibleSteps.Add((row + 1, Col: col));
            possibleSteps.Add((row - 1, Col: col));
            possibleSteps.Add((Row: row, col + 1));
            possibleSteps.Add((Row: row, col - 1));

            if (previousSteps.Count > 1)
            {
                var previous = previousSteps[^2];
                var diff = (Row: row - previous.Row, Col: col - previous.Col);

                possibleSteps.RemoveAll(
                    step => step.Row == row - diff.Row && step.Col == col - diff.Col);

                if (previousSteps.Count > 3)
                {
                    var relevant = previousSteps[^4..];

                    if (relevant.DistinctBy(step => step.Row).Count() == 4 ||
                        relevant.DistinctBy(step => step.Col).Count() == 4)
                    {
                        possibleSteps.RemoveAll(step => step.Row == row + diff.Row && step.Col == col + diff.Col);
                    }
                }
            }

            // Remove steps that accidentally goes outside map
            possibleSteps.RemoveAll(step => step.Row < 0 || step.Row >= _maxRow || step.Col < 0 || step.Col >= _maxCol);

            return possibleSteps;
        }
    }
}
