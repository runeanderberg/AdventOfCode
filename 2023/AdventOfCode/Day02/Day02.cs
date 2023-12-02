var lines = File.ReadLines("input.txt").ToArray();

var colourTable = new Dictionary<string, int>
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 }
};

var firstSum = lines
    .Where(line => line[(line.IndexOf(':') + 1)..]
        .Split(';')
        .All(set => set.Split(',')
            .Select(colour => colour.Trim())
            .All(colour => int.Parse(colour[..colour.IndexOf(' ')]) <=
                           colourTable[colour[(colour.IndexOf(' ') + 1)..]])))
    .Select(line => int.Parse(line[5..line.IndexOf(':')]))
    .Sum();

var secondSum = lines
    .Select(line => line[(line.IndexOf(':') + 1)..]
        .Split(';')
        .SelectMany(set => set.Split(',')
            .Select(colour => colour.Trim())
            .Select(colour => (Name: colour[(colour.IndexOf(' ') + 1)..],
                Count: int.Parse(colour[..colour.IndexOf(' ')])))
        )
        .GroupBy(colour => colour.Name)
        .Select(group => group.MaxBy(colour => colour.Count))
        .Aggregate(1, (i, colour) => i * colour.Count))
    .Sum();

Console.WriteLine($"First sum = {firstSum}, Second sum = {secondSum}");