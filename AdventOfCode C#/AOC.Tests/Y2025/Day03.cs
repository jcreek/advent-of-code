using System.Text;

namespace AOC.Tests.Y2025;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day03
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2025", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"987654321111111
811111111111119
234234234234278
818181911112111", 357)]
    [TestCase(null, 17383)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        List<long> maxJoltages = new();

        foreach (string line in lines)
        {
            int[] digits = line
                .Select(c => (int)char.GetNumericValue(c))
                .ToArray();

            // Find the biggest number in the digits array (excluding the last digit), and its position
            int biggestDigit = digits[..^1].Max();
            int biggestDigitPosition = Array.IndexOf(digits, biggestDigit);

            // Ignore any digits before and including the biggest digit, then find the next biggest
            digits = digits.Skip(biggestDigitPosition + 1).ToArray();
            int nextBiggestDigit = digits.Max();

            int maxJoltage = biggestDigit * 10 + nextBiggestDigit;
            maxJoltages.Add(maxJoltage);
        }

        long result = maxJoltages.Sum();

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"987654321111111
811111111111119
234234234234278
818181911112111", 3121910778619)]
    [TestCase(null, 172601598658203)] // The actual answer
    public void Part2(string? input, long? expected)
    {
        //string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        List<long> maxJoltages = new();

        foreach (string line in lines)
        {
            int[] digits = line
                .Select(c => (int)char.GetNumericValue(c))
                .ToArray();

            List<int> biggestDigits = new();

            // Get the 12 biggest digits
            for (int i = 0; i < 12; i++)
            {
                // Find the biggest number in the digits array (excluding the remaining 11-0 digits), and its position
                int biggestDigit = digits[..^(11 - i)].Max();
                int biggestDigitPosition = Array.IndexOf(digits, biggestDigit);

                // Ignore any digits before and including the biggest digit, then find the next biggest
                digits = digits.Skip(biggestDigitPosition + 1).ToArray();

                biggestDigits.Add(biggestDigit);
            }

            StringBuilder sb = new();
            foreach (int digit in biggestDigits)
            {
                sb.Append(digit);
            }

            long maxJoltage = long.Parse(sb.ToString());
            maxJoltages.Add(maxJoltage);
        }

        long result = maxJoltages.Sum();

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
