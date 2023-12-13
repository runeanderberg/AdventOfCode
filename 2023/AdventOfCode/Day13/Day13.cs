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

            // Search for horisontal reflections
            var horisontalSum = 0;
            foreach (var input in inputs)
            {
                for (var row = 0; row < input.GetLength(0) - 1; row++)
                {
                    var col = 0;
                    while (col < input.GetLength(1) && input[row, col] == input[row + 1, col]) col++;

                    if (col != input.GetLength(1))
                        continue;

                    // Found two matching horisontal lines => middle of pattern I think
                    // Now check outwards until no longer matching
                    var a = row - 1;
                    var b = row + 2;
                    var reflection = true;
                    while (a >= 0 && b < input.GetLength(0))
                    {
                        col = 0;
                        while (col < input.GetLength(1) && input[a, col] == input[b, col]) col++;

                        if (col != input.GetLength(1))
                        {
                            // Found non-matching, not a reflection
                            reflection = false;
                            break;
                        }

                        a--;
                        b++;
                    }

                    if (!reflection)
                        continue;

                    horisontalSum += 100 * (row + 1);
                    break;
                }
            }

            // Search for vertical reflections
            var verticalSum = 0;
            foreach (var input in inputs)
            {
                for (var col = 0; col < input.GetLength(1) - 1; col++)
                {
                    var row = 0;
                    while (row < input.GetLength(0) && input[row, col] == input[row, col + 1]) row++;

                    if (row != input.GetLength(0))
                        continue;

                    // Found two matching vertical lines => middle of pattern I think
                    // Now check outwards until no longer matching
                    var a = col - 1;
                    var b = col + 2;
                    var reflection = true;
                    while (a >= 0 && b < input.GetLength(1))
                    {
                        row = 0;
                        while (row < input.GetLength(0) && input[row, a] == input[row, b]) row++;

                        if (row != input.GetLength(0))
                        {
                            // Found non-matching, not a reflection
                            reflection = false;
                            break;
                        }

                        a--;
                        b++;
                    }

                    if (!reflection)
                        continue;

                    verticalSum += (col + 1);
                    break;
                }
            }

            Console.WriteLine($"First sum = {horisontalSum + verticalSum}");
        }
    }
}