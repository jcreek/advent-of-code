using System.Reflection;

namespace AOC.Tests.YX
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class DayX
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "YX", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase("blah", 0)]
        [TestCase(null, 232)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string[] lines = input != null ? new[] { input } : realData;

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

            //if (expected != null)
            //{
            //    Assert.That(result, Is.EqualTo(expected.Value));
            //}

            //Console.WriteLine($"Part 2: {result}");
        }
    }
}