namespace AOC.Tests.Y2025;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day05
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2025", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"3-5
10-14
16-20
12-18

1
5
8
11
17
32", (long)3)]
    [TestCase(null, (long)513)] // The actual answer
    public void Part1(string? input, long? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        bool isListOfValidIngredients = true;

        List<(long Start, long End)> validRanges = new();
        List<long> availableIngredients = new();

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                isListOfValidIngredients = false;
                continue;
            }

            if (isListOfValidIngredients)
            {
                string[] rangeNumbers = line.Split('-', 2);
                long startNumber = long.Parse(rangeNumbers[0]);
                long endNumber = long.Parse(rangeNumbers[1]);

                validRanges.Add((startNumber, endNumber));
            }
            else
            {
                availableIngredients.Add(long.Parse(line));
            }
        }

        // For each available ingredient, check if it falls in any valid range
        long result = availableIngredients.Count(value =>
            validRanges.Any(r => value >= r.Start && value <= r.End));

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"3-5
10-14
16-20
12-18

1
5
8
11
17
32", (long)14)]
    [TestCase(null, (long)1783)] // The actual answer
    public void Part2(string? input, long? expected)
    {
        //string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        List<(long Start, long End)> validRanges = new();

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            string[] rangeNumbers = line.Split('-', 2);
            long startNumber = long.Parse(rangeNumbers[0]);
            long endNumber = long.Parse(rangeNumbers[1]);

            validRanges.Add((startNumber, endNumber));
        }

        // Sort ranges by Start then End
        validRanges.Sort((a, b) =>
        {
            int cmp = a.Start.CompareTo(b.Start);
            return cmp != 0 ? cmp : a.End.CompareTo(b.End);
        });

        // Merge overlapping or adjacent ranges
        List<(long Start, long End)> merged = new();
        (long Start, long End) current = validRanges[0];

        for (int i = 1; i < validRanges.Count; i++)
        {
            (long Start, long End) next = validRanges[i];

            // overlapping or touching?
            if (next.Start <= current.End + 1)
            {
                current = (current.Start, Math.Max(current.End, next.End));
            }
            else
            {
                merged.Add(current);
                current = next;
            }
        }

        merged.Add(current);

        long result = 0;
        foreach ((long start, long end) in merged)
        {
            // +1 because both ends are inclusive
            result += end - start + 1;
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
