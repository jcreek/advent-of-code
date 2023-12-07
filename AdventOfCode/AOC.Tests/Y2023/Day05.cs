namespace AOC.Tests.Y2023;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day05
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private record Map
    {
        public long DestinationRangeStart { get; set; }
        public long SourceRangeStart { get; set; }
        public long RangeLength { get; set; }
    }

    private record Almanac
    {
        public List<long> SeedsToBePlanted { get; set; }
        public List<Map> SeedToSoilMap { get; set; }
        public List<Map> SoilToFertiliserMap { get; set; }
        public List<Map> FertiliserToWaterMap { get; set; }
        public List<Map> WaterToLightMap { get; set; }
        public List<Map> LightToTemperatureMap { get; set; }
        public List<Map> TemperatureToHumidityMap { get; set; }
        public List<Map> HumidityToLocationMap { get; set; }
    }

    private List<Map> GetMap(string[] lines, string mapName)
    {
        List<Map> map = new();
        int lineNumber = 0;
        while (lines[lineNumber] != mapName)
        {
            lineNumber++;
        }

        lineNumber++;
        while (lineNumber < lines.Length && lines[lineNumber] != "")
        {
            string[] mapLine = lines[lineNumber].Split(" ");
            map.Add(new Map
            {
                DestinationRangeStart = long.Parse(mapLine[0]),
                SourceRangeStart = long.Parse(mapLine[1]),
                RangeLength = long.Parse(mapLine[2])
            });
            lineNumber++;
        }

        return map;
    }

    private long GetValueUsingMaps(List<Map> maps, long sourceValue)
    {
        // is the seed number within any of the ranges for the seed to soil map?
        if (maps.Any(m => m.SourceRangeStart + m.RangeLength >= sourceValue && sourceValue >= m.SourceRangeStart))
        {
            Map appropriateMap = maps
                .OrderBy(m => m.DestinationRangeStart)
                .First(m =>
                    m.SourceRangeStart + m.RangeLength >= sourceValue && sourceValue >= m.SourceRangeStart);

            // Calculate how far to move from the start of the source range to our seed number
            long increment = sourceValue - appropriateMap.SourceRangeStart;
            return appropriateMap.DestinationRangeStart + increment;
        }

        // Just use the same value as the source
        return sourceValue;
    }


    [TestCase(@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4", 35)]
    [TestCase(null, 265018614)] // The actual answer
    public void Part1(string input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;
        Almanac almanac = new();

        // Get the seeds from the first line
        almanac.SeedsToBePlanted = lines[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();

        // Get the maps when we don't know what line number they occur on
        almanac.SeedToSoilMap = GetMap(lines, "seed-to-soil map:");
        almanac.SoilToFertiliserMap = GetMap(lines, "soil-to-fertilizer map:");
        almanac.FertiliserToWaterMap = GetMap(lines, "fertilizer-to-water map:");
        almanac.WaterToLightMap = GetMap(lines, "water-to-light map:");
        almanac.LightToTemperatureMap = GetMap(lines, "light-to-temperature map:");
        almanac.TemperatureToHumidityMap = GetMap(lines, "temperature-to-humidity map:");
        almanac.HumidityToLocationMap = GetMap(lines, "humidity-to-location map:");

        List<long> locations = new();

        foreach (long seed in almanac.SeedsToBePlanted)
        {
            // Get the soil, fertilizer, water, light, temperature, humidity, and location
            long soil = GetValueUsingMaps(almanac.SeedToSoilMap, seed);
            long fertiliser = GetValueUsingMaps(almanac.SoilToFertiliserMap, soil);
            long water = GetValueUsingMaps(almanac.FertiliserToWaterMap, fertiliser);
            long light = GetValueUsingMaps(almanac.WaterToLightMap, water);
            long temperature = GetValueUsingMaps(almanac.LightToTemperatureMap, light);
            long humidity = GetValueUsingMaps(almanac.TemperatureToHumidityMap, temperature);
            long location = GetValueUsingMaps(almanac.HumidityToLocationMap, humidity);

            Console.WriteLine($"Seed {seed} grows in location {location}");
            locations.Add(location);
        }

        long result = locations.Min();

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }


    [TestCase(@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4", 46)]
    [TestCase(null, 63179500)] // The actual answer
    public void Part2(string input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;
        Almanac almanac = new();

        // Get the maps when we don't know what line number they occur on
        almanac.SeedToSoilMap = GetMap(lines, "seed-to-soil map:");
        almanac.SoilToFertiliserMap = GetMap(lines, "soil-to-fertilizer map:");
        almanac.FertiliserToWaterMap = GetMap(lines, "fertilizer-to-water map:");
        almanac.WaterToLightMap = GetMap(lines, "water-to-light map:");
        almanac.LightToTemperatureMap = GetMap(lines, "light-to-temperature map:");
        almanac.TemperatureToHumidityMap = GetMap(lines, "temperature-to-humidity map:");
        almanac.HumidityToLocationMap = GetMap(lines, "humidity-to-location map:");

        long result = long.MaxValue;
        object lockObj = new();

        List<long> values = lines[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();

        Parallel.For(0, values.Count / 2, i =>
        {
            long startRange = values[i * 2];
            long rangeLength = values[i * 2 + 1];

            for (long seed = startRange; seed < startRange + rangeLength; seed++)
            {
                // Get the soil, fertilizer, water, light, temperature, humidity, and location
                long soil = GetValueUsingMaps(almanac.SeedToSoilMap, seed);
                long fertiliser = GetValueUsingMaps(almanac.SoilToFertiliserMap, soil);
                long water = GetValueUsingMaps(almanac.FertiliserToWaterMap, fertiliser);
                long light = GetValueUsingMaps(almanac.WaterToLightMap, water);
                long temperature = GetValueUsingMaps(almanac.LightToTemperatureMap, light);
                long humidity = GetValueUsingMaps(almanac.TemperatureToHumidityMap, temperature);
                long location = GetValueUsingMaps(almanac.HumidityToLocationMap, humidity);

                lock (lockObj)
                {
                    if (location < result)
                    {
                        result = location;
                    }
                }
            }
        });

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
