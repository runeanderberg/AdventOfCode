namespace Day06;
internal class Day06
{
    public static void Main(string[] args)
    {
        var lines = File.ReadLines("input.txt").ToArray();

        var times = lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var records = lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        var firstSum = 1L;

        for (var i = 0; i < times.Length; i++)
        {
            firstSum *= CountRecordBeaters(times[i], records[i]);
        }

        var time = long.Parse(lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", ""));
        var record = long.Parse(lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", ""));

        var secondSum = CountRecordBeaters(time, record);

        Console.WriteLine($"First sum = {firstSum}, second sum = {secondSum}");
    }

    private static long CountRecordBeaters(double time, long record)
    {
        var rootTerm = Math.Sqrt(Math.Pow(time * 0.5, 2) - record);
        return (long) (Math.Ceiling(time * 0.5 + rootTerm) - Math.Floor(time * 0.5 - rootTerm) - 1);
    }
}