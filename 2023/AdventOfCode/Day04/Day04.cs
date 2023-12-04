var lines = File.ReadLines("input.txt").ToArray();

var cards = new Dictionary<int, int>();
            
foreach (var line in lines)
{
    var input = line.Split(':');
                
    var id = int.Parse(input[0].Split(' ')[^1]);
    var numbers = input[1].Split('|');

    var winningNumbers = numbers[0].Split(' ').Where(s => s != "").Select(int.Parse);
    var otherNumbers = numbers[1].Split(' ').Where(s => s != "").Select(int.Parse);

    var numMatches = otherNumbers.Count(number => winningNumbers.Any(n => n == number));

    cards.Add(id, numMatches);
}

var firstSum = cards.Where(card => card.Value > 0).Sum(card => (int) Math.Pow(2, card.Value - 1));

var ownedCards = new Dictionary<int, int>();
cards.ToList().ForEach(card => ownedCards.Add(card.Key, 1));

for (var i = 1; i <= ownedCards.Count; i++)
{
    for (var j = 1; j <= cards[i] && i + j <= ownedCards.Count; j++)
    {
        ownedCards[i + j] += ownedCards[i];
    }
}

var secondSum = ownedCards.Sum(card => card.Value);

Console.WriteLine($"First sum = {firstSum}, Second sum = {secondSum}");