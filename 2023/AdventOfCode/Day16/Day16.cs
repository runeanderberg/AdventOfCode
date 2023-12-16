using static Day16.Beam;

namespace Day16
{
    internal class Day16
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

            var startBeams = new List<Beam>();
            var maxRow = input.GetLength(0);
            var maxCol = input.GetLength(1);

            for (var row = 0; row < maxRow; row++)
            {
                startBeams.Add(new Beam(BeamDirection.Right, row, 0));
                startBeams.Add(new Beam(BeamDirection.Left, row, maxCol - 1));
            }

            for (var col = 0; col < maxCol; col++)
            {
                startBeams.Add(new Beam(BeamDirection.Down, 0, col));
                startBeams.Add(new Beam(BeamDirection.Up, maxRow - 1, col));
            }

            var max = startBeams.Max(beam => CalculateEnergy(input, beam));

            Console.WriteLine(
                $"Number of energized tiles (part 1, part 2 max) = ({CalculateEnergy(input, new Beam(BeamDirection.Right, 0, 0))}, {max})");
        }

        private static int CalculateEnergy(char[,] input, Beam startBeam, bool printResultMap = false)
        {
            var maxRow = input.GetLength(0);
            var maxCol = input.GetLength(1);

            var beams = new List<Beam> { startBeam };
            var previousBeamLocations = new HashSet<(int Row, int Col, BeamDirection Direction)>();

            while (beams.Count > 0)
            {
                var toAdd = new List<Beam>();
                var toRemove = new List<Beam>();

                foreach (var beam in beams)
                {
                    // Identical to a beam that has already been processed, "destroy" beam so it can eventually terminate
                    if (previousBeamLocations.Contains((beam.CurrentRow, beam.CurrentCol, beam.Direction)))
                    {
                        toRemove.Add(beam);
                        continue;
                    }

                    previousBeamLocations.Add((beam.CurrentRow, beam.CurrentCol, beam.Direction));

                    switch (input[beam.CurrentRow, beam.CurrentCol])
                    {
                        case '/':
                            beam.Direction = beam.Direction switch
                            {
                                BeamDirection.Up => BeamDirection.Right,
                                BeamDirection.Down => BeamDirection.Left,
                                BeamDirection.Left => BeamDirection.Down,
                                BeamDirection.Right => BeamDirection.Up,
                                _ => throw new ArgumentOutOfRangeException()
                            };
                            break;
                        case '\\':
                            beam.Direction = beam.Direction switch
                            {
                                BeamDirection.Up => BeamDirection.Left,
                                BeamDirection.Down => BeamDirection.Right,
                                BeamDirection.Left => BeamDirection.Up,
                                BeamDirection.Right => BeamDirection.Down,
                                _ => throw new ArgumentOutOfRangeException()
                            };
                            break;
                        case '|' when beam.Direction is BeamDirection.Up or BeamDirection.Down:
                            break;
                        case '|':
                        {
                            beam.Direction = BeamDirection.Up;
                            var newBeam = beam.GetSplitBeam();
                            newBeam.Step();
                            toAdd.Add(newBeam);
                            break;
                        }
                        case '-' when beam.Direction is BeamDirection.Left or BeamDirection.Right:
                            break;
                        case '-':
                        {
                            beam.Direction = BeamDirection.Left;
                            var newBeam = beam.GetSplitBeam();
                            newBeam.Step();
                            toAdd.Add(newBeam);
                            break;
                        }
                    }

                    beam.Step();

                    if (beam.CurrentRow < 0 || beam.CurrentRow >= maxRow ||
                        beam.CurrentCol < 0 || beam.CurrentCol >= maxCol)
                    {
                        toRemove.Add(beam);
                    }
                }

                foreach (var beam in toRemove)
                {
                    beams.Remove(beam);
                }

                beams.AddRange(toAdd.Where(beam =>
                    beam.CurrentRow >= 0 && beam.CurrentRow < maxRow && beam.CurrentCol >= 0 &&
                    beam.CurrentCol < maxCol));
            }

            if (!printResultMap)
                return previousBeamLocations.DistinctBy(beam => (beam.Row, beam.Col)).Count();

            for (var row = 0; row < maxRow; row++)
            {
                for (var col = 0; col < maxCol; col++)
                {
                    var matchingBeams = previousBeamLocations.Where(beam => beam.Row == row && beam.Col == col)
                        .ToArray();

                    Console.BackgroundColor = matchingBeams.Length > 0 ? ConsoleColor.DarkRed : ConsoleColor.Black;

                    Console.Write(input[row, col]);
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write('\n');
            }

            return previousBeamLocations.DistinctBy(beam => (beam.Row, beam.Col)).Count();
        }
    }

    internal class Beam(BeamDirection direction, int currentRow, int currentCol)
    {
        public BeamDirection Direction { get; set; } = direction;

        public int CurrentRow { get; set; } = currentRow;
        public int CurrentCol { get; set; } = currentCol;

        public void Step()
        {
            switch (Direction)
            {
                case BeamDirection.Up:
                    CurrentRow--;
                    break;
                case BeamDirection.Down:
                    CurrentRow++;
                    break;
                case BeamDirection.Left:
                    CurrentCol--;
                    break;
                case BeamDirection.Right:
                    CurrentCol++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Beam GetSplitBeam()
        {
            var direction = Direction switch
            {
                BeamDirection.Up => BeamDirection.Down,
                BeamDirection.Down => BeamDirection.Up,
                BeamDirection.Left => BeamDirection.Right,
                BeamDirection.Right => BeamDirection.Left,
                _ => throw new ArgumentOutOfRangeException()
            };

            return new Beam(direction, CurrentRow, CurrentCol);
        }

        internal enum BeamDirection
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}