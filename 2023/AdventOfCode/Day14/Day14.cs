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

            var previousCycles = new List<char[,]>();
            int index;
            int cycleLength;
            const int targetCycleCount = 1000000000;

            while (true)
            {
                Cycle(input);

                index = previousCycles.FindIndex(previous => MultiDimensionalArraysAreEqual(previous, input));
                if (index != -1)
                {
                    cycleLength = previousCycles.Count - index;
                    break;
                }

                previousCycles.Add((char[,]) input.Clone());
            }

            var offset = (targetCycleCount - previousCycles.Count - 1) % cycleLength;

            var secondLoad = CalculateNorthLoad(previousCycles[index + offset]);

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

        private static bool MultiDimensionalArraysAreEqual<T>(T[,] input1, T[,] input2)
        {
            return input1.Rank == input2.Rank &&
                   Enumerable.Range(0, input1.Rank).All(dimension => input1.GetLength(dimension) == input2.GetLength(dimension)) &&
                   input1.Cast<T>().SequenceEqual(input2.Cast<T>());
        }
    }
}
