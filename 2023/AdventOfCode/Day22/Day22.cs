﻿namespace Day22
{
    internal class Day22
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").ToArray();

            var bricks = new List<Brick>();

            foreach (var line in lines)
            {
                var input = line.Split('~');

                var start = input[0].Split(',').Select(int.Parse).ToArray();
                var end = input[1].Split(',').Select(int.Parse).ToArray();

                bricks.Add(new Brick(new Position(start[0], start[1], start[2]), new Position(end[0], end[1], end[2])));
            }

            // Move bricks to as low of a Z-value as possible
            foreach (var brick in bricks.OrderBy(b => b.End.Z))
            {
                // First, check for lowest (end) Z in bricks that overlaps in X and Y
                var lowestZ = bricks.Where(b => b.End.Z < brick.Start.Z && b.HasXYOverlap(brick))
                    .DefaultIfEmpty().Max(b => b?.End.Z);

                // If no overlap, move brick down to Z = 1
                if (lowestZ is null)
                {
                    brick.Start.Z = 1;
                    continue;
                }

                // Else, check how far down to move the brick
                var offset = brick.Start.Z - lowestZ.Value - 1;

                if (offset == 0)
                    continue;

                brick.Start.Z -= offset;
                brick.End.Z -= offset;
            }

            // Map out which bricks rest on top of which bricks
            var connections =
                bricks.Select(brick => (Bottom: brick, OnTop: bricks.Where(b => b.RestsOn(brick)).ToList())).ToList();

            // Check which bricks can be removed
            var canBeRemoved = 0;
            foreach (var (bottom, onTop) in connections)
            {
                // If no bricks resting on it, can be removed
                if (onTop.Count == 0)
                {
                    canBeRemoved++;
                    continue;
                }

                // If bricks resting on top of the bottom brick all rests on another brick, the bottom brick can be removed
                var removable = onTop.All(brick =>
                    connections.Where(connection => connection.Bottom != bottom)
                        .Any(connection => connection.OnTop.Contains(brick)));

                if (removable)
                    canBeRemoved++;
            }

            Console.WriteLine($"First sum = {canBeRemoved}");
        }
    }

    internal record Brick(Position Start, Position End)
    {
        public bool HasXYOverlap(Brick other)
        {
            return Start.X <= other.End.X && other.Start.X <= End.X && Start.Y <= other.End.Y && other.Start.Y <= End.Y;
        }

        public bool RestsOn(Brick other)
        {
            return HasXYOverlap(other) && other.End.Z == Start.Z - 1;
        }
    }

    internal class Position(int x, int y, int z)
    {
        public int X { get; init; } = x;
        public int Y { get; init; } = y;
        public int Z { get; set; } = z;
    }
}