namespace AOC.Tests.Y2023;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day07
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483", 6440)]
    [TestCase(null, 247815719)] // The actual answer
    public void Part1(string input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        List<Hand> hands = new();

        foreach (string line in lines)
        {
            string[] parts = line.Split(" ");
            string cards = parts[0];
            int bid = int.Parse(parts[1]);

            Hand hand = new(cards, bid);
            hands.Add(hand);
        }

        ParallelMergeSort.Sort(hands);

        int totalWinnings = 0;
        int rank = 1;

        foreach (Hand hand in hands)
        {
            int handWinnings = hand.Bid * rank;
            totalWinnings += handWinnings;

            // Console.WriteLine($"Hand: {hand.Cards}, Rank: {rank}, Winnings: {handWinnings}");

            rank++;
        }

        int result = totalWinnings;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483", 5905)]
    [TestCase(@"2345A 1
Q2KJJ 13
Q2Q2Q 19
T3T3J 17
T3Q33 11
2345J 3
J345A 2
32T3K 5
T55J5 29
KK677 7
KTJJT 34
QQQJA 31
JJJJJ 37
JAAAA 43
AAAAJ 59
AAAAA 61
2AAAA 23
2JJJJ 53
JJJJ2 41", 6839)]
    [TestCase(null, 248747492)] // The actual answer
    public void Part2(string input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        List<Hand> hands = new();

        foreach (string line in lines)
        {
            string[] parts = line.Split(" ");
            string cards = parts[0];
            int bid = int.Parse(parts[1]);

            Hand hand = new(cards, bid, true);
            hands.Add(hand);
        }

        ParallelMergeSort.Sort(hands);

        int totalWinnings = 0;
        int rank = 1;

        foreach (Hand hand in hands)
        {
            int handWinnings = hand.Bid * rank;
            totalWinnings += handWinnings;

            Console.WriteLine(
                $"Hand: {hand.Cards}, Type: {hand.Type.ToString()}, Rank: {rank}, Winnings: {handWinnings}");

            rank++;
        }

        int result = totalWinnings;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}

public enum HandType
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}

public class Hand : IComparable<Hand>
{
    private readonly bool _useJokerLogic;

    public Hand(string cards, int bid, bool useJokerLogic = false)
    {
        _useJokerLogic = useJokerLogic;
        Cards = cards;
        Bid = bid;
        DetermineHandType();
    }

    public string Cards { get; }
    public int Bid { get; }
    public HandType Type { get; set; }

    public int CompareTo(Hand other)
    {
        // First compare based on hand type
        if (Type != other.Type)
        {
            return Type.CompareTo(other.Type);
        }

        // If types are the same, then compare based on individual card strength
        // Convert card labels to a list of integers for easier comparison
        List<int> thisCardStrengths = ConvertToCardStrengths(Cards, _useJokerLogic);
        List<int> otherCardStrengths = ConvertToCardStrengths(other.Cards, _useJokerLogic);

        for (int i = 0; i < thisCardStrengths.Count; i++)
        {
            if (thisCardStrengths[i] != otherCardStrengths[i])
            {
                return thisCardStrengths[i].CompareTo(otherCardStrengths[i]);
            }
        }

        // If all cards are the same, the hands are equal
        return 0;
    }

    private void DetermineHandType()
    {
        // Count occurrences of each card
        Dictionary<char, int> cardCounts = new();
        int jokers = 0;

        foreach (char card in Cards)
        {
            if (_useJokerLogic && card == 'J')
            {
                jokers++;
                continue;
            }

            if (cardCounts.ContainsKey(card))
            {
                cardCounts[card]++;
            }
            else
            {
                cardCounts.Add(card, 1);
            }
        }

        if (_useJokerLogic && jokers > 0)
        {
            ApplyJokerLogic(cardCounts, jokers);
        }
        else
        {
            DetermineStandardHandType(cardCounts);
        }
    }

    private void ApplyJokerLogic(Dictionary<char, int> cardCounts, int jokers)
    {
        // Get the maximum amount of any single card in the hand
        int maxCount = cardCounts.Any() ? cardCounts.Max(c => c.Value) : 0;

        // FiveOfAKind
        if (maxCount == 5 || maxCount + jokers == 5)
        {
            Type = HandType.FiveOfAKind;
            return;
        }

        // FourOfAKind
        if (maxCount == 4 || maxCount + jokers == 4)
        {
            Type = HandType.FourOfAKind;
            return;
        }

        // FullHouse
        // if there are 3 of one card and 2 of another
        // or if there are three of one card and one joker
        // or if there are two of one card and two jokers
        if (
            (cardCounts.Any(c => c.Value == 3) && cardCounts.Any(c => c.Value == 2))
            || (cardCounts.Any(c => c.Value == 3) && jokers == 1)
            || (cardCounts.Any(c => c.Value == 2) && jokers == 2)
            || (cardCounts.Count(c => c.Value == 2) == 2 && jokers == 1)
        )
        {
            Type = HandType.FullHouse;
            return;
        }

        // ThreeOfAKind
        // if there are three of one card
        // or if there are two of one card and one joker
        // or if there are two jokers
        if (cardCounts.Any(c => c.Value == 3)
            || (cardCounts.Any(c => c.Value == 2) && jokers == 1)
            || jokers == 2)
        {
            Type = HandType.ThreeOfAKind;
            return;
        }

        // TwoPair
        // if there are two of two cards
        // or if there is two of one card and two jokers
        // or if there are three jokers
        if (cardCounts.Count(c => c.Value == 2) == 2
            || (cardCounts.Any(c => c.Value == 2) && jokers == 1)
            || jokers == 3)
        {
            Type = HandType.TwoPair;
            return;
        }

        // OnePair
        // if there is two of one card
        // or if there is one joker
        if (cardCounts.Count(c => c.Value == 2) == 2 || jokers == 1)
        {
            Type = HandType.OnePair;
            return;
        }

        // HighCard
        // If no other hand can be formed, use a high card
        Type = HandType.HighCard;
    }

    private void DetermineStandardHandType(Dictionary<char, int> cardCounts)
    {
        // Sort counts to help identify patterns like full house or two pairs
        List<int> sortedCounts = cardCounts.Values.OrderByDescending(count => count).ToList();

        Type = sortedCounts[0] switch
        {
            // Determine hand type based on the sorted counts
            5 => HandType.FiveOfAKind,
            4 => HandType.FourOfAKind,
            3 when sortedCounts[1] == 2 => HandType.FullHouse,
            3 => HandType.ThreeOfAKind,
            2 when sortedCounts[1] == 2 => HandType.TwoPair,
            2 => HandType.OnePair,
            _ => HandType.HighCard
        };
    }

    private static List<int> ConvertToCardStrengths(string cards, bool useJokerLogic)
    {
        Dictionary<char, int> cardStrength = new()
        {
            { 'A', 14 },
            { 'K', 13 },
            { 'Q', 12 },
            { 'T', 10 },
            { '9', 9 },
            { '8', 8 },
            { '7', 7 },
            { '6', 6 },
            { '5', 5 },
            { '4', 4 },
            { '3', 3 },
            { '2', 2 },
            { 'J', useJokerLogic ? 1 : 11 } // 'J' is weakest if joker logic is enabled
        };

        return cards.Select(c => cardStrength[c]).ToList();
    }
}

public static class ParallelMergeSort
{
    private const int Threshold = 100;

    public static void Sort(List<Hand> hands)
    {
        if (hands.Count <= 1)
        {
            return;
        }

        int mid = hands.Count / 2;
        List<Hand> left = new(hands.GetRange(0, mid));
        List<Hand> right = new(hands.GetRange(mid, hands.Count - mid));

        if (hands.Count <= Threshold)
        {
            // Sequential sort for small lists
            Sort(left);
            Sort(right);
        }
        else
        {
            // Sort in parallel for large lists
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => Sort(left));
            tasks[1] = Task.Factory.StartNew(() => Sort(right));
            Task.WaitAll(tasks);
        }

        Merge(hands, left, right);
    }

    private static void Merge(List<Hand> hands, List<Hand> left, List<Hand> right)
    {
        int i = 0, j = 0, k = 0;

        while (i < left.Count && j < right.Count)
        {
            if (left[i].CompareTo(right[j]) <= 0)
            {
                hands[k++] = left[i++];
            }
            else
            {
                hands[k++] = right[j++];
            }
        }

        while (i < left.Count)
        {
            hands[k++] = left[i++];
        }

        while (j < right.Count)
        {
            hands[k++] = right[j++];
        }
    }
}
