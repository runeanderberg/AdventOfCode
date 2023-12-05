namespace Day05
{
    internal class Day05
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var values = lines[0].Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
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
                    .Select(numbers =>
                        new MapEntry(new Interval(numbers[1], numbers[1] + numbers[2] - 1), numbers[0] - numbers[1]))))
                .ToList();

            foreach (var map in maps)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    var result = map.FirstOrDefault(entry => entry.SourceInterval.Contains(values[i]));

                    if (result is not null)
                    {
                        values[i] += result.Offset;
                    }
                }
            }

            var intervals = lines[0].Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse).ToArray().Chunk(2).ToArray()
                .Select(group => new Interval(group[0], group[0] + group[1] - 1)).ToList();

            foreach (var map in maps)
            {
                var newIntervals = new List<Interval>();
                foreach (var interval in intervals)
                {
                    // Check if there is a map entry that overlaps with the interval
                    var match = map.FirstOrDefault(entry => entry.SourceInterval.HasOverlap(interval));

                    // If there isn't, the interval is unchanged
                    if (match is null)
                    {
                        newIntervals.Add(interval);
                        continue;
                    }

                    // Else, keep non-overlapping bits unchanged but transform the overlapping part to new values
                    var overlap = match.SourceInterval.GetOverlap(interval);
                    var unchanged = interval.GetNonOverlap(overlap);
                    newIntervals.AddRange(unchanged);

                    overlap.MoveBy(match.Offset);
                    newIntervals.Add(overlap);
                }

                var lowest = newIntervals.Min(interval => interval.Start);

                intervals.Clear();
                intervals.AddRange(newIntervals);
            }

            Console.WriteLine(
                $"Lowest location numbers (part 1, part 2) = {values.Min()}, {intervals.Min(interval => interval.Start)}");
        }
    }


    internal class MapEntry(Interval sourceInterval, long offset)
    {
        public Interval SourceInterval { get; set; } = sourceInterval;
        public long Offset { get; set; } = offset;
    }

    internal class Interval(long start, long end)
    {
        public long Start { get; set; } = start;
        public long End { get; set; } = end;

        public void MoveBy(long offset)
        {
            Start += offset;
            End += offset;
        }

        public bool Contains(long number)
        {
            return number >= Start && number <= End;
        }

        public bool HasOverlap(Interval other)
        {
            return Start < other.End && other.Start < End;
        }

        public Interval GetOverlap(Interval other)
        {
            return new Interval(long.Max(Start, other.Start), long.Min(End, other.End));
        }

        public List<Interval> GetNonOverlap(Interval overlap)
        {
            var intervals = new List<Interval>();

            if (Start < overlap.Start)
            {
                intervals.Add(new Interval(Start, overlap.Start - 1));
            }

            if (End > overlap.End)
            {
                intervals.Add(new Interval(overlap.End + 1, End));
            }

            return intervals;
        }
    }
}