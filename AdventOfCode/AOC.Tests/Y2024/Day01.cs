namespace AOC.Tests.Y2024;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day01
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2024", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"3   4
4   3
2   5
1   3
3   9
3   3", 11)]
    [TestCase(null, 2000468)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        List<int> left = new();
        List<int> right = new();

        for (int i = 0; i < lines.Length; i++)
        {
            string leftString = lines[i].Split("   ")[0];
            string rightString = lines[i].Split("   ")[1];

            left.Add(int.Parse(leftString));
            right.Add(int.Parse(rightString));
        }

        int totalDistance = 0;

        left = left.Order().ToList();
        right = right.Order().ToList();

        for (int i = 0; i < left.Count; i++)
        {
            // Get the difference between left and right at this index
            totalDistance += Math.Abs(left[i] - right[i]);
        }

        if (expected != null)
        {
            Assert.That(totalDistance, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {totalDistance}");
    }

    [TestCase(@"3   4
4   3
2   5
1   3
3   9
3   3", 31)]
    [TestCase(null, 18567089)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        //string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        List<int> left = new();
        List<int> right = new();

        for (int i = 0; i < lines.Length; i++)
        {
            string leftString = lines[i].Split("   ")[0];
            string rightString = lines[i].Split("   ")[1];

            left.Add(int.Parse(leftString));
            right.Add(int.Parse(rightString));
        }

        int similarityScore = 0;

        for (int i = 0; i < left.Count; i++)
        {
            int count = right.Count(x => x == left[i]);

            similarityScore += left[i] * count;
        }


        if (expected != null)
        {
            Assert.That(similarityScore, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {similarityScore}");
    }
}
