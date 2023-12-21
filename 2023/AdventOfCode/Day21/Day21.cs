namespace Day21
{
    internal class Day21
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var map = new char[lines.Length, lines[0].Length];

            (int Row, int Col) start = (0, 0);

            for (var row = 0; row < lines.Length; row++)
            {
                for (var col = 0; col < lines[row].Length; col++)
                {
                    map[row, col] = lines[row][col];

                    if (map[row, col] == 'S')
                        start = (row, col);
                }
            }

            var rowLength = map.GetLength(0);
            var colLength = map.GetLength(1);

            var visitedMap = new List<int>?[rowLength, colLength];
            var endPoints = new List<(int Row, int Col)>();

            var queue = new Queue<((int Row, int Col) Position, int RemainingSteps)>();
            queue.Enqueue((start, 64));

            while (queue.Count > 0)
            {
                var ((row, col), remainingSteps) = queue.Dequeue();

                if (visitedMap[row, col]?.Contains(remainingSteps) ?? false)
                    continue;

                visitedMap[row, col] ??= new();

                visitedMap[row, col]!.Add(remainingSteps);

                if (remainingSteps == 0)
                {
                    endPoints.Add((row, col));
                    continue;
                }

                var toCheck = new List<((int Row, int Col) Position, int RemainingSteps)>
                {
                    ((row + 1, col), remainingSteps - 1),
                    ((row - 1, col), remainingSteps - 1),
                    ((row, col + 1), remainingSteps - 1),
                    ((row, col - 1), remainingSteps - 1)
                };
                
                foreach (var step in toCheck
                             .Where(step => step.Position.Row >= 0 && step.Position.Row < rowLength && 
                                            step.Position.Col >= 0 && step.Position.Col < colLength && 
                                            map[step.Position.Row, step.Position.Col] != '#'))
                {
                    queue.Enqueue(step);
                }
            }


            for (var row = 0; row < rowLength; row++)
            {
                for (var col = 0; col < colLength; col++)
                {
                    Console.BackgroundColor = visitedMap[row, col]?.Any(i => i == 0) ?? false ? ConsoleColor.DarkGreen : ConsoleColor.Black;
                    Console.Write(map[row, col]);
                }
                Console.Write('\n');
            }
            Console.WriteLine();

            Console.WriteLine($"First sum = {endPoints.Distinct().Count()}");
        }
    }
}
