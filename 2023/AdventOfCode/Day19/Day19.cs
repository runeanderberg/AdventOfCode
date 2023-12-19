namespace Day19
{
    internal class Day19
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();
            var workflowStrings = lines.TakeWhile(line => line != "");
            var partStrings = lines.SkipWhile(line => line != "").Skip(1);


            var parts = partStrings
                .Select(partString => partString[1..^1].Split(','))
                .Select(values =>
                    new Part(int.Parse(values[0][2..]), int.Parse(values[1][2..]),
                        int.Parse(values[2][2..]), int.Parse(values[3][2..])))
                .ToList();

            var workflows = new Dictionary<string, Workflow>();

            foreach (var workflowString in workflowStrings)
            {
                var bracketIndex = workflowString.IndexOf('{');
                var name = workflowString[..bracketIndex];
                var rest = workflowString[(bracketIndex + 1)..^1];
                var ruleStrings = rest.Split(',');

                var rules = new List<Rule>();
                foreach (var ruleString in ruleStrings[..^1])
                {
                    var colonIndex = ruleString.IndexOf(':');
                    rules.Add(new Rule(ruleString[0], ruleString[1], int.Parse(ruleString[2..colonIndex]),
                        ruleString[(colonIndex + 1)..]));
                }

                rules.Add(new Rule('-', '-', 0, ruleStrings[^1]));

                workflows.Add(name, new Workflow(rules));
            }

            var accepted = new List<Part>();

            var queue = new Queue<(Part Part, string WorkflowName)>();
            parts.ForEach(part => queue.Enqueue((part, "in")));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var workflow = workflows[current.WorkflowName];

                foreach (var rule in workflow.Rules.Where(rule => rule.TestPart(current.Part)))
                {
                    if (rule.NextWorkflow == "A")
                    {
                        accepted.Add(current.Part);
                        break;
                    }

                    if (rule.NextWorkflow == "R")
                    {
                        break;
                    }

                    queue.Enqueue((current.Part, rule.NextWorkflow));
                    break;
                }
            }

            var firstSum = accepted.Sum(part => part.X + part.M + part.A + part.S);

            Console.WriteLine($"First sum = {firstSum}");
        }
    }

    internal record Part(int X, int M, int A, int S);

    internal record Rule(char Category, char Comparer, int Value, string NextWorkflow)
    {
        public bool TestPart(Part part)
        {
            if (Category == '-')
                return true;

            var valueToTest = Category switch
            {
                'x' => part.X,
                'm' => part.M,
                'a' => part.A,
                's' => part.S,
                _ => throw new ArgumentOutOfRangeException(nameof(Category), Category, "Unknown category")
            };

            return Comparer switch
            {
                '>' => valueToTest > Value,
                '<' => valueToTest < Value,
                _ => throw new ArgumentOutOfRangeException(nameof(Category), Category, "Unknown comparer")
            };
        }
    }

    internal record Workflow(List<Rule> Rules);
}