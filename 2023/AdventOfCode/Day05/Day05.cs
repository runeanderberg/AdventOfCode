namespace Day05
{
    internal class Day05
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var seeds = lines[0].Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse).ToArray();

            var mapsLines = lines[1..];
            var indexes = mapsLines.Select((line, index) => (line, index)).Where(x => string.IsNullOrEmpty(x.line))
                .Select(x => x.index).ToList();
            indexes.Add(mapsLines.Length);

            var mapInputs = new List<string[]>();

            for (var i = 0; i < indexes.Count - 1; i++)
            {
                mapInputs.Add(mapsLines[(indexes[i] + 1)..indexes[i + 1]]);
            }

            var maps = mapInputs.Select(mapInput => new List<MapEntry>(mapInput.Skip(1)
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray())
                .Select(numbers => new MapEntry
                    { DestinationStart = numbers[0], SourceStart = numbers[1], RangeLength = numbers[2] }))).ToList();

            foreach (var map in maps)
            {
                for (var i = 0; i < seeds.Length; i++)
                {
                    var result = map.FirstOrDefault(entry =>
                        seeds[i] >= entry.SourceStart && seeds[i] < entry.SourceStart + entry.RangeLength);

                    if (result is not null)
                    {
                        seeds[i] = result.DestinationStart + (seeds[i] - result.SourceStart);
                    }
                }
            }

            var seedGroups = lines[0].Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse).ToArray().Chunk(2).ToArray();

            var lowest = long.MaxValue;

            foreach (var seedGroup in seedGroups)
            {
                Console.WriteLine($"Starting seedGroup {seedGroup[0]},{seedGroup[1]}");
                for (var i = seedGroup[0]; i < seedGroup[0] + seedGroup[1]; i++)
                {
                    var value = i;
                    foreach (var map in maps)
                    {
                        var result = map.FirstOrDefault(entry =>
                            value >= entry.SourceStart && value < entry.SourceStart + entry.RangeLength);

                        if (result is not null)
                        {
                            value = result.DestinationStart + (value - result.SourceStart);
                        }
                    }

                    if (value < lowest)
                        lowest = value;
                }
            }

            Console.WriteLine($"Lowest location numbers (part 1, part 2) = {seeds.Min()}, {lowest}");
        }
    }

    class MapEntry
    {
        public long DestinationStart { get; set; }
        public long SourceStart { get; set; }
        public long RangeLength { get; set; }
    }
}