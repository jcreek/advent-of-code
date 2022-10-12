using System.Reflection;

namespace AOC.Tests.Y2015
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day01
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2015", "Data", $"{GetThisClassName()}.dat"))[0];
        }

        [TestCase("(())", 0)] // Example data
        [TestCase("()()", 0)] // Example data
        [TestCase("(((", 3)] // Example data
        [TestCase("(()(()(", 3)] // Example data
        [TestCase("))(((((", 3)] // Example data
        [TestCase("())", -1)] // Example data
        [TestCase("))(", -1)] // Example data
        [TestCase(")))", -3)] // Example data
        [TestCase(")())())", -3)] // Example data
        [TestCase(null, 232)] // The actual answer
        public void Part1(string input, int? expected)
        {
            var lines = input != null ? input : realData;

            int floorsUp = lines.Count(x => x == '(');
            int floorsDown = lines.Count(x => x == ')');

            var result = floorsUp - floorsDown;


            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        [TestCase(")", 1)] // Example data
        [TestCase("()())", 5)]
        [TestCase(null, 1783)] // The actual answer
        public void Part2(string input, int? expected)
        {
            var lines = input != null ? input : realData;

            var currentFloor = 0;
            int? result = null;

            foreach (var (instruction, index) in lines.WithIndex())
            {
                if (instruction is '(')
                {
                    currentFloor += 1;
                }
                else if (instruction is ')')
                {
                    currentFloor -= 1;
                }

                if (currentFloor is -1)
                {
                    // Zero-indexed, so need to increment
                    result = index + 1;
                    break;
                }
            }

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }
    }
}