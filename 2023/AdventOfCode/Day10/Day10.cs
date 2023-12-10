namespace Day10
{
    internal class Day10
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var map = new char[lines[0].Length, lines.Length];
            (int longitude, int latitude) startCoordinates = (0, 0);

            for (var latitude = 0; latitude < lines.Length; latitude++)
            {
                for (var longitude = 0; longitude < lines[0].Length; longitude++)
                {
                    map[longitude, latitude] = lines[latitude][longitude];

                    if (map[longitude, latitude] == 'S')
                    {
                        startCoordinates = (longitude, latitude);
                    }
                }
            }

            var finished = false;
            var currentLongitude = startCoordinates.longitude;
            var currentLatitude = startCoordinates.latitude;

            var visitedMap = new bool[map.GetLength(0), map.GetLength(1)];

            var visitedStart = false;
            var steps = 0;
            while (!finished)
            {
                var previousLongitude = currentLongitude;
                var previousLatitude = currentLatitude;

                switch (map[currentLongitude, currentLatitude])
                {
                    case '|':
                        if (CanConnectNorth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude--;
                        else if (CanConnectSouth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude++;
                        break;
                    case '-':
                        if (CanConnectWest(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude--;
                        else if (CanConnectEast(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude++;
                        break;
                    case 'L':
                        if (CanConnectNorth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude--;
                        else if (CanConnectEast(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude++;
                        break;
                    case 'J':
                        if (CanConnectNorth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude--;
                        else if (CanConnectWest(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude--;
                        break;
                    case '7':
                        if (CanConnectSouth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude++;
                        else if (CanConnectWest(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude--;
                        break;
                    case 'F':
                        if (CanConnectSouth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude++;
                        else if (CanConnectEast(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude++;
                        break;
                    case '.':
                        break;
                    case 'S':
                        if (visitedStart)
                        {
                            finished = true;
                            break;
                        }

                        if (CanConnectNorth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude--;
                        else if (CanConnectEast(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude++;
                        else if (CanConnectSouth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude++;
                        else if (CanConnectWest(currentLongitude, currentLatitude, map, visitedMap)) 
                            currentLongitude--;

                        visitedStart = true;
                        break;
                }

                if (map[previousLongitude, previousLatitude] != 'S')
                    visitedMap[previousLongitude, previousLatitude] = true;
                steps++;
            }
            
            for (var latitude = 0; latitude < map.GetLength(1); latitude++)
            {
                for (var longitude = 0; longitude < map.GetLength(0); longitude++)
                {
                    var c = map[longitude, latitude];

                    if (c == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else if (visitedMap[longitude, latitude])
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    
                    Console.Write(c);
                }
                Console.Write('\n');
            }

            Console.Write($"Loop length / 2 = {steps / 2}");
        }

        private static bool CanConnectNorth(int longitude, int latitude, char[,] map, bool[,] visited)
        {
            if (latitude - 1 < 0)
                return false;

            if (visited[longitude, latitude - 1])
                return false;

            // Pipe north of self has to be |, 7, F, or S
            var c = map[longitude, latitude - 1];
            return c is '|' or '7' or 'F' or 'S';   
        }

        private static bool CanConnectSouth(int longitude, int latitude, char[,] map, bool[,] visited)
        {
            if (latitude + 1 == map.GetLength(1))
                return false;

            if (visited[longitude, latitude + 1])
                return false;

            // Pipe south of self has to be |, L, J, or S
            var c = map[longitude, latitude + 1];
            return c is '|' or 'L' or 'J' or 'S';
        }

        private static bool CanConnectWest(int longitude, int latitude, char[,] map, bool[,] visited)
        {
            if (longitude - 1 < 0)
                return false;

            if (visited[longitude -1, latitude])
                return false;

            // Pipe north of self has to be -, L, F, or S
            var c = map[longitude - 1, latitude];
            return c is '-' or 'L' or 'F' or 'S';
        }

        private static bool CanConnectEast(int longitude, int latitude, char[,] map, bool[,] visited)
        {
            if (longitude + 1 == map.GetLength(0))
                return false;

            if (visited[longitude + 1, latitude])
                return false;

            // Pipe east of self has to be -, J, 7, or S
            var c = map[longitude + 1, latitude];
            return c is '-' or 'J' or '7' or 'S';
        }
    }
}
