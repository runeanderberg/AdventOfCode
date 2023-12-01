using System.Text;

var lines = File.ReadLines("input.txt").ToArray();

var firstSum = lines
    .Select(line => line.Where(char.IsDigit).ToArray())
    .Select(chars => int.Parse($"{chars[0]}{chars[^1]}"))
    .Sum();

var replacements = new List<(string OldChars, string NewChars)>
{
    ("one", "o1e"),
    ("two", "t2o"),
    ("three", "t3e"),
    ("four", "f4r"),
    ("five", "f5e"),
    ("six", "s6x"),
    ("seven", "s7n"),
    ("eight", "e8t"),
    ("nine", "n9e")
};

var secondSum = lines
    .Select(line =>
    {
        var stringBuilder = new StringBuilder(line);
        replacements.ForEach(replacement => stringBuilder.Replace(replacement.OldChars, replacement.NewChars));
        return stringBuilder.ToString();
    })
    .Select(line => line.Where(char.IsDigit).ToArray())
    .Select(chars => int.Parse($"{chars[0]}{chars[^1]}"))
    .Sum();

Console.WriteLine($"First sum = {firstSum}, Second sum = {secondSum}");