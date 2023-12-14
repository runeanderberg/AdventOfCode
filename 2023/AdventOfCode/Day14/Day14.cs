namespace Day14
{
    internal class Day14
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var input = new char[lines.Length, lines[0].Length];

            for (var row = 0; row < lines.Length; row++)
            {
                for (var col = 0; col < lines[row].Length; col++)
                {
                    input[row, col] = lines[row][col];
                }
            }
            
            TiltNorth(input);

            var firstLoad = CalculateNorthLoad(input);

            const int targetCycleCount = 1000000000;
            const int initialCycleCount = 10000;

            // Iterate a decent amount of times to stabilize
            for (var i = 0; i < initialCycleCount; i++)
            {
                Cycle(input);
            }

            // Start searching for cycle length
            var value = CalculateNorthLoad(input);
            var cycleLength = 0;

            while (true)
            {
                Cycle(input);
                cycleLength++;

                if (CalculateNorthLoad(input) == value)
                    break;
            }

            // Continue cycle until right offset
            var currentOffset = initialCycleCount % cycleLength;
            var targetOffset = targetCycleCount % cycleLength;

            while (currentOffset != targetOffset)
            {
                Cycle(input);
                currentOffset = (currentOffset + 1) % cycleLength;
            }

            var secondLoad = CalculateNorthLoad(input);

            Console.WriteLine($"First north load = {firstLoad}, second north load = {secondLoad}");
        }

        private static void Cycle(char[,] input)
        {
            TiltNorth(input);
            TiltWest(input);
            TiltSouth(input);
            TiltEast(input);
        }

        private static void TiltNorth(char[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            // "Stopping" points for each rock sliding
            var stoppingPoints = new int[numCols];
            for (var col = 0; col < numCols; col++)
            {
                stoppingPoints[col] = input[0, col] == '.' ? 0 : 1;
            }

            for (var row = 1; row < numRows; row++)
            {
                for (var col = 0; col < numCols; col++)
                {
                    if (input[row, col] == '.')
                        continue;
                    
                    if (input[row, col] == '#') 
                    {
                        stoppingPoints[col] = row + 1;
                        continue;
                    }

                    input[row, col] = '.';
                    input[stoppingPoints[col], col] = 'O';
                    stoppingPoints[col]++;
                }
            }
        }

        private static void TiltSouth(char[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            // "Stopping" points for each rock sliding
            var stoppingPoints = new int[numCols];
            for (var col = 0; col < numCols; col++)
            {
                stoppingPoints[col] = input[numRows - 1, col] == '.' ? numRows - 1 : numRows - 2;
            }

            for (var row = numRows - 2; row >= 0; row--)
            {
                for (var col = 0; col < numCols; col++)
                {
                    if (input[row, col] == '.')
                        continue;

                    if (input[row, col] == '#')
                    {
                        stoppingPoints[col] = row - 1;
                        continue;
                    }

                    input[row, col] = '.';
                    input[stoppingPoints[col], col] = 'O';
                    stoppingPoints[col]--;
                }
            }
        }

        private static void TiltWest(char[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            // "Stopping" points for each rock sliding
            var stoppingPoints = new int[numRows];
            for (var row = 0; row < numRows; row++)
            {
                stoppingPoints[row] = input[row, 0] == '.' ? 0 : 1;
            }

            for (var col = 1; col < numCols; col++)
            {
                for (var row = 0; row < numRows; row++)
                {
                    if (input[row, col] == '.')
                        continue;

                    if (input[row, col] == '#')
                    {
                        stoppingPoints[row] = col + 1;
                        continue;
                    }

                    input[row, col] = '.';
                    input[row, stoppingPoints[row]] = 'O';
                    stoppingPoints[row]++;
                }
            }
        }

        private static void TiltEast(char[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            // "Stopping" points for each rock sliding
            var stoppingPoints = new int[numRows];
            for (var row = 0; row < numRows; row++)
            {
                stoppingPoints[row] = input[row, numCols - 1] == '.' ? numCols - 1 : numCols - 2;
            }

            for (var col = numCols - 2; col >= 0; col--)
            {
                for (var row = 0; row < numRows; row++)
                {
                    if (input[row, col] == '.')
                        continue;

                    if (input[row, col] == '#')
                    {
                        stoppingPoints[row] = col - 1;
                        continue;
                    }

                    input[row, col] = '.';
                    input[row, stoppingPoints[row]] = 'O';
                    stoppingPoints[row]--;
                }
            }
        }

        private static int CalculateNorthLoad(char[,] input)
        {
            var numRows = input.GetLength(0);
            var numCols = input.GetLength(1);

            var load = 0;

            for (var row = 0; row < numRows; row++)
            {
                for (var col = 0; col < numCols; col++)
                {
                    if (input[row, col] != 'O')
                        continue;

                    load += numRows - row;
                }
            }

            return load;
        }
    }
}
