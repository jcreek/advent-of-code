using System.Text;

namespace AOC.Tests.Y2025;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day02
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2025", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private List<long> FindInvalidIds(long firstId, long lastId)
    {
        List<long> invalidIds = new();
        // Since the young Elf was just doing silly patterns, you can find the invalid IDs by looking for any ID which is
        // made only of some sequence of digits repeated twice. So, 55 (5 twice), 6464 (64 twice), and 123123 (123 twice)
        // would all be invalid IDs.
        // None of the numbers have leading zeroes; 0101 isn't an ID at all. (101 is a valid ID that you would ignore.)

        for (long i = firstId; i <= lastId; i++)
        {
            string numberString = i.ToString();

            // split the string in the middle, and if the two halves are identical then add i to invalidIds
            int middlePosition = numberString.Length / 2;
            string leftString = numberString.Substring(0, middlePosition);
            string rightString = numberString.Substring(middlePosition);

            if (leftString == rightString)
            {
                invalidIds.Add(i);
            }
        }

        return invalidIds;
    }

    private static long FindRepeatingPattern(string numberString)
    {
        int digitsCount = numberString.Length;

        for (int k = 1; k <= digitsCount / 2; k++)
        {
            if (digitsCount % k != 0)
            {
                continue; // can't evenly repeat so escape early
            }

            string pattern = numberString.Substring(0, k);
            long times = digitsCount / k;

            StringBuilder repeated = new(digitsCount);
            for (long i = 0; i < times; i++)
            {
                repeated.Append(pattern);
            }

            if (repeated.ToString() == numberString)
            {
                return times;
            }
        }

        // No repeating pattern found
        return 1;
    }

    private List<long> FindInvalidIdsPart2(long firstId, long lastId)
    {
        List<long> invalidIds = new();

        // Now, an ID is invalid if it is made only of some sequence of digits repeated at least twice. So, 12341234 (1234 two times),
        // 123123123 (123 three times), 1212121212 (12 five times), and 1111111 (1 seven times) are all invalid IDs.

        for (long i = firstId; i <= lastId; i++)
        {
            string numberString = i.ToString();
            long count = FindRepeatingPattern(numberString);

            if (count > 1)
            {
                invalidIds.Add(i);
            }
        }

        return invalidIds;
    }

    [TestCase(
        "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124",
        1227775554)]
    [TestCase(null, 23534117921)] // The actual answer
    public void Part1(string? input, long? expected)
    {
        string[] lines = input != null ? new[] { input } : realData;
        // string[] lines = input != null ? input.Split("\n") : realData;

        List<long> invalidIds = new();

        // Split by comma to get each range
        string[] ranges = lines[0].Split(',');
        foreach (string range in ranges)
        {
            // Split by - to get the first and last ID in the range
            long firstId = long.Parse(range.Split('-')[0]);
            long lastId = long.Parse(range.Split('-')[1]);

            List<long> invalidIdsInRange = FindInvalidIds(firstId, lastId);
            invalidIds.AddRange(invalidIdsInRange);
        }

        long result = invalidIds.Sum();

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(
        "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124",
        (long)4174379265)]
    [TestCase(null, 31755323497)] // The actual answer
    public void Part2(string? input, long? expected)
    {
        string[] lines = input != null ? new[] { input } : realData;
        // string[] lines = input != null ? input.Split("\n") : realData;

        List<long> invalidIds = new();

        // Split by comma to get each range
        string[] ranges = lines[0].Split(',');
        foreach (string range in ranges)
        {
            // Split by - to get the first and last ID in the range
            long firstId = long.Parse(range.Split('-')[0]);
            long lastId = long.Parse(range.Split('-')[1]);

            List<long> invalidIdsInRange = FindInvalidIdsPart2(firstId, lastId);
            invalidIds.AddRange(invalidIdsInRange);
        }

        long result = invalidIds.Sum();

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
