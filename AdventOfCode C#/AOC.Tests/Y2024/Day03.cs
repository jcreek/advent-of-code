using System.Text.RegularExpressions;

namespace AOC.Tests.Y2024;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day03
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2024", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", 161)]
    [TestCase(null, 168539636)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        int result = 0;

        foreach (string line in lines)
        {
            MatchCollection instructions = Regex.Matches(line, @"mul\((\d{1,3}),(\d{1,3})\)");

            foreach (Match match in instructions)
            {
                if (match.Success)
                {
                    int firstNumber = int.Parse(match.Groups[1].Value);
                    int secondNumber = int.Parse(match.Groups[2].Value);

                    result += firstNumber * secondNumber;
                }
            }
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", 48)]
    [TestCase(null, 97529391)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        int result = 0;
        bool enableMuls = true;

        foreach (string line in lines)
        {
            MatchCollection instructions = Regex.Matches(line,
                @"(?<do>do\(\))|(?<dont>don't\(\))|mul\((?<num1>\d{1,3}),(?<num2>\d{1,3})\)");

            foreach (Match match in instructions)
            {
                if (match.Success)
                {
                    if (match.Groups["do"].Success)
                    {
                        enableMuls = true;
                    }
                    else if (match.Groups["dont"].Success)
                    {
                        enableMuls = false;
                    }
                    else if (match.Groups["num1"].Success && match.Groups["num2"].Success)
                    {
                        if (enableMuls)
                        {
                            int firstNumber = int.Parse(match.Groups["num1"].Value);
                            int secondNumber = int.Parse(match.Groups["num2"].Value);

                            result += firstNumber * secondNumber;
                        }
                    }
                }
            }
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
