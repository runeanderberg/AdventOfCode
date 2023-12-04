var lines = File.ReadLines("input.txt").ToArray();

var cards = new int[lines.Length];

for (var i = 0; i < lines.Length; i++)
{
    var numbers = lines[i].Split(':')[1].Split('|');
    
    var winningNumbers = numbers[0].Split(' ').Where(s => s != "").Select(int.Parse);
    var otherNumbers = numbers[1].Split(' ').Where(s => s != "").Select(int.Parse);

    var numMatches = otherNumbers.Count(number => winningNumbers.Any(n => n == number));

    cards[i] = numMatches;
}

var firstSum = cards.Where(card => card > 0).Sum(card => (int) Math.Pow(2, card - 1));

var ownedCards = new int[lines.Length];
Array.Fill(ownedCards, 1);

for (var i = 0; i < ownedCards.Length; i++)
{
    for (var j = 1; j <= cards[i] && i + j < ownedCards.Length; j++)
    {
        ownedCards[i + j] += ownedCards[i];
    }
}

var secondSum = ownedCards.Sum(card => card);

Console.WriteLine($"First sum = {firstSum}, Second sum = {secondSum}");