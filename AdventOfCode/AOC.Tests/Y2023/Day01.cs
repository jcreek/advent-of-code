using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AOC.Tests.Y2023
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day01
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase(@"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet", 142)]
        [TestCase(null, 53080)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            int result = 0;

            foreach (string line in lines)
            {
                // Get all characters that are digits
                string digits = new(line.Where(char.IsDigit).ToArray());
                if (digits.Length == 1)
                {
                    digits += digits;
                }
                else if (digits.Length > 2)
                {
                    digits = $"{digits[0]}{digits[digits.Length - 1]}";
                }
                result += int.Parse(digits);
            }

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        [TestCase(@"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen", 281)]
        [TestCase("three2six8two5", 35)]
        [TestCase("eightjzqzhrllg1oneightfck", 88)]
        [TestCase(null, 53268)] // The actual answer
        public void Part2(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            int result = 0;
            foreach (string line in lines)
            {
                result += GetNumber(line);
            }

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }

        private record FoundMatch
        {
            public string Number { get; set; }
            public List<int> Indexes { get; set; }
        };
        
        private int GetNumber(string input)
        {
            // Find every instance of a match for these strings, along with the index of their first character
            List<string> stringsToMatch = new()
            {
                "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4", "5", "6", "7", "8", "9"
            };

            List<FoundMatch> foundMatches = new();

            foreach (string number in stringsToMatch)
            {
                if (!input.Contains(number))
                {
                    continue;
                }

                FoundMatch foundMatch = new()
                {
                    Number = number,
                    Indexes = new(),
                };

                // Find each instance of the string within the string, and record the index of the first character for each
                int index = input.IndexOf(number, StringComparison.Ordinal);
                while (index != -1)
                {
                    foundMatch.Indexes.Add(index);
                    index = input.IndexOf(number, index + 1, StringComparison.Ordinal);
                }

                foundMatches.Add(foundMatch);
            }

            // Get the first occuring number in foundMatches, and the last occurring
            FoundMatch firstMatch = foundMatches.OrderBy(x => x.Indexes[0]).First();
            FoundMatch lastMatch = foundMatches.OrderBy(x => x.Indexes[^1]).Last();

            Dictionary<string, string> numbersToConvert = new()
            {
                {"one", "1"},
                {"two", "2"},
                {"three", "3"},
                {"four", "4"},
                {"five", "5"},
                {"six", "6"},
                {"seven", "7"},
                {"eight", "8"},
                {"nine", "9"},
            };

            // Get the firstDigit and lastDigit as ints, converting any words using the numbersToConvert dictionary
            string firstDigit = firstMatch.Number;
            if (numbersToConvert.ContainsKey(firstMatch.Number))
            {
                firstDigit = numbersToConvert[firstMatch.Number];
            }
            string lastDigit = lastMatch.Number;
            if (numbersToConvert.ContainsKey(lastMatch.Number))
            {
                lastDigit = numbersToConvert[lastMatch.Number];
            }

            int finalNumber = int.Parse($"{firstDigit}{lastDigit}");
            Console.WriteLine(finalNumber);

            return finalNumber;
        }
    }
}