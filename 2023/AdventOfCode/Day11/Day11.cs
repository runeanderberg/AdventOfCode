namespace Day11
{
    internal class Day11
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("input.txt").ToList();

            // Get pre-expansion coordinates of each galaxy
            var preExpansionGalaxies = new List<Galaxy>();
            var id = 0;

            for (var i = 0; i < lines.Count; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] != '#')
                        continue;

                    preExpansionGalaxies.Add(new Galaxy(id, i, j));
                    id++;
                }
            }

            var smallExpansionGalaxies = preExpansionGalaxies.Select(galaxy => galaxy.Clone()).ToList();
            var firstSum = ExpandAndSumDistances(smallExpansionGalaxies, 2);

            var bigExpansionGalaxies = preExpansionGalaxies.Select(galaxy => galaxy.Clone()).ToList();
            var secondSum = ExpandAndSumDistances(bigExpansionGalaxies, 1000000);

            Console.WriteLine($"First sum = {firstSum}, second sum = {secondSum}");
        }

        private static long ExpandAndSumDistances(IReadOnlyCollection<Galaxy> galaxies, int expansionFactor)
        {
            var expansionAddition = expansionFactor - 1;

            // iterate over each row, if no galaxies on row, increment all galaxies' rows after it by expansion addition
            var max = galaxies.Max(galaxy => galaxy.Row);
            for (var row = 0; row < max; row++)
            {
                if (galaxies.Any(galaxy => galaxy.Row == row))
                    continue;

                foreach (var galaxy in galaxies.Where(galaxy => galaxy.Row > row))
                {
                    galaxy.Row += expansionAddition;
                }

                row += expansionAddition;
                max += expansionAddition;
            }

            // iterate over each col, if no galaxies on col, increment all galaxies' cols after it by expansion addition
            max = galaxies.Max(galaxy => galaxy.Col);
            for (var col = 0; col < max; col++)
            {
                if (galaxies.Any(galaxy => galaxy.Col == col))
                    continue;

                foreach (var galaxy in galaxies.Where(galaxy => galaxy.Col > col))
                {
                    galaxy.Col += expansionAddition;
                }

                col += expansionAddition;
                max += expansionAddition;
            }


            var combinations = galaxies.SelectMany(x => galaxies, Tuple.Create)
                .Where(tuple => tuple.Item1.Id < tuple.Item2.Id).ToList();

            return combinations.Sum(pair =>
                Math.Abs(pair.Item1.Row - pair.Item2.Row) + Math.Abs(pair.Item1.Col - pair.Item2.Col));
        }
    }

    internal class Galaxy(int id, long row, long col)
    {
        public int Id { get; set; } = id;
        public long Row { get; set; } = row;
        public long Col { get; set; } = col;

        public Galaxy Clone()
        {
            return new Galaxy(Id, Row, Col);
        }
    }
}