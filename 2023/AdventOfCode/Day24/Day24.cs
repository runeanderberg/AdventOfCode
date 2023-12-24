using System;
using Helpers;

namespace Day24
{
    internal class Day24
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").ToArray();

            var hailstones = new List<Hailstone>();

            foreach (var line in lines)
            {
                var input = line.Split('@', StringSplitOptions.TrimEntries);

                var position = input[0].Split(',', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
                var velocity = input[1].Split(',', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

                hailstones.Add(new Hailstone(new Point3D(position[0], position[1], position[2]), new Point3D(velocity[0], velocity[1], velocity[2])));
            }

            const long minX = 200000000000000;
            const long maxX = 400000000000000;
            const long minY = 200000000000000;
            const long maxY = 400000000000000;

            var pairs = hailstones.SelectMany(x => hailstones, Tuple.Create)
                .Where(tuple => tuple.Item1.GetHashCode() < tuple.Item2.GetHashCode())
                .Select(tuple => (A: tuple.Item1, B: tuple.Item2))
                .ToList();

            var collisions = pairs.Where(pair => !pair.A.IsParallelWith(pair.B))
                .Select(pair => (Point: pair.A.GetIntersectionPointWith(pair.B), pair.A, pair.B))
                .ToList();

            var collisionsInsideArea = collisions.Where(collision =>
                collision.Point.X is >= minX and <= maxX && collision.Point.Y is >= minY and <= maxY);

            var collisionsInFuture = collisionsInsideArea.Where(collision =>
            {
                var changeA = collision.Point - collision.A.GetPosition2D();
                var factorA = changeA.X / collision.A.Velocity.X;

                var changeB = collision.Point - collision.B.GetPosition2D();
                var factorB = changeB.X / collision.B.Velocity.X;

                return factorA > 0 && factorB > 0;
            });

            Console.WriteLine($"First sum = {collisionsInFuture.Count()}");
        }
    }

    internal record Hailstone(Point3D Position, Point3D Velocity)
    {
        public Point2D GetPosition2D() => new(Position.X, Position.Y);

        public (double A, double B, double C) GetStandardForm()
        {
            var p1 = Position;
            var p2 = Position + Velocity;
            var m = (double) (p2.Y - p1.Y) / (p2.X - p1.X);
            return (-m, 1, (-m) * p1.X + p1.Y);
        }

        public bool IsParallelWith(Hailstone other)
        {
            var ours = GetStandardForm();
            var others = other.GetStandardForm();

            return ours.A * others.B - others.A * ours.B == 0;
        }

        public Point2D GetIntersectionPointWith(Hailstone other)
        {
            var ours = GetStandardForm();
            var others = other.GetStandardForm();

            var det = ours.A * others.B - others.A * ours.B;

            return new Point2D((others.B * ours.C - ours.B * others.C) / det,
                (ours.A * others.C - others.A * ours.C) / det);
        }
    }
}
