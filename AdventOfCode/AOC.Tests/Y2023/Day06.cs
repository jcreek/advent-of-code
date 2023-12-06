namespace AOC.Tests.Y2023;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day06
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"Time:      7  15   30
Distance:  9  40  200", 288)]
    [TestCase(null, 1660968)] // The actual answer
    public void Part1(string input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        List<int> times = lines[0].Split(" ").Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList();
        List<int> distances = lines[1].Split(" ").Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList();
        List<int> waysToBeatMaxDistance = new();
        // populate waysToBeatMaxDistance with 0s for the same list length as times
        for (int i = 0; i < times.Count; i++)
        {
            waysToBeatMaxDistance.Add(0);
        }

        int startingSpeedInMilimetersPerSecond = 0;
        int amountToIncreaseSpeedPerSecondHoldingTheButtonInMilimetersPerSecond = 1;

        // for each race, determine how many different ways you can beat the maximum distance
        for (int i = 0; i < times.Count; i++)
        {
            int raceTime = times[i];
            int distanceToBeat = distances[i];

            for (int j = 1; j < raceTime; j++)
            {
                // Hold the button for j seconds
                int speed = startingSpeedInMilimetersPerSecond +
                            amountToIncreaseSpeedPerSecondHoldingTheButtonInMilimetersPerSecond * j;

                // Calculate the distance traveled
                int distanceTraveled = speed * (raceTime - j);

                // If the distance traveled is greater than the distance to beat, update the count of ways to beat the max distance
                if (distanceTraveled > distanceToBeat)
                {
                    Console.WriteLine(
                        $"Can beat race {i} by holding the button for {j} seconds to travel {distanceTraveled}");
                    waysToBeatMaxDistance[i] += 1;
                }
            }
        }

        int result = 0;
        for (int i = 0; i < times.Count; i++)
        {
            if (result == 0)
            {
                result = waysToBeatMaxDistance[i];
            }
            else
            {
                result *= waysToBeatMaxDistance[i];
            }
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"Time:      7  15   30
Distance:  9  40  200", 71503)]
    [TestCase(null, 26499773)] // The actual answer
    public void Part2(string input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        string realTime = lines[0].Replace("Time:", "").Replace(" ", "");
        string realDistance = lines[1].Replace("Distance:", "").Replace(" ", "");


        List<long> times = new() { long.Parse(realTime) };
        List<long> distances = new() { long.Parse(realDistance) };
        List<int> waysToBeatMaxDistance = new();
        // populate waysToBeatMaxDistance with 0s for the same list length as times
        for (int i = 0; i < times.Count; i++)
        {
            waysToBeatMaxDistance.Add(0);
        }

        int startingSpeedInMilimetersPerSecond = 0;
        int amountToIncreaseSpeedPerSecondHoldingTheButtonInMilimetersPerSecond = 1;

        // for each race, determine how many different ways you can beat the maximum distance
        for (int i = 0; i < times.Count; i++)
        {
            long raceTime = times[i];
            long distanceToBeat = distances[i];

            for (long j = 1; j < raceTime; j++)
            {
                // Hold the button for j seconds
                long speed = startingSpeedInMilimetersPerSecond +
                             amountToIncreaseSpeedPerSecondHoldingTheButtonInMilimetersPerSecond * j;

                // Calculate the distance traveled
                long distanceTraveled = speed * (raceTime - j);

                // If the distance traveled is greater than the distance to beat, update the count of ways to beat the max distance
                if (distanceTraveled > distanceToBeat)
                {
                    // Console.WriteLine($"Can beat race {i} by holding the button for {j} seconds to travel {distanceTraveled}");
                    waysToBeatMaxDistance[i] += 1;
                }
            }
        }

        long result = 0;
        for (int i = 0; i < times.Count; i++)
        {
            if (result == 0)
            {
                result = waysToBeatMaxDistance[i];
            }
            else
            {
                result *= waysToBeatMaxDistance[i];
            }
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
