var lines = File.ReadLines("input.txt").ToArray();

var times = lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
    .Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
var records = lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
    .Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

var firstSum = 1;

for (var i = 0; i < times.Length; i++)
{
    var count = 0;
    // Test distance of all times, note how many higher than record
    for (var t = 0; t < times[i]; t++)
    {
        var distance = (times[i] - t) * t;
        if (distance > records[i])
            count++;
    }

    firstSum *= count;
}

var time = long.Parse(lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", ""));
var record = long.Parse(lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", ""));

var secondSum = 0;
// Test distance of all times, note how many higher than record
for (var t = 0; t < time; t++)
{
    var distance = (time - t) * t;
    if (distance > record)
        secondSum++;
}

Console.WriteLine($"First sum = {firstSum}, second sum = {secondSum}");