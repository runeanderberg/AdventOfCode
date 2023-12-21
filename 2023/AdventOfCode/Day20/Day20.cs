namespace Day20
{
    internal class Day20
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToArray();

            var modules = new Dictionary<string, Module>();

            foreach (var line in lines)
            {
                var input = line.Split("->", StringSplitOptions.TrimEntries);
                var type = input[0][0];
                var name = type != 'b' ? input[0][1..] : input[0];
                var receivers = input[^1].Split(',', StringSplitOptions.TrimEntries);

                Module module = type switch
                {
                    '%' => new FlipFlop(name, receivers),
                    '&' => new Conjunction(name, receivers),
                    'b' => new Broadcaster(name, receivers),
                    _ => throw new ArgumentOutOfRangeException()
                };

                modules.Add(name, module);
            }

            foreach (var (name, module) in modules)
            {
                var result = module.Receivers
                    .Where(modules.ContainsKey)
                    .Select(receiver => modules[receiver])
                    .Where(m => m.GetType() == typeof(Conjunction))
                    .Select(receiver => (Conjunction) receiver);

                foreach (var receiver in result)
                {
                    receiver.AddSender(name);
                }
            }

            long lowPulses = 0;
            long highPulses = 0;

            for (var i = 0; i < 1000; i++)
            {
                var processQueue = new Queue<(string Sender, string Receiver, bool High)>();
                processQueue.Enqueue(("button", "broadcaster", false));

                while (processQueue.Count > 0)
                {
                    var (sender, receiver, high) = processQueue.Dequeue();

                    if (high)
                        highPulses++;
                    else
                        lowPulses++;

                    if (!modules.ContainsKey(receiver))
                        continue;

                    var module = modules[receiver];

                    foreach (var message in module.Process(sender, high))
                    {
                        processQueue.Enqueue(message);
                    }
                }
            }

            Console.WriteLine(
                $"Numbers of low and high pulses (low, high) = ({lowPulses}, {highPulses}), gives {lowPulses * highPulses}");
        }
    }

    internal abstract class Module(string name, IEnumerable<string> receivers)
    {
        public static bool DoPrint = false;

        public readonly IEnumerable<string> Receivers = receivers;
        public abstract IEnumerable<(string Sender, string Receiver, bool High)> Process(string sender, bool high);

        internal void Print(string sender, bool high)
        {
            if (!DoPrint)
                return;

            var value = high ? "high" : "low";
            Console.WriteLine($"{sender} -{value} -> {name}");
        }
    }

    internal class Broadcaster(string name, IEnumerable<string> receivers) : Module(name, receivers)
    {
        public override IEnumerable<(string Sender, string Receiver, bool High)> Process(string sender, bool high)
        {
            Print(sender, high);

            return receivers.Select(receiver => (name, receiver, high));
        }
    }

    internal class FlipFlop(string name, IEnumerable<string> receivers) : Module(name, receivers)
    {
        private bool _on;

        public override IEnumerable<(string Sender, string Receiver, bool High)> Process(string sender, bool high)
        {
            Print(sender, high);

            if (high)
                return Array.Empty<(string Sender, string Receiver, bool High)>();

            if (!_on)
            {
                _on = true;
                return receivers.Select(receiver => (name, receiver, true));
            }

            _on = false;
            return receivers.Select(receiver => (name, receiver, false));
        }
    }

    internal class Conjunction(string name, IEnumerable<string> receivers) : Module(name, receivers)
    {
        private readonly Dictionary<string, bool> _lastPulses = new();

        public void AddSender(string name)
        {
            _lastPulses.Add(name, false);
        }

        public override IEnumerable<(string Sender, string Receiver, bool High)> Process(string sender, bool high)
        {
            Print(sender, high);

            _lastPulses[sender] = high;

            return _lastPulses.All(pair => pair.Value)
                ? receivers.Select(receiver => (name, receiver, false))
                : receivers.Select(receiver => (name, receiver, true));
        }
    }
}