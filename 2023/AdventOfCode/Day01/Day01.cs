using System.Text;

var lines = File.ReadLines("input.txt");

var firstSum = lines
    .Select(l => l.Where(c => c is >= '0' and <= '9').ToArray())
    .Select(ca => int.Parse(ca[0].ToString() + ca[^1]))
    .Sum();

var replacementTableList = new Dictionary<string, string>
{
    { "one", "o1e" },
    { "two", "t2o" },
    { "three", "t3e" },
    { "four", "f4r" },
    { "five", "f5e" },
    { "six", "s6x" },
    { "seven", "s7n" },
    { "eight", "e8t" },
    { "nine", "n9e" }
}.ToList();

var secondSum = lines
    .Select(l =>
    {
        var sb = new StringBuilder(l);
        replacementTableList.ForEach(r => sb.Replace(r.Key, r.Value));
        return sb.ToString();
    })
    .Select(l => l.Where(c => c is >= '0' and <= '9').ToArray())
    .Select(ca => int.Parse(ca[0].ToString() + ca[^1]))
    .Sum();

Console.WriteLine($"First sum = {firstSum}, Second sum = {secondSum}");