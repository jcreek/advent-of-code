using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace AOC.Tests.Y2015
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day03
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string realData;
        private class House
        {
            public int VisitedCount { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        };

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2015", "Data", $"{GetThisClassName()}.dat"))[0];
        }

        private void DeliverPresentToHouse(int x, int y, ref List<House> houses)
        {
            if (houses.FindIndex(h => h.X == x && h.Y ==y) > -1)
            {
                foreach (House house in houses.Where(h => h.X == x && h.Y == y))
                {
                    house.VisitedCount += 1;
                }
            }
            else
            {
                House house = new House()
                {
                    VisitedCount = 1,
                    X = x,
                    Y = y,
                };
                houses.Add(house);
            }
        }

        [TestCase(">", 2)]
        [TestCase("^>v<", 4)]
        [TestCase("^v^v^v^v^v", 2)]
        [TestCase(null, 2565)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string lines = input != null ? input : realData;
            int currentX = 0;
            int currentY = 0;

            List<House> houses = new List<House>();

            House firstHouse = new House()
            {
                VisitedCount = 1,
                X = currentX,
                Y = currentY,
            };
            houses.Add(firstHouse);

            foreach (char movement in lines)
            {
                switch (movement)
                {
                    case '^':
                        // North
                        currentY += 1;
                        break;
                    case 'v':
                        // South
                        currentY -= 1;
                        break;
                    case '<':
                        // East
                        currentX += 1;
                        break;
                    case '>':
                        // West
                        currentX -= 1;
                        break;
                }

                DeliverPresentToHouse(currentX, currentY, ref houses);
            }

            int numberOfHousesVisited = houses.Count;

            if (expected != null)
            {
                Assert.That(numberOfHousesVisited, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {numberOfHousesVisited}");
        }

        [TestCase("^v", 3)]
        [TestCase("^>v<", 3)]
        [TestCase("^v^v^v^v^v", 11)]
        [TestCase(null, 2639)] // The actual answer
        public void Part2(string input, int? expected)
        {
            string lines = input != null ? input : realData;
            int currentX = 0;
            int currentY = 0;
            int roboSantacurrentX = 0;
            int roboSantacurrentY = 0;

            List<House> houses = new List<House>();

            House firstHouse = new House()
            {
                VisitedCount = 2, // Visited by both Santa and Robo-Santa
                X = currentX,
                Y = currentY,
            };
            houses.Add(firstHouse);

            bool isSanta = true;

            foreach (char movement in lines)
            {
                switch (movement)
                {
                    case '^':
                        // North
                        if (isSanta)
                        {
                            currentY += 1;
                        }
                        else
                        {
                            roboSantacurrentY += 1;
                        }
                        break;
                    case 'v':
                        // South
                        if (isSanta)
                        {
                            currentY -= 1;
                        }
                        else
                        {
                            roboSantacurrentY -= 1;
                        }
                        break;
                    case '<':
                        // East
                        if (isSanta)
                        {
                            currentX += 1;
                        }
                        else
                        {
                            roboSantacurrentX += 1;
                        }
                        break;
                    case '>':
                        // West
                        if (isSanta)
                        {
                            currentX -= 1;
                        }
                        else
                        {
                            roboSantacurrentX -= 1;
                        }
                        break;
                }

                if (isSanta)
                {
                    DeliverPresentToHouse(currentX, currentY, ref houses);
                    isSanta = false;
                }
                else
                {
                    DeliverPresentToHouse(roboSantacurrentX, roboSantacurrentY, ref houses);
                    isSanta = true;
                }
            }

            int numberOfHousesVisited = houses.Count;

            if (expected != null)
            {
                Assert.That(numberOfHousesVisited, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {numberOfHousesVisited}");
        }
    }
}