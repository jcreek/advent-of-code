using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace AOC.Tests.Y2015
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day04
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2015", "Data", $"{GetThisClassName()}.dat"))[0];
        }

        private int CreateMD5Hash(string input, string startingString)
        {
            int result = 0;

            MD5 md5 = MD5.Create();

            while (true)
            {
                byte[] inputBytes = new UTF8Encoding().GetBytes($"{input}{result}");
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }

                if (sb.ToString().StartsWith(startingString))
                {
                    break;
                }

                result += 1;
            }

            return result;
        }

        [TestCase("abcdef", 609043)]
        [TestCase("pqrstuv", 1048970)]
        [TestCase(null, 254575)] // The actual answer
        public void Part1(string? input, int? expected)
        {
            string lines = input != null ? input : realData;

            var i = CreateMD5Hash(lines, "00000");

            if (expected != null)
            {
                Assert.That(i, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {i}");
        }

        [TestCase(null, 1038736)] // The actual answer
        public void Part2(string? input, int? expected)
        {
            string lines = input != null ? input : realData;

            var i = CreateMD5Hash(lines, "000000");

            if (expected != null)
            {
                Assert.That(i, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {i}");
        }
    }
}
