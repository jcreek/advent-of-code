using Creek.HelpfulExtensions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AOC.Tests.Y2015
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day05
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2015", "Data", $"{GetThisClassName()}.dat"));
        }

        private static bool HasRepeatedCharacters(string input)
        {
            bool hasRepeatedCharacters = false;

            if (input.Length >= 2)
            {
                for (int index = 0; index < input.Length - 1; index++)
                {
                    if (input[index] == input[index + 1])
                    {
                        hasRepeatedCharacters = true;
                    }
                }
            }

            return hasRepeatedCharacters;
        }

        private static bool IsNice(string checkString)
        {
            // Count non-unique vowels
            Regex rx = new("[aeiou]");
            int vowelCount = rx.Matches(checkString).Count;

            // Check for at least one letter appearing twice in a row
            bool hasRepeatedCharacters = HasRepeatedCharacters(checkString);

            // Check it does not contain the strings ab, cd, pq, or xy
            bool containsForbiddenStrings = checkString.Contains("ab")
                || checkString.Contains("cd")
                || checkString.Contains("pq")
                || checkString.Contains("xy");

            return vowelCount >= 3 && hasRepeatedCharacters && !containsForbiddenStrings;
        }

        private class Pair
        {
            public int LeftIndex { get; set; }
            public int RightIndex { get; set; }
            public string? Text { get; set; }
        }

        private static bool TwiceWithoutOverlap(string checkString)
        {
            // It contains a pair of any two letters that appears at least twice in the string without overlapping,
            // like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).

            // Split the string into 'pairs'
            List<Pair> pairs = new();
            for (int i = 0; i < checkString.Length - 1; i++)
            {
                Pair pair = new()
                {
                    LeftIndex = i,
                    RightIndex = i + 1,
                    Text = $"{checkString[i]}{checkString[i + 1]}",
                };

                pairs.Add(pair);
            }

            List<Pair> possibleNonOverlappingPairs = new();
            // Check if any of those pairs occurs more than once
            foreach (Pair pair in pairs)
            {
                int count = Regex.Matches(checkString, pair.Text).Count;

                if (count > 1)
                {
                    possibleNonOverlappingPairs.Add(pair);
                }
            }

            // Check if any do not overlap, if they don't then return true
            foreach (Pair pair in possibleNonOverlappingPairs)
            {
                if (possibleNonOverlappingPairs.Any(p =>
                    p.Text == pair.Text
                    && (p.LeftIndex != pair.RightIndex || p.RightIndex != pair.LeftIndex)
                    ))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckRepeatedCharacters(string checkString)
        {
            // It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
            for (int i = 0; i < checkString.Length; i++)
            {
                if ((i + 2) < checkString.Length)
                {
                    if (checkString[i] == checkString[i + 2] && checkString[i] != checkString[i + 1])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsNiceV2(string checkString)
        {
            // It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
            bool twiceWithoutOverlapping = TwiceWithoutOverlap(checkString);

            // It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
            bool hasRepeatedCharacters = CheckRepeatedCharacters(checkString);

            return twiceWithoutOverlapping && hasRepeatedCharacters;
        }

        [TestCase("ugknbfddgicrmopn", 1)]
        [TestCase("aaa", 1)]
        [TestCase("jchzalrnumimnmhp", 0)]
        [TestCase("haegwjzuvuyypxyu", 0)]
        [TestCase("dvszwmarrgswjxmb", 0)]
        [TestCase(null, 258)] // The actual answer
        public void Part1(string? input, int? expected)
        {
            string[] lines = input != null ? new[] { input } : realData;

            int niceCount = 0;

            foreach (string line in lines)
            {
                if (IsNice(line))
                {
                    niceCount += 1;
                }
            }

            if (expected != null)
            {
               Assert.That(niceCount, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {niceCount}");
        }

        [TestCase("qjhvhtzxzqqjkmpb", 1)]
        [TestCase("xxyxx", 1)]
        [TestCase("uurcxstgmygtbstg", 0)]
        [TestCase("ieodomkazucvgmuy", 0)]
        [TestCase(null, 53)] // The actual answer
        public void Part2(string? input, int? expected)
        {
            string[] lines = input != null ? new[] { input } : realData;

            int niceCount = 0;

            foreach (string line in lines)
            {
                if (IsNiceV2(line))
                {
                    niceCount += 1;
                }
            }

            if (expected != null)
            {
                Assert.That(niceCount, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {niceCount}");
        }
    }
}
