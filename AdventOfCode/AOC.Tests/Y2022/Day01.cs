using System.Reflection;

namespace AOC.Tests.Y2022
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day01
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2022", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase(@"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000", 24000)]
        [TestCase(null, 70764)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            List<int> elfTotalCalories = new() { 0 };

            int elfCounter = 0;
            foreach (string line in lines)
            {
                if (line.Length == 0)
                {
                    // Move to next elf
                    elfCounter += 1;
                    elfTotalCalories.Add(0);
                }
                else
                {
                    elfTotalCalories[elfCounter] += int.Parse(line);
                }
            }

            int result = elfTotalCalories.Max();

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        [TestCase(@"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000", 45000)]
        [TestCase(null, 203905)] // The actual answer
        public void Part2(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            List<int> elfTotalCalories = new() { 0 };

            int elfCounter = 0;
            foreach (string line in lines)
            {
                if (line.Length == 0)
                {
                    // Move to next elf
                    elfCounter += 1;
                    elfTotalCalories.Add(0);
                }
                else
                {
                    elfTotalCalories[elfCounter] += int.Parse(line);
                }
            }

            List<int>? topThreeCalorieElves = elfTotalCalories.OrderByDescending(x => x).Take(3).ToList();
            int result = topThreeCalorieElves.Sum();

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }
    }
}