using Helpers;

namespace Day23
{
    internal class Day23
    {
        static void Main(string[] args)
        {
            var map = File.ReadAllLines("input.txt").To2DArray(c => c);

            var rowLength = map.GetLength(0);
            var colLength = map.GetLength(1);

            var start = (Row: 0, Col: 0);
            var end = (Row: 0, Col: 0);

            for (var col = 0; col < colLength; col++)
            {
                if (map[0, col] != '.') 
                    continue;
                start = (0, col);

                break;
            }

            for (var col = 0; col < colLength; col++)
            {
                if (map[rowLength - 1, col] != '.')
                    continue;
                end = (rowLength - 1, col);

                break;
            }

            Console.WriteLine($"First sum = {FindLongestSlipperyPathLength(map, start, end)}");

            Console.WriteLine($"Second sum = {FindLongestNonSlipperyPathLength(map, start, end)}");
        }

        private static int FindLongestSlipperyPathLength(char[,] map, (int Row, int Col) start, (int Row, int Col) end)
        {
            var rowLength = map.GetLength(0);
            var colLength = map.GetLength(1);

            var queue = new Queue<(int Row, int Col, List<(int Row, int Col)> PreviousSteps)>();
            queue.Enqueue((start.Row, start.Col, new List<(int Row, int Col)>()));

            var stepsMap = new int[rowLength, colLength];

            while (queue.Count > 0)
            {
                var (row, col, previousSteps) = queue.Dequeue();

                if (stepsMap[row, col] != 0 && stepsMap[row, col] + 2 >= previousSteps.Count)
                {
                    continue;
                }

                stepsMap[row, col] = previousSteps.Count;

                var toCheck = new List<(int Row, int Col, List<(int Row, int Col)>)>();

                switch (map[row, col])
                {
                    case '.':
                        toCheck.Add((row - 1, col, new List<(int Row, int Col)>(previousSteps) { (row - 1, col) }));
                        toCheck.Add((row + 1, col, new List<(int Row, int Col)>(previousSteps) { (row + 1, col) }));
                        toCheck.Add((row, col - 1, new List<(int Row, int Col)>(previousSteps) { (row, col - 1) }));
                        toCheck.Add((row, col + 1, new List<(int Row, int Col)>(previousSteps) { (row, col + 1) }));

                        foreach (var step in toCheck.Where(step => step.Row >= 0 && step.Row < rowLength &&
                                                                   step.Col >= 0 && step.Col < colLength &&
                                                                   map[step.Row, step.Col] != '#'))
                        {
                            queue.Enqueue(step);
                        }
                        break;
                    case '>':
                        queue.Enqueue((row, col + 1, new List<(int Row, int Col)>(previousSteps) { (row, col + 1) }));
                        break;
                    case 'v':
                        queue.Enqueue((row + 1, col, new List<(int Row, int Col)>(previousSteps) { (row + 1, col) }));
                        break;
                }
            }
            
            return stepsMap[end.Row, end.Col];
        }

        private static int FindLongestNonSlipperyPathLength(char[,] map, (int Row, int Col) start, (int Row, int Col) end)
        {
            var rowLength = map.GetLength(0);
            var colLength = map.GetLength(1);

            var queue = new Queue<(int Row, int Col, HashSet<(int Row, int Col)> PreviousSteps)>();
            queue.Enqueue((start.Row, start.Col, new HashSet<(int Row, int Col)>()));

            var stepsMap = new int[rowLength, colLength];
            var paths = new List<HashSet<(int Row, int Col)>>();
            
            while (queue.Count > 0)
            {
                var (row, col, previousSteps) = queue.Dequeue();

                if (previousSteps.Contains((row, col)))
                    continue;

                if (stepsMap[row, col] != 0 && previousSteps.Count <= stepsMap[row, col])
                    continue;

                stepsMap[row, col] = previousSteps.Count;
                previousSteps.Add((row, col));

                if ((row, col) == end)
                {
                    paths.Add(previousSteps);
                }

                var toCheck = new List<(int Row, int Col, HashSet<(int Row, int Col)>)>
                {
                    (row - 1, col, new(previousSteps)),
                    (row + 1, col, new(previousSteps)),
                    (row, col - 1, new(previousSteps)),
                    (row, col + 1, new(previousSteps))
                };

                foreach (var step in toCheck.Where(step => step.Row >= 0 && step.Row < rowLength &&
                                                           step.Col >= 0 && step.Col < colLength &&
                                                           map[step.Row, step.Col] != '#'))
                {
                    queue.Enqueue(step);
                }
            }
            
            var path = paths.MaxBy(path => path.Count);

            Console.WriteLine();
            for (var row = 0; row < rowLength; row++)
            {
                for (var col = 0; col < colLength; col++)
                {
                    Console.BackgroundColor = path!.Contains((row, col)) ? ConsoleColor.DarkGreen : ConsoleColor.Black;
                    Console.Write(map[row, col]);
                }
                Console.ResetColor();
                Console.Write('\n');
            }

            return stepsMap[end.Row, end.Col];
        }
    }
}
