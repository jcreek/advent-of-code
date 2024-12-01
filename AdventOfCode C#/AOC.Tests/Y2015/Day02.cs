using System;
using System.Reflection;

namespace AOC.Tests.Y2015
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day02
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2015", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase("2x3x4", 58)]
        [TestCase("1x1x10", 43)]
        [TestCase(null, 1606483)] // The actual answer
        public void Part1(string? input, int? expected)
        {
            string[] lines = input != null ? new[] { input } : realData;

            int totalWrappingPaper = 0;

            foreach (var present in lines)
            {
                string[] dimensions = present.Split('x');
                int length = Int32.Parse(dimensions[0]);
                int width = Int32.Parse(dimensions[1]);
                int height = Int32.Parse(dimensions[2]);

                int sideAArea = length * width;
                int sideBArea = width * height;
                int sideCArea = height * length;

                int smallestSideArea = new List<int>() { sideAArea, sideBArea, sideCArea }.Min();

                int totalPaperForPresent = (2 * sideAArea) + (2 * sideBArea) + (2 * sideCArea) + smallestSideArea;

                totalWrappingPaper += totalPaperForPresent;
            }

            if (expected != null)
            {
                Assert.That(totalWrappingPaper, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {totalWrappingPaper}");
        }

        [TestCase("2x3x4", 34)]
        [TestCase("1x1x10", 14)]
        [TestCase(null, 3842356)] // The actual answer
        public void Part2(string? input, int? expected)
        {
            string[] lines = input != null ? new[] { input } : realData;

            var totalRibbon = 0;

            foreach (var present in lines)
            {
                string[] dimensions = present.Split('x');
                int[] dimensionsInts = Array.ConvertAll(dimensions, s => int.Parse(s));

                int length = dimensionsInts[0];
                int width = dimensionsInts[1];
                int height = dimensionsInts[2];

                Array.Sort(dimensionsInts);

                var smallestPerimeter = (2 * dimensionsInts[0]) + (2 * dimensionsInts[1]);
                var cubicFeetVolume = length * width * height;

                var totalRibbonForPresent = smallestPerimeter + cubicFeetVolume;

                totalRibbon += totalRibbonForPresent;
            }

            if (expected != null)
            {
                Assert.That(totalRibbon, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {totalRibbon}");
        }
    }
}
