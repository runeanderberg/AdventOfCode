using Helpers;
using System.ComponentModel;

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
            var edgeDict = new Dictionary<Edge, int>();

            foreach (var component in componentMap)
            {
                var queue = new Queue<(Component Component, HashSet<Edge> Previous)>();
                queue.Enqueue((component.Value, new HashSet<Edge>()));

                var visited = new HashSet<Component>();

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    if (visited.Contains(current.Component))
                    {
                        foreach (var previous in current.Previous)
                        {
                            edgeDict.AddOrIncrement(previous, 1);
                        }
                        continue;
                    }

                    visited.Add(current.Component);

                    foreach (var edge in current.Component.Connections)
                    {
                        queue.Enqueue((edge.Other(current.Component), new HashSet<Edge>(current.Previous) { edge }));
                    }
                }
            }

            // Remove most visited edges
            edgeDict.OrderByDescending(pair => pair.Value).Take(3).ToList().ForEach(pair => pair.Key.Destroy());

            int visitedCount;
            // Flood fill once from any node, reachable nodes is one cluster, the rest is the other
            {
                var queue = new Queue<(Component Component, HashSet<Edge> Previous)>();
                queue.Enqueue((componentMap.First().Value, new HashSet<Edge>()));

                var visited = new HashSet<Component>();

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    if (visited.Contains(current.Component))
                    {
                        continue;
                    }

                    visited.Add(current.Component);

                    foreach (var edge in current.Component.Connections)
                    {
                        queue.Enqueue((edge.Other(current.Component), new HashSet<Edge>(current.Previous) { edge }));
                    }
                }

                visitedCount = visited.Count;
            }

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
