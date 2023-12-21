namespace Day15
{
    internal class Day15
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");

            var steps = input.Split(',');
            var firstSum = steps.Sum(Hash);


            var boxes = new List<(string Label, char FocalLength)>[256];
            for (var i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new();
            }

            foreach (var step in steps)
            {
                var label = step[^1] == '-' ? step[..^1] : step[..^2];

                var boxNumber = Hash(label);

                if (step[^2] == '=')
                {
                    var focalLength = step[^1];

                    var index = boxes[boxNumber].FindIndex(item => item.Label == label);

                    if (index != -1)
                    {
                        boxes[boxNumber][index] = (label, focalLength);
                    }
                    else
                    {
                        boxes[boxNumber].Add((label, focalLength));
                    }
                }
                else if (step[^1] == '-')
                {
                    boxes[boxNumber].RemoveAll(item => item.Label == label);
                }
            }

            var secondSum = 0;

            for (var i = 0; i < boxes.Length; i++)
            {
                for (var j = 0; j < boxes[i].Count; j++)
                {
                    secondSum += (i + 1) * (j + 1) * (boxes[i][j].FocalLength - '0');
                }
            }

            Console.WriteLine($"First sum = {firstSum}, second sum = {secondSum}");
        }

        private static int Hash(string input)
        {
            var chars = input.ToCharArray();

            var hash = 0;
            foreach (var c in chars)
            {
                hash += c;
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }
    }
}