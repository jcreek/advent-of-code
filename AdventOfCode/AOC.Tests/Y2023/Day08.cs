namespace AOC.Tests.Y2023;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day08
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)", 2)]
    [TestCase(@"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)", 6)]
    [TestCase(null, 19199)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        // The first line is a looping list of instructions
        List<char> instructions = lines[0].ToCharArray().ToList();

        // Every other line (except the second blank line) takes the form "AAA = (BBB, CCC)" where AAA, BBB, and CCC are all strings representing locations, AAA is the current location, and BBB and CCC are the two possible next locations, BBB if the user moves L and CCC if the user moves R
        Dictionary<string, (string, string)> locations = new();
        foreach (string line in lines.Skip(2))
        {
            string[] parts = line.Split(" = ");
            Console.WriteLine($"{parts[0]}, {parts[1]}");
            string location = parts[0];
            string[] nextLocations = parts[1].Trim('(', ')').Split(", ");
            locations.Add(location, (nextLocations[0], nextLocations[1]));
        }

        // The user starts at the first location and follows the instructions, looping until they reach location ZZZ
        string currentLocation = "AAA";
        int instructionIndex = 0;
        int stepsCounter = 0;
        while (currentLocation != "ZZZ")
        {
            char instruction = instructions[instructionIndex];
            (string left, string right) = locations[currentLocation];
            currentLocation = instruction switch
            {
                'L' => left,
                'R' => right,
                _ => throw new Exception($"Unknown instruction: {instruction}")
            };

            // If we are already at the last instructionIndex, start again from 0, otherwise move to the next index
            instructionIndex = instructionIndex == instructions.Count - 1 ? 0 : instructionIndex + 1;

            stepsCounter++;
        }

        int result = stepsCounter;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)", 6)]
    [TestCase(null, 1783)] // The actual answer
    public async Task Part2(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        // The first line is a looping list of instructions
        List<char> instructions = lines[0].ToCharArray().ToList();

        // Every other line (except the second blank line) takes the form "11A = (11B, XXX)" where A11A, 11B, and XXX are all strings representing locations, 11A is the current location, and 11B and XXX are the two possible next locations, 11B if the user moves L and XXX if the user moves R
        Dictionary<string, (string, string)> locations = new();
        foreach (string line in lines.Skip(2))
        {
            string[] parts = line.Split(" = ");
            string location = parts[0];
            string[] nextLocations = parts[1].Trim('(', ')').Split(", ");
            locations.Add(location, (nextLocations[0], nextLocations[1]));
        }

        // The user simultaneously starts at all locations that end with "A" and follow all of the paths at the same time until they all simultaneously end up at nodes that end with "Z"
        List<string> currentLocations = locations.Keys.Where(k => k.EndsWith("A")).ToList();

        // Print out all the starting locations from the currentLocations list as a single string comma separated
        Console.WriteLine($"Starting locations: {string.Join(", ", currentLocations)}");

        int instructionIndex = 0;
        int stepsCounter = 0;
        while (currentLocations.Any(l => !l.EndsWith("Z")))
        {
            // Sort in parallel for large lists
            Task[] tasks = new Task[currentLocations.Count];

            // For each location, follow the instruction
            for (int i = 0; i < currentLocations.Count; i++)
            {
                int localI = i; // Local copy of the loop variable
                char instruction = instructions[instructionIndex];
                tasks[localI] = Task.Factory.StartNew(() =>
                {
                    (string left, string right) = locations[currentLocations[localI]];
                    string newLocation = instruction switch
                    {
                        'L' => left,
                        'R' => right,
                        _ => throw new Exception($"Unknown instruction: {instruction}")
                    };

                    // Commented out due to ballooning memory
                    // Console.WriteLine($"Moved {instruction} from {currentLocations[localI]} to {newLocation}");

                    // Update the current location
                    currentLocations[localI] = newLocation;
                });
            }

            await Task.WhenAll(tasks);

            // If we are already at the last instructionIndex, start again from 0, otherwise move to the next index
            instructionIndex = instructionIndex == instructions.Count - 1 ? 0 : instructionIndex + 1;

            stepsCounter++;
        }

        int result = stepsCounter;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
