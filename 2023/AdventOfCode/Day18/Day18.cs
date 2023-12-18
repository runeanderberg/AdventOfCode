namespace Day18
{
    internal class Day18
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var digInstructions = lines.Select(line => line.Split(' '))
                .Select(split => (Direction: split[0], Length: int.Parse(split[1])));

            // Start digging at (0, 0), store coordinates of dug spaces
            var points = new List<(int X, int Y)> { (0, 0) };
            var currentX = 0;
            var currentY = 0;

            foreach (var (direction, length) in digInstructions)
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

            points = TranslatePoints(points);
            var volume = CalculateVolume(points);

            var lengthX = points.Max(space => space.X) + 1;
            var lengthY = points.Max(space => space.Y) + 1;
            
            //for (var y = 0; y < lengthY; y++)
            //{
            //    for (var x = 0; x < lengthX; x++)
            //    {
            //        Console.BackgroundColor = IsPointInPolygon((x, y), points) ? ConsoleColor.DarkRed : ConsoleColor.Black;
            //        Console.Write(points.Any(point => point.X == x && point.Y == y) ? '#' : '.');
            //    }
            //    Console.BackgroundColor = ConsoleColor.Black;
            //    Console.Write("\n");
            //}

            Console.WriteLine($"First sum = {volume}");

            digInstructions = lines.Select(line => line.Split(' '))
                .Select(split => (Direction: DirectionNumberToString(split[^1][^2..^1]), Length: Convert.ToInt32(split[^1][2..^2], 16)));

            currentX = 0;
            currentY = 0;
            points = new List<(int X, int Y)> { (0, 0) };

            foreach (var (direction, length) in digInstructions)
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

            points = TranslatePoints(points);

            // Calculating volume with point in polygon is just too slow, something something shoelace formula I'm guessing
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

        private static List<(int X, int Y)> TranslatePoints(IReadOnlyCollection<(int X, int Y)> points)
        {
            var minX = points.Min(space => space.X);
            var minY = points.Min(space => space.Y);

            return points.Select(point => (point.X - minX, point.Y - minY)).ToList();
        }

        private static long CalculateVolume(IList<(int X, int Y)> points)
        {
            var lengthX = points.Max(space => space.X) + 1;
            var lengthY = points.Max(space => space.Y) + 1;

            long area = 0;

            for (var y = 0; y < lengthY; y++)
            {
                for (var x = 0; x < lengthX; x++)
                {
                    if (IsPointInPolygon((x, y), points))
                    {
                        area++;
                    }
                }
            }

            return area;
        }

        // https://stackoverflow.com/a/57624683/11186555 but with a fix cause it's incorrect
        private static bool IsPointInPolygon((int X, int Y) point, IList<(int X, int Y)> polygon)
        {
            var intersects = new List<int>();
            var a = polygon.Last();
            foreach (var b in polygon)
            {
                if (b.X == point.X && b.Y == point.Y)
                {
                    return true;
                }

                if (b.X == a.X && point.X == a.X && point.Y >= Math.Min(a.Y, b.Y) && point.Y <= Math.Max(a.Y, b.Y))
                {
                    return true;
                }

                if (b.Y == a.Y && point.Y == a.Y && point.X >= Math.Min(a.X, b.X) && point.X <= Math.Max(a.X, b.X))
                {
                    return true;
                }

                if ((b.Y < point.Y && a.Y >= point.Y) || (a.Y < point.Y && b.Y >= point.Y))
                {
                    var px = (int)(b.X + 1.0 * (point.Y - b.Y) / (a.Y - b.Y) * (a.X - b.X));
                    intersects.Add(px);
                }

                a = b;
            }

            intersects.Sort();
            return intersects.IndexOf(point.X) % 2 == 0 || intersects.Count(x => x < point.X) % 2 == 1;
        }
    }
}
