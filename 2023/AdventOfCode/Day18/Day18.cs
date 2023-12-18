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
            var dugSpaces = new List<(int X, int Y)> { (0, 0) };
            var currentX = 0;
            var currentY = 0;

            foreach (var (direction, length) in digInstructions)
            {
                var offsetX = 0;
                var offsetY = 0;

                switch (direction)
                {
                    case "U":
                        offsetY--;
                        break;
                    case "D":
                        offsetY++;
                        break;
                    case "L":
                        offsetX--;
                        break;
                    case "R":
                        offsetX++;
                        break;
                }

                for (var i = 0; i < length; i++)
                {
                    currentX += offsetX;
                    currentY += offsetY;
                    dugSpaces.Add((currentX, currentY));
                }
            }

            // Move dug spaces coordinates into map (+ translate coordinates as digging can have happened at negative X and Y)
            var minX = dugSpaces.Min(space => space.X);
            var maxX = dugSpaces.Max(space => space.X);
            var minY = dugSpaces.Min(space => space.Y);
            var maxY = dugSpaces.Max(space => space.Y);
            var dugMap = new bool[maxX - minX + 1,maxY - minY + 1];
            foreach (var (x, y) in dugSpaces)
            {
                dugMap[x - minX, y - minY] = true;
            }

            var lengthX = dugMap.GetLength(0);
            var lengthY = dugMap.GetLength(1);

            var queue = new Queue<(int X, int Y)>();
            queue.Enqueue((lengthX / 2, lengthY / 2));
            
            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                if (x < 0 || x >= lengthX || y < 0 || y >= lengthY)
                {
                    continue;
                }

                if (dugMap[x, y])
                {
                    continue;
                }

                dugMap[x, y] = true;

                queue.Enqueue((x - 1, y));
                queue.Enqueue((x + 1, y));
                queue.Enqueue((x, y - 1));
                queue.Enqueue((x, y + 1));
            }
            
            for (var y = 0; y < lengthY; y++)
            {
                for (var x = 0; x < lengthX; x++)
                {
                    Console.Write(dugMap[x, y] ? '#' : '.');
                }
                Console.Write("\n");
            }

            var area = 0;
            for (var x = 0; x < lengthX; x++)
            {
                for (var y = 0; y < lengthY; y++)
                {
                    if (dugMap[x, y])
                    {
                        area++;
                    }
                }
            }

            Console.WriteLine($"First sum = {area}");
        }
    }
}
