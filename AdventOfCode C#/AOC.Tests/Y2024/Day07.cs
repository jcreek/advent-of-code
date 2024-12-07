namespace AOC.Tests.Y2024;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day07
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2024", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private bool CanAchieveTarget(long[] remainingNumbers, long testValue, long currentIndex, long currentTotal)
    {
        // Base case: if we've used all numbers, check if the total equals the test value
        if (currentIndex == remainingNumbers.Length)
        {
            return currentTotal == testValue;
        }

        // Try adding the next number
        if (CanAchieveTarget(remainingNumbers, testValue, currentIndex + 1,
                currentTotal + remainingNumbers[currentIndex]))
        {
            return true;
        }

        // Try multiplying by the next number
        if (CanAchieveTarget(remainingNumbers, testValue, currentIndex + 1,
                currentTotal * remainingNumbers[currentIndex]))
        {
            return true;
        }

        // If neither operation works, return false
        return false;
    }

    private bool CanAchieveTargetWithConcatenation(long[] remainingNumbers, long testValue, long currentIndex,
        long currentTotal)
    {
        // If none of the three operations works, return false
        return false;
    }


    private (bool, long) CanEquationBeTrue(string equation, bool enableConcatenation = false)
    {
        try
        {
            // Split the equation into its parts
            string[] parts = equation.Split(": ");
            long testValue = long.Parse(parts[0]);
            long[] remainingNumbers = parts[1].Split(" ").Select(long.Parse).ToArray();

            if (enableConcatenation)
            {
                bool result = CanAchieveTargetWithConcatenation(remainingNumbers, testValue, 1, remainingNumbers[0]);

                return (result, testValue);
            }
            else
            {
                // Start recursing through the numbers
                bool result = CanAchieveTarget(remainingNumbers, testValue, 1, remainingNumbers[0]);

                return (result, testValue);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(equation, ex);
            throw;
        }
    }

    [TestCase(@"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20", 3749)]
    [TestCase(null, 1620690235709)] // The actual answer
    public void Part1(string? input, long? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        long result = 0;

        foreach (string line in lines)
        {
            (bool, long) response = CanEquationBeTrue(line);
            if (response.Item1)
            {
                result += response.Item2;
            }
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20", 11387)]
    [TestCase(null, 1783)] // The actual answer
    public void Part2(string? input, long? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        long result = 0;

        foreach (string line in lines)
        {
            (bool, long) response = CanEquationBeTrue(line, true);
            if (response.Item1)
            {
                result += response.Item2;
            }
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
