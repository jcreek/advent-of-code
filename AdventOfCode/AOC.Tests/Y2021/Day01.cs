using System.Reflection;

namespace AOC.Tests.Y2021
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day01
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2021", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase("199,200,208,210,200,207,240,269,260,263", 7)] // Example data
        [TestCase(null, 1681)] // The actual answer
        public void Part1(string input, int? expected)
        {
            var lines = input != null ? input.Split(',') : realData;

            int numberOfBiggerMeasurements = 0;

            foreach (var (line, index) in lines.WithIndex())
            {
                if (index > 0)
                {
                    int previousReading = Int32.Parse(lines[index-1]);
                    int currentReading = Int32.Parse(line);
                    
                    if (currentReading > previousReading)
                    {
                        numberOfBiggerMeasurements += 1;
                    }
                }
            }

            if (expected != null)
            {
                Assert.That(numberOfBiggerMeasurements, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {numberOfBiggerMeasurements}");
        }

        [TestCase("199,200,208,210,200,207,240,269,260,263", 5)] // Example data
        [TestCase(null, 1704)] // The actual answer
        public void Part2(string input, int? expected)
        {
            var lines = input != null ? input.Split(',') : realData;

            int numberOfBiggerSlidingTotals = 0;

            List<int> slidingTotals = new List<int>();

            for (int i = 0; i < lines.Count(); i++) 
            {
            if (lines.Count() > i+2)
            {
                slidingTotals.Add(Int32.Parse(lines[i]) + Int32.Parse(lines[i+1]) + Int32.Parse(lines[i+2]));
            }
            }

            foreach (var (total, index) in slidingTotals.WithIndex())
            {
                if (index > 0)
                {
                    int previousTotal = slidingTotals[index-1];
                    int currentTotal = total;
                    
                    if (currentTotal > previousTotal)
                    {
                        numberOfBiggerSlidingTotals += 1;
                    }
                }
            }

            if (expected != null)
            {
                Assert.That(numberOfBiggerSlidingTotals, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {numberOfBiggerSlidingTotals}");
        }
    }
}