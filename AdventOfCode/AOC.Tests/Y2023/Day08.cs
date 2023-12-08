namespace AOC.Tests.Y2023;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day08
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)", 2)]
    [TestCase(@"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)", 6)]
    [TestCase(null, 232)] // The actual answer
    public void Part1(string input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        //if (expected != null)
        //{
        //    Assert.That(result, Is.EqualTo(expected.Value));
        //}

        //Console.WriteLine($"Part 1: {result}");
    }

    [TestCase("blah", 1)]
    [TestCase(null, 1783)] // The actual answer
    public void Part2(string input, int? expected)
    {
        //string[] lines = input != null ? new[] { input } : realData;
        // string[] lines = input != null ? input.Split("\n") : realData;

        //if (expected != null)
        //{
        //    Assert.That(result, Is.EqualTo(expected.Value));
        //}

        //Console.WriteLine($"Part 2: {result}");
    }
}
