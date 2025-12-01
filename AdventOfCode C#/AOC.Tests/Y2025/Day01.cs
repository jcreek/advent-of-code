namespace AOC.Tests.Y2025;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day01
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2025", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private int TurnDial(int startNumber, char direction, int clicks)
    {
        int delta = direction switch
        {
            'R' => clicks,
            'L' => -clicks,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        int finalNumber = (startNumber + delta) % 100;
        if (finalNumber < 0)
        {
            finalNumber += 100;
        }

        return finalNumber;
    }

    private int CountZeroHits(int startNumber, char direction, int clicks)
    {
        if (clicks <= 0)
        {
            return 0;
        }

        int firstHit;

        if (direction == 'R')
        {
            // Position after k clicks: (startNumber + k) mod 100 == 0
            // => k ≡ -startNumber ≡ 100 - startNumber (mod 100)
            firstHit = (100 - startNumber) % 100;
            if (firstHit == 0)
            {
                firstHit = 100; // don't count "k = 0", we only care about actual clicks
            }
        }
        else if (direction == 'L')
        {
            // Position after k clicks: (startNumber - k) mod 100 == 0
            // => k ≡ startNumber (mod 100)
            firstHit = startNumber % 100;
            if (firstHit == 0)
            {
                firstHit = 100;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        if (firstHit > clicks)
        {
            return 0; // we never reach 0 within this rotation
        }

        // After the first hit, every extra 100 clicks we hit 0 again
        int remaining = clicks - firstHit;
        return 1 + remaining / 100;
    }

    private int HandleRotations(string[] lines, bool isPart2 = false)
    {
        int startNumber = 50;
        int zerosCount = 0;

        foreach (string line in lines)
        {
            // L or R, then a number to EOL
            char direction = line[0];
            int clicks = int.Parse(line.Substring(1));

            // The dial is a circle, turning the dial left from 0 one click makes it point at 99.
            // Similarly, turning the dial right from 99 one click makes it point at 0.

            if (isPart2)
            {
                // Count every click that lands on 0 (during + at end of rotation).
                zerosCount += CountZeroHits(startNumber, direction, clicks);
            }

            // For the final dial position ignore full rotations.
            int clicksLessFullRotations = clicks % 100;
            int finalNumber = TurnDial(startNumber, direction, clicksLessFullRotations);

            if (!isPart2 && finalNumber == 0)
            {
                zerosCount++;
            }

            startNumber = finalNumber;
        }

        return zerosCount;
    }

    [TestCase(@"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82", 3)]
    [TestCase(null, 1066)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        int result = HandleRotations(lines);

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82", 6)]
    [TestCase(null, 6223)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        int result = HandleRotations(lines, true);

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
