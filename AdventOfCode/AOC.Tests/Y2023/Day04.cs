namespace AOC.Tests.Y2023;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day04
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 13)]
    [TestCase(null, 17782)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        int totalPoints = 0;

        foreach (string line in lines)
        {
            // Given this string "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53" split it into two lists of numbers divided by |,
            // ignoring everything before the :

            // Extract substring after colon
            string numbersPart = line.Substring(line.IndexOf(':') + 1).Trim();

            // Split the string into two parts using '|'
            string[] parts = numbersPart.Split('|');

            // Convert each part to a list of integers
            List<int> myNumbers = parts[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList();
            List<int> winningNumbers = parts[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList();

            // Get all numbers in myNumbers that appear in the winningNumbers
            List<int> matchingNumbers = myNumbers.Intersect(winningNumbers).ToList();

            Console.WriteLine($"Matches: {matchingNumbers.Count}");

            if (matchingNumbers.Count > 0)
            {
                // 1 point for the first match, then doubled for each of the other matches after the first
                int points = 1; // Start with 1 point for the first match
                for (int i = 1; i < matchingNumbers.Count; i++)
                {
                    points *= 2; // Double the points for each subsequent match
                }

                Console.WriteLine($"Points: {points}");

                totalPoints += points;
            }
        }

        int result = totalPoints;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    private record Card
    {
        public int Id { get; set; }
        public int Matches { get; set; }
        public int Copies { get; set; }
    }

    [TestCase(@"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 30)]
    [TestCase(null, 8477787)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        int totalCards = 0;

        List<Card> cards = new();

        // Get the data for all the original cards
        foreach (string line in lines)
        {
            // Given this string "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53" split it into two lists of numbers divided by |,
            // ignoring everything before the :

            // Get the card number after "Card " and before ":"
            int cardNumber =
                int.Parse(line.Substring(line.IndexOf(' ') + 1, line.IndexOf(':') - line.IndexOf(' ') - 1));

            // Extract substring after colon
            string numbersPart = line.Substring(line.IndexOf(':') + 1).Trim();

            // Split the string into two parts using '|'
            string[] parts = numbersPart.Split('|');

            // Convert each part to a list of integers
            List<int> myNumbers = parts[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList();
            List<int> winningNumbers = parts[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToList();

            // Get all numbers in myNumbers that appear in the winningNumbers
            List<int> matchingNumbers = myNumbers.Intersect(winningNumbers).ToList();

            Card card = new() { Id = cardNumber, Matches = matchingNumbers.Count, Copies = 1 };

            cards.Add(card);
        }

        // Work out any copies - you win 1 copy of each of the next 4 cards for each matching number
        foreach (Card card in cards)
        {
            if (card.Matches > 0)
            {
                // Handle all copies, including the original
                for (int i = 0; i < card.Copies; i++)
                {
                    for (int j = 1; j <= card.Matches; j++)
                    {
                        // Increment the number of copies for the relevant card
                        Card cardToCopy = cards.First(c => c.Id == card.Id + j);
                        cardToCopy.Copies += 1;
                    }
                }
            }
        }

        totalCards = cards.Sum(c => c.Copies);

        int result = totalCards;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
