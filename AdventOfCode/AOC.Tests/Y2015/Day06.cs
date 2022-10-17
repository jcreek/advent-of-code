using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace AOC.Tests.Y2015
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day06
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2015", "Data", $"{GetThisClassName()}.dat"));
        }

        private enum Instructions
        {
            Toggle = 7,
            TurnOn = 8,
            TurnOff = 9,
        }

        private void OperateLights(string instruction, ref bool[,] lights)
        {
            Instructions currentInstruction = instruction switch
            {
                string s when s.StartsWith("toggle") => Instructions.Toggle,
                string s when s.StartsWith("turn on") => Instructions.TurnOn,
                string s when s.StartsWith("turn off") => Instructions.TurnOff,
                _ => throw new InvalidDataException($"Bad instruction -> {instruction}"),
            };

            // e.g. turn on 0,0 through 999,999
            string[] instructionParts = instruction.Substring((int)currentInstruction).Split(' ');

            int startX = int.Parse(instructionParts[0].Substring(0, instructionParts[0].IndexOf(",")));
            int startY = int.Parse(instructionParts[0].Substring(instructionParts[0].IndexOf(",") + 1));
            int endX = int.Parse(instructionParts[2].Substring(0, instructionParts[2].IndexOf(",")));
            int endY = int.Parse(instructionParts[2].Substring(instructionParts[2].IndexOf(",") + 1));

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    lights[i, j] = currentInstruction switch
                    {
                        Instructions.Toggle => !lights[i, j],
                        Instructions.TurnOn => true,
                        Instructions.TurnOff => false,
                        _ => throw new NotImplementedException(),
                    };
                }
            }
        }

        private void OperateScalingLights(string instruction, ref int[,] lights)
        {
            Instructions currentInstruction = instruction switch
            {
                string s when s.StartsWith("toggle") => Instructions.Toggle,
                string s when s.StartsWith("turn on") => Instructions.TurnOn,
                string s when s.StartsWith("turn off") => Instructions.TurnOff,
                _ => throw new InvalidDataException($"Bad instruction -> {instruction}"),
            };

            // e.g. turn on 0,0 through 999,999
            string[] instructionParts = instruction.Substring((int)currentInstruction).Split(' ');

            int startX = int.Parse(instructionParts[0].Substring(0, instructionParts[0].IndexOf(",")));
            int startY = int.Parse(instructionParts[0].Substring(instructionParts[0].IndexOf(",") + 1));
            int endX = int.Parse(instructionParts[2].Substring(0, instructionParts[2].IndexOf(",")));
            int endY = int.Parse(instructionParts[2].Substring(instructionParts[2].IndexOf(",") + 1));

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    lights[i, j] = currentInstruction switch
                    {
                        Instructions.Toggle => lights[i, j] += 2,
                        Instructions.TurnOn => lights[i, j] += 1,
                        Instructions.TurnOff => lights[i, j] > 0 ? lights[i, j] -= 1 : lights[i, j] = 0,
                        _ => throw new NotImplementedException(),
                    };
                }
            }
        }

        [TestCase("turn on 0,0 through 999,999", 1000000)]
        [TestCase("toggle 0,0 through 999,0", 1000)]
        [TestCase("turn on 0,0 through 999,999/nturn off 499,499 through 500,500", 999996)]
        [TestCase(null, 400410)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("/n") : realData;

            bool[,] lights = new bool[1000, 1000];

            foreach (string line in lines)
            {
                OperateLights(line, ref lights);
            }

            IEnumerable<bool> lightsOn = from bool light in lights
                                         where light
                                         select light;

            int lightsOnCount = lightsOn.Count();

            if (expected != null)
            {
                Assert.That(lightsOnCount, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {lightsOnCount}");
        }

        [TestCase("turn on 0,0 through 0,0", 1)]
        [TestCase("toggle 0,0 through 999,999", 2000000)]
        [TestCase(null, 15343601)] // The actual answer
        public void Part2(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("/n") : realData;

            int[,] lights = new int[1000, 1000];

            foreach (string line in lines)
            {
                OperateScalingLights(line, ref lights);
            }

            int totalBrightness = lights.Cast<int>().Sum();

            if (expected != null)
            {
                Assert.That(totalBrightness, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {totalBrightness}");
        }
    }
}