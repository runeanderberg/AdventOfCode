using Helpers;

namespace Day08
{
    internal class Day08
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var pattern = lines[0].ToCharArray();

            var instructions = new Dictionary<string, (string Left, string Right)>();

            lines[2..].Select(line => line.Split("=", StringSplitOptions.TrimEntries)).ToList().ForEach(
                input =>
                {
                    var directions = input[1].Split(',', StringSplitOptions.TrimEntries)
                        .Select(direction => direction.Replace("(", "").Replace(")", "")).ToArray();
                    instructions.Add(input[0], (directions[0], directions[1]));
                });

            var current = "AAA";
            var instruction = instructions[current];
            var steps = 0;
            var finished = false;
            while (!finished)
            {
                foreach (var c in pattern)
                {
                    if (current == "ZZZ")
                    {
                        finished = true;
                        break;
                    }

                    current = c == 'L' ? instruction.Left : instruction.Right;
                    instruction = c == 'L' ? instructions[instruction.Left] : instructions[instruction.Right];
                    steps++;
                }
            }

            var currents = instructions.Where(pair => pair.Key[^1] == 'A').Select(pair => pair.Key).ToArray();
            var individualSteps = new long[currents.Length];

            for (var i = 0; i < currents.Length; i++)
            {
                current = currents[i];
                instruction = instructions[current];
                finished = false;
                while (!finished)
                {
                    foreach (var c in pattern)
                    {
                        if (current[^1] == 'Z')
                        {
                            finished = true;
                            break;
                        }

                        current = c == 'L' ? instruction.Left : instruction.Right;
                        instruction = c == 'L' ? instructions[instruction.Left] : instructions[instruction.Right];

                        individualSteps[i]++;
                    }
                }
            }

            Console.WriteLine($"Steps needed (part 1, part 2) = {steps}, {Maths.LCM(individualSteps)}");
        }
    }
}