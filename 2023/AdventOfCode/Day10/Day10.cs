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

            var longitudeLength = map.GetLength(0);
            var latitudeLength = map.GetLength(1);

            var visitedMap = new bool[longitudeLength, latitudeLength];
            var pathMap = new char[longitudeLength, latitudeLength];

            for (var latitude = 0; latitude < latitudeLength; latitude++)
            {
                for (var longitude = 0; longitude < longitudeLength; longitude++)
                {
                    pathMap[longitude, latitude] = ' ';
                }
            }

            var currentLongitude = startCoordinates.longitude;
            var currentLatitude = startCoordinates.latitude;
            var steps = 0;
            while (true)
            {
                if (visitedMap[currentLongitude, currentLatitude])
                    break;

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
                    case 'S':
                        if (CanConnectNorth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude--;
                        else if (CanConnectEast(currentLongitude, currentLatitude, map, visitedMap))
                            currentLongitude++;
                        else if (CanConnectSouth(currentLongitude, currentLatitude, map, visitedMap))
                            currentLatitude++;
                        else if (CanConnectWest(currentLongitude, currentLatitude, map, visitedMap)) 
                            currentLongitude--;
                        break;
                }

                visitedMap[previousLongitude, previousLatitude] = true;
                pathMap[previousLongitude, previousLatitude] = map[previousLongitude, previousLatitude];

                steps++;
            }

            // After done generating path map, replace S with correct path char
            currentLongitude = startCoordinates.longitude;
            currentLatitude = startCoordinates.latitude;
            var emptyVisitedMap = new bool[longitudeLength, latitudeLength];

            var canConnectNorth = CanConnectNorth(currentLongitude, currentLatitude, pathMap, emptyVisitedMap);
            var canConnectSouth = CanConnectSouth(currentLongitude, currentLatitude, pathMap, emptyVisitedMap);
            var canConnectWest = CanConnectWest(currentLongitude, currentLatitude, pathMap, emptyVisitedMap);
            var canConnectEast = CanConnectEast(currentLongitude, currentLatitude, pathMap, emptyVisitedMap);

            char replacement;
            if (canConnectNorth && canConnectSouth)
                replacement = '|';
            else if (canConnectWest && canConnectEast)
                replacement = '-';
            else if (canConnectNorth && canConnectWest)
                replacement = 'J';
            else if (canConnectNorth && canConnectEast)
                replacement = 'L';
            else if (canConnectSouth && canConnectWest)
                replacement = '7';
            else if (canConnectSouth && canConnectEast)
                replacement = 'F';
            else
                throw new Exception("Failed to find replacement character for S");

            pathMap[currentLongitude, currentLatitude] = replacement;


            // Generate map marking inside and outside
            var insideOutsideMap = new char[longitudeLength, latitudeLength];

            for (var latitude = 0; latitude < latitudeLength; latitude++)
            {
                for (var longitude = 0; longitude < longitudeLength; longitude++)
                {
                    if (pathMap[longitude, latitude] != ' ')
                    {
                        insideOutsideMap[longitude, latitude] = ' ';
                        continue;
                    }

                    var outside = true;
                    var upwards = false;
                    var i = longitude;

                    while (i < longitudeLength)
                    {
                        var c = pathMap[i, latitude];
                        i++;

                        switch (c)
                        {
                            case '|':
                                outside = !outside;
                                break;
                            case 'F':
                                outside = !outside;
                                upwards = true;
                                break;
                            case 'J' when upwards:
                                break;
                            case 'J':
                                outside = !outside;
                                break;
                            case 'L':
                                outside = !outside;
                                upwards = false;
                                break;
                            case '7' when !upwards:
                                break;
                            case '7':
                                outside = !outside;
                                break;
                        }
                    }
                    
                    insideOutsideMap[longitude, latitude] = outside ? 'O' : 'I';
                }
            }

            for (var latitude = 0; latitude < latitudeLength; latitude++)
            {
                for (var longitude = 0; longitude < longitudeLength; longitude++)
                {
                    // If square has been visited, it's part of path map, else inside/outside map
                    var c = visitedMap[longitude, latitude] ? pathMap[longitude, latitude] : insideOutsideMap[longitude, latitude];

                    if (c == 'S' || map[longitude, latitude] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (visitedMap[longitude, latitude])
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    else if (c == 'I')
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    var formatted = c switch
                    {
                        '|' => '\u2502',
                        '-' => '\u2500',
                        'F' => '\u250c',
                        '7' => '\u2510',
                        'L' => '\u2514',
                        'J' => '\u2518',
                        'I' => 'I',
                        'O' => 'O',
                        _ => ' '
                    };

                    Console.Write(formatted);
                }
                Console.Write('\n');
            }

            var insideCount = 0;

            for (var latitude = 0; latitude < latitudeLength; latitude++)
            {
                for (var longitude = 0; longitude < longitudeLength; longitude++)
                {
                    if (insideOutsideMap[longitude, latitude] == 'I')
                        insideCount++;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Furthest distance from start = {steps / 2} steps, area enclosed by loop = {insideCount}");
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
