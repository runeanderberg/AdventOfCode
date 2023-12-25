using Helpers;

namespace Day25
{
    internal class Day25
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").ToArray();

            var componentMap = new Dictionary<string, Component>();

            foreach (var line in lines)
            {
                var input = line.Split(':', StringSplitOptions.TrimEntries);
                var leftName = input[0];
                var rightNames = input[1].Split(' ', StringSplitOptions.TrimEntries);

                if (!componentMap.ContainsKey(leftName))
                {
                    componentMap.Add(leftName, new Component(leftName));
                }

                foreach (var rightName in rightNames)
                {
                    if (!componentMap.ContainsKey(rightName))
                    {
                        componentMap.Add(rightName, new Component(rightName));
                    }

                    componentMap[leftName].AddConnectionTo(componentMap[rightName]);
                }
            }


            // Flood fill from each node, count number of times each edge has been passed
            var edgeCounts = new Dictionary<Edge, int>();
            var queue = new Queue<(Component Component, HashSet<Edge> Previous)>();
            var visited = new HashSet<Component>();

            foreach (var pair in componentMap)
            {
                queue.Enqueue((pair.Value, new HashSet<Edge>()));
                visited.Clear();

                while (queue.Count > 0)
                {
                    var (component, previous) = queue.Dequeue();

                    if (visited.Contains(component))
                    {
                        foreach (var edge in previous)
                        {
                            edgeCounts.AddOrIncrement(edge, 1);
                        }

                        continue;
                    }

                    visited.Add(component);

                    foreach (var edge in component.Connections)
                    {
                        queue.Enqueue((edge.Other(component), new HashSet<Edge>(previous) { edge }));
                    }
                }
            }

            // Remove 3 most visited edges
            foreach (var edge in edgeCounts.OrderByDescending(pair => pair.Value).Take(3).Select(pair => pair.Key))
            {
                edge.Destroy();
            }

            // Flood fill once from any node, reachable nodes is one cluster, the rest is the other
            queue.Enqueue((componentMap.First().Value, new HashSet<Edge>()));
            visited.Clear();

            while (queue.Count > 0)
            {
                var (component, previous) = queue.Dequeue();

                if (visited.Contains(component))
                {
                    continue;
                }

                visited.Add(component);

                foreach (var edge in component.Connections)
                {
                    queue.Enqueue((edge.Other(component), new HashSet<Edge>(previous) { edge }));
                }
            }

            var visitedCount = visited.Count;


            Console.WriteLine($"First sum = {(componentMap.Count - visitedCount) * visitedCount}");
        }
    }

    internal record Component(string Name)
    {
        public List<Edge> Connections { get; init; } = new();

        public void AddConnectionTo(Component other)
        {
            var edge = new Edge(this, other);
            Connections.Add(edge);
            other.AddEdge(edge);
        }

        private void AddEdge(Edge edge)
        {
            Connections.Add(edge);
        }
    }

    internal record Edge(Component A, Component B)
    {
        public Component Other(Component component)
        {
            return A == component ? B : A;
        }

        public void Destroy()
        {
            A.Connections.Remove(this);
            B.Connections.Remove(this);
        }
    }
}