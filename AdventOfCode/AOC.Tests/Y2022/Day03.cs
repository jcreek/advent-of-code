using System.Reflection;

namespace AOC.Tests.Y2022
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day03
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2022", "Data", $"{GetThisClassName()}.dat"));
        }

        private char FindItemInBothCompartments(string rucksack)
        {
            string firstCompartment = rucksack[..(rucksack.Length / 2)];
            string secondCompartment = rucksack.Substring(rucksack.Length / 2, rucksack.Length / 2);

            List<char> commonItems = firstCompartment.Intersect(secondCompartment).ToList();

            return commonItems.Count == 1
                ? commonItems[0]
                : throw new IndexOutOfRangeException("More than one common item between compartments");
        }

        private char FindCommonItem(string rucksack1, string rucksack2, string rucksack3)
        {
            List<char> allChars = new();
            foreach (char letter in rucksack1)
            {
                allChars.Add(letter);
            }
            foreach (char letter in rucksack2)
            {
                allChars.Add(letter);
            }
            foreach (char letter in rucksack3)
            {
                allChars.Add(letter);
            }

            List<char> commonItems1 = rucksack1.Intersect(rucksack2).ToList();
            List<char> commonItems2 = rucksack1.Intersect(rucksack3).ToList();
            List<char> commonItems3 = rucksack2.Intersect(rucksack3).ToList();

            List<char> commonItems = allChars.Where(c => commonItems1.Contains(c) && commonItems2.Contains(c) && commonItems3.Contains(c)).Distinct().ToList();

            return commonItems.Count == 1
                ? commonItems[0]
                : throw new IndexOutOfRangeException("More than one common item between compartments");
        }

        private int GetPriority(char item)
        {
            if (char.IsUpper(item))
            {
                // Uppercase item types A through Z have priorities 27 through 52.
                // 65 is ascii for A -> -38
                return item - 38;
            }
            else
            {
                // Lowercase item types a through z have priorities 1 through 26.
                // 97 is ascii for A -> -96
                return item - 96;
            }
        }

        [TestCase(@"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw", 157)]
        [TestCase(null, 8176)] // The actual answer
        public void Part1(string? input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;
            List<char> items = new();
            List<int> priorities = new();

            // Find char appearing in both halves of the input string (case sensitive)
            foreach (string line in lines)
            {
                items.Add(FindItemInBothCompartments(line));
            }

            // Get priority value of char 
            foreach (char item in items)
            {
                priorities.Add(GetPriority(item));
            }

            // Get sum of priorities
            int result = priorities.Sum();

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        [TestCase(@"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw", 70)]
        [TestCase(null, 2689)] // The actual answer
        public void Part2(string? input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;
            List<char> items = new();
            List<int> priorities = new();

            // Get each three line group and find the common item
            for (int i = 0; i < lines.Length; i += 3)
            {
                items.Add(FindCommonItem(lines[i], lines[i + 1], lines[i + 2]));
            }

            // Get the priorities of all common items
            foreach (char item in items)
            {
                priorities.Add(GetPriority(item));
            }

            // Get sum of priorities
            int result = priorities.Sum();

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }
    }
}
