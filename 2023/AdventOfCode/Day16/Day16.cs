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

            var beams = new List<Beam> { new(BeamDirection.Right, 0, 0) };

            var previousBeamLocations = new HashSet<(int Row, int Col, BeamDirection Direction)>();

            while (beams.Count > 0)
            {
                var toAdd = new List<Beam>();
                var toRemove = new List<Beam>();

                //Console.Clear();
                //PrintCurrentState(input, beams);
                //Console.ReadLine();

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

                    if (beam.CurrentRow < 0 || beam.CurrentRow >= input.GetLength(0) ||
                        beam.CurrentCol < 0 || beam.CurrentCol >= input.GetLength(1))
                    {
                        toRemove.Add(beam);
                    }
                }

                foreach (var beam in toRemove)
                {
                    beams.Remove(beam);
                }

                beams.AddRange(toAdd.Where(beam => beam.CurrentRow >= 0 && beam.CurrentRow < input.GetLength(0) && beam.CurrentCol >= 0 && beam.CurrentCol < input.GetLength(1)));
            }

            Console.WriteLine($"First sum = {previousBeamLocations.DistinctBy(beam => (beam.Row, beam.Col)).Count()}");

            for (var row = 0; row < input.GetLength(0); row++)
            {
                for (var col = 0; col < input.GetLength(1); col++)
                {
                    var matchingBeams = previousBeamLocations.Where(beam => beam.Row == row && beam.Col == col).ToArray();

                    Console.BackgroundColor = matchingBeams.Length > 0 ? ConsoleColor.DarkRed : ConsoleColor.Black;

                    Console.Write(input[row, col]);
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write('\n');
            }
        }

        static void PrintCurrentState(char[,] input, IReadOnlyCollection<Beam> beams)
        {
            for (var row = 0; row < input.GetLength(0); row++)
            {
                for (var col = 0; col < input.GetLength(1); col++)
                {
                    var matchingBeams = beams.Where(beam => beam.CurrentRow == row && beam.CurrentCol == col).ToArray();

                    if (matchingBeams.Length > 0)
                    {
                        if (matchingBeams.Length > 1)
                        {
                            Console.Write(matchingBeams.Length);
                        }
                        else
                        {
                            var c = matchingBeams[0].Direction switch
                            {
                                BeamDirection.Up => '^',
                                BeamDirection.Down => 'v',
                                BeamDirection.Left => '<',
                                BeamDirection.Right => '>',
                                _ => throw new ArgumentOutOfRangeException()
                            };
                            Console.Write(c);
                        }
                    }
                    else
                    {
                        Console.Write(input[row, col]);
                    }
                }
                Console.Write('\n');
            }
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