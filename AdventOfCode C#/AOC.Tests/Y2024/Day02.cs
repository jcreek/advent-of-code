namespace AOC.Tests.Y2024;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day02
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2024", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private bool IsReportSafe(string report)
    {
        // Each report is a string of numbers (levels) separated by spaces
        int[] levels = report.Split(' ').Select(int.Parse).ToArray();

        // If the numbers in the report are in ascending or descending order, return true
        bool isIncreasing = true;
        bool isDecreasing = true;

        for (int i = 1; i < levels.Length; i++)
        {
            int diff = Math.Abs(levels[i] - levels[i - 1]);
            if (diff < 1 || diff > 3)
            {
                return false;
            }

            if (levels[i] > levels[i - 1])
            {
                isDecreasing = false;
            }

            if (levels[i] < levels[i - 1])
            {
                isIncreasing = false;
            }
        }

        if (!isIncreasing && !isDecreasing)
        {
            return false;
        }


        // If any two adjacent numbers differ by more than 3 or are the same, return false
        for (int i = 1; i < levels.Length; i++)
        {
            if (Math.Abs(levels[i] - levels[i - 1]) > 3)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsReportSafeAllowRemovingOneNumber(string report)
    {
        if (IsReportSafe(report))
        {
            return true;
        }

        string[] levels = report.Split(' ');

        // Iterate through each level and check if removing it makes the report safe
        for (int i = 0; i < levels.Length; i++)
        {
            string modifiedReport = string.Join(' ', levels.Where((_, index) => index != i));

            if (IsReportSafe(modifiedReport))
            {
                return true;
            }
        }

        return false;
    }

    [TestCase(@"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9", 2)]
    [TestCase(null, 585)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        int amountOfSafeReports = 0;

        foreach (string report in lines)
        {
            if (IsReportSafe(report))
            {
                amountOfSafeReports += 1;
            }
        }

        if (expected != null)
        {
            Assert.That(amountOfSafeReports, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {amountOfSafeReports}");
    }

    [TestCase(@"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9", 4)]
    [TestCase(null, 626)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        int amountOfSafeReports = 0;

        foreach (string report in lines)
        {
            if (IsReportSafeAllowRemovingOneNumber(report))
            {
                amountOfSafeReports += 1;
            }
        }

        if (expected != null)
        {
            Assert.That(amountOfSafeReports, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {amountOfSafeReports}");
    }
}
