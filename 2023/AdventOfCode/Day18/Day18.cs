namespace Day18
{
    internal class Day18
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();
            
            var digInstructions = lines.Select(line => line.Split(' '))
                .Select(split => (Direction: split[0], Length: long.Parse(split[1])));
            var vertices = GetVerticesFromDigInstructions(digInstructions);
            var firstVolume = CalculateVolume(vertices);

            digInstructions = lines.Select(line => line.Split(' '))
                .Select(split => (Direction: DirectionNumberToString(split[^1][^2..^1]), Length: Convert.ToInt64(split[^1][2..^2], 16)));
            vertices = GetVerticesFromDigInstructions(digInstructions);
            var secondVolume = CalculateVolume(vertices);

            Console.WriteLine($"Volumes (part 1, part 2) = ({firstVolume}, {secondVolume})");
        }

        private static string DirectionNumberToString(string direction)
        {
            return direction switch
            {
                "0" => "R",
                "1" => "D",
                "2" => "L",
                "3" => "U",
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        private static List<(long X, long Y)> GetVerticesFromDigInstructions(IEnumerable<(string Direction, long length)> instructions)
        {
            // Start digging at (0, 0), store coordinates of dug spaces
            var points = new List<(long X, long Y)> { (0, 0) };
            long currentX = 0;
            long currentY = 0;

            foreach (var (direction, length) in instructions)
            {
                switch (direction)
                {
                    case "U":
                        currentY -= length;
                        break;
                    case "D":
                        currentY += length;
                        break;
                    case "L":
                        currentX -= length;
                        break;
                    case "R":
                        currentX += length;
                        break;
                }

                points.Add((currentX, currentY));
            }

            return points;
        }

        private static long CalculateVolume(IReadOnlyList<(long X, long Y)> points)
        {
            // Calculate inner area through shoelace formula (https://stackoverflow.com/a/16281192/11186555)
            var innerVolume = Math.Abs(points.Take(points.Count - 1)
                .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                .Sum() / 2);

            // To get the total area, use Pick's theorem, but since only vertices are stored and not all the points on the edges, calculate the sum of points on edges first
            // Since the lines always are horizontal or vertical, it's simple math
            long pointsOnEdges = 0;
            for (var i = 0; i < points.Count - 1; i++)
            {
                pointsOnEdges += Math.Abs(points[i + 1].X - points[i].X) + Math.Abs(points[i + 1].Y - points[i].Y);
            }

            // + 1 instead of - 1 gives correct result, why?
            return innerVolume + pointsOnEdges / 2 + 1;
        }
    }
}
