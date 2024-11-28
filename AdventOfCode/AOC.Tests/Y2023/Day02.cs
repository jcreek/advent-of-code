using System.Reflection;

namespace AOC.Tests.Y2023
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day02
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data", $"{GetThisClassName()}.dat"));
        }
        
        private record Reveal
        {
            public int RedCount { get; set; }
            public int GreenCount { get; set; }
            public int BlueCount { get; set; }
        };
        
        private record Game
        {
            public int Id { get; set; }
            public List<Reveal> Reveals { get; set; }
            public int MinimumReds { get; set; }
            public int MinimumGreens { get; set; }
            public int MinimumBlues { get; set; }
            public int Power { get; set; }
        };

        [TestCase(@"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 8)]
        [TestCase(null, 1734)] // The actual answer
        public void Part1(string? input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            List<Game> games = new();

            foreach (string line in lines)
            {
                // Split up the string "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
                
                // Get the number after "Game " and before ":"
                int id = int.Parse(line.Substring(5, line.IndexOf(':') - 5));

                // Get each set of reveals, separated by ";"
                string[] reveals = line.Substring(line.IndexOf(':') + 2).Split("; ");

                // For each reveal, get the number of each color
                List<Reveal> revealList = new();
                foreach (string reveal in reveals)
                {
                    Reveal r = new();
                    string[] colors = reveal.Split(", ");
                    foreach (string color in colors)
                    {
                        if (color.Contains("red"))
                        {
                            r.RedCount = int.Parse(color.Substring(0, color.IndexOf(' ')));
                        }
                        else if (color.Contains("green"))
                        {
                            r.GreenCount = int.Parse(color.Substring(0, color.IndexOf(' ')));
                        }
                        else if (color.Contains("blue"))
                        {
                            r.BlueCount = int.Parse(color.Substring(0, color.IndexOf(' ')));
                        }
                    }
                    revealList.Add(r);
                }

                // Add the game to the list
                games.Add(new Game
                {
                    Id = id,
                    Reveals = revealList,
                });
            }

            // Remove any games with more than 12 red cubes, or 13 green cubes, or 14 blue cubes
            games.RemoveAll(g => g.Reveals.Any(r => r.RedCount > 12 || r.GreenCount > 13 || r.BlueCount > 14));
            
            // Add up the ids of the remaining games
            int result = games.Sum(g => g.Id);

            if (expected != null)
            {
               Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        [TestCase(@"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 2286)]
        [TestCase(null, 70387)] // The actual answer
        public void Part2(string? input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;
            
            List<Game> games = new();

            foreach (string line in lines)
            {
                // Split up the string "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
                
                // Get the number after "Game " and before ":"
                int id = int.Parse(line.Substring(5, line.IndexOf(':') - 5));

                // Get each set of reveals, separated by ";"
                string[] reveals = line.Substring(line.IndexOf(':') + 2).Split("; ");

                // For each reveal, get the number of each color
                List<Reveal> revealList = new();
                foreach (string reveal in reveals)
                {
                    Reveal r = new();
                    string[] colors = reveal.Split(", ");
                    foreach (string color in colors)
                    {
                        if (color.Contains("red"))
                        {
                            r.RedCount = int.Parse(color.Substring(0, color.IndexOf(' ')));
                        }
                        else if (color.Contains("green"))
                        {
                            r.GreenCount = int.Parse(color.Substring(0, color.IndexOf(' ')));
                        }
                        else if (color.Contains("blue"))
                        {
                            r.BlueCount = int.Parse(color.Substring(0, color.IndexOf(' ')));
                        }
                    }
                    revealList.Add(r);
                }
                
                // Get the minimum number of each colour required for each game (i,e. the highest amount of each colour in any reveal
                int minimumReds = revealList.Max(r => r.RedCount);
                int minimumGreens = revealList.Max(r => r.GreenCount);
                int minimumBlues = revealList.Max(r => r.BlueCount);

                int power = minimumReds * minimumGreens * minimumBlues;
                

                // Add the game to the list
                games.Add(new Game
                {
                    Id = id,
                    Reveals = revealList,
                    MinimumReds = minimumReds,
                    MinimumGreens = minimumGreens,
                    MinimumBlues = minimumBlues,
                    Power = power,
                });
            }

            int result = games.Sum(g => g.Power);
            
            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }
    }
}
