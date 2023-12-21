using Helpers;

namespace Day10
{
    internal class Day10
    {
        static void Main(string[] args)
        {
            var map = File.ReadLines("input.txt").To2DArray(c => c).Transpose();

            (int Longitude, int Latitude) startCoordinates = map.IndexOf('S');

            var longitudeLength = map.GetLength(0);
            var latitudeLength = map.GetLength(1);

            var pathMap = new char[longitudeLength, latitudeLength];
            var currentLongitude = startCoordinates.Longitude;
            var currentLatitude = startCoordinates.Latitude;
            var previousLongitude = startCoordinates.Longitude;
            var previousLatitude = startCoordinates.Latitude;
            var steps = 0;

            // First, replace S with correct pipe character
            var canConnectNorth = CanConnectNorth(currentLongitude, currentLatitude, map);
            var canConnectSouth = CanConnectSouth(currentLongitude, currentLatitude, map);
            var canConnectWest = CanConnectWest(currentLongitude, currentLatitude, map);
            var canConnectEast = CanConnectEast(currentLongitude, currentLatitude, map);

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

            // Then move in one of the possible directions
            if (canConnectNorth)
                currentLatitude--;
            else if (canConnectEast)
                currentLongitude++;
            else if (canConnectSouth)
                currentLatitude++;
            else if (canConnectWest)
                currentLongitude--;
            steps++;

            while (true)
            {
                if ((currentLongitude, currentLatitude) == startCoordinates)
                    break;

                var (longitudeDifference, latitudeDifference) = GetNextDiff(map[currentLongitude, currentLatitude],
                    (previousLongitude - currentLongitude, previousLatitude - currentLatitude));

                previousLongitude = currentLongitude;
                previousLatitude = currentLatitude;

                currentLongitude += longitudeDifference;
                currentLatitude += latitudeDifference;

                pathMap[previousLongitude, previousLatitude] = map[previousLongitude, previousLatitude];

                steps++;
            }

            // Generate map marking inside and outside
            var insideOutsideMap = new char[longitudeLength, latitudeLength];

            for (var latitude = 0; latitude < latitudeLength; latitude++)
            {
                for (var longitude = 0; longitude < longitudeLength; longitude++)
                {
                    if (pathMap[longitude, latitude] != 0)
                        continue;

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
                    var c = pathMap[longitude, latitude] != 0
                        ? pathMap[longitude, latitude]
                        : insideOutsideMap[longitude, latitude];

                    if (c == 'S' || map[longitude, latitude] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (pathMap[longitude, latitude] != 0)
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

        private static bool CanConnectNorth(int longitude, int latitude, char[,] map)
        {
            if (latitude - 1 < 0)
                return false;

            // Pipe north of self has to be |, 7, F, or S
            var c = map[longitude, latitude - 1];
            return c is '|' or '7' or 'F';
        }

        private static bool CanConnectSouth(int longitude, int latitude, char[,] map)
        {
            if (latitude + 1 == map.GetLength(1))
                return false;

            // Pipe south of self has to be |, L, J, or S
            var c = map[longitude, latitude + 1];
            return c is '|' or 'L' or 'J';
        }

        private static bool CanConnectWest(int longitude, int latitude, char[,] map)
        {
            if (longitude - 1 < 0)
                return false;

            // Pipe north of self has to be -, L, F, or S
            var c = map[longitude - 1, latitude];
            return c is '-' or 'L' or 'F';
        }

        private static bool CanConnectEast(int longitude, int latitude, char[,] map)
        {
            if (longitude + 1 == map.GetLength(0))
                return false;

            // Pipe east of self has to be -, J, 7, or S
            var c = map[longitude + 1, latitude];
            return c is '-' or 'J' or '7';
        }

        private static (int LongitudeDifference, int LatitudeDifference) GetNextDiff(char current,
            (int LongitudeDifference, int LatitudeDifference) previous)
        {
            return current switch
            {
                'F' => previous == (0, 1) ? (1, 0) : (0, 1),
                '7' => previous == (0, 1) ? (-1, 0) : (0, 1),
                'L' => previous == (0, -1) ? (1, 0) : (0, -1),
                'J' => previous == (0, -1) ? (-1, 0) : (0, -1),
                '|' => previous == (0, 1) ? (0, -1) : (0, 1),
                '-' => previous == (1, 0) ? (-1, 0) : (1, 0),
                _ => (0, 0)
            };
        }
    }
}