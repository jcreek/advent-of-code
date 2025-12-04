using AOC.Helpers;

namespace AOC.Tests.Y2025;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day04
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2025", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.", 13)]
    [TestCase(null, 1451)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        char[,] array = TwoDimensionalArrays.Make2DArrayFromStringArray(lines);

        int rollsOfPaperThatCanBeAccessed = 0;

        foreach ((int row, int column, char value) in TwoDimensionalArrays.Cells(array))
        {
            if (value != '@')
            {
                continue;
            }

            List<(int, int)> positionsWherePaperExists =
                TwoDimensionalArrays.CheckAllSurroundingCellsForCharacter(array, row, column, '@');

            if (positionsWherePaperExists.Count < 4)
            {
                rollsOfPaperThatCanBeAccessed++;
            }
        }

        int result = rollsOfPaperThatCanBeAccessed;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.", 43)]
    [TestCase(null, 8701)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        //string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        char[,] array = TwoDimensionalArrays.Make2DArrayFromStringArray(lines);

        int totalRollsOfPaperRemoved = 0;
        int rollsOfPaperAccessed = 0;

        bool continueLoop = true;

        while (continueLoop)
        {
            List<(int, int)> positionsToRemovePaper = new();
            foreach ((int row, int column, char value) in TwoDimensionalArrays.Cells(array))
            {
                if (value != '@')
                {
                    continue;
                }

                List<(int, int)> positionsWherePaperExists =
                    TwoDimensionalArrays.CheckAllSurroundingCellsForCharacter(array, row, column, '@');

                if (positionsWherePaperExists.Count < 4)
                {
                    totalRollsOfPaperRemoved++;
                    rollsOfPaperAccessed++;
                    positionsToRemovePaper.Add((row, column));
                }
            }

            if (rollsOfPaperAccessed == 0)
            {
                continueLoop = false;
            }

            rollsOfPaperAccessed = 0;

            // Remove each roll
            foreach ((int, int) position in positionsToRemovePaper)
            {
                array[position.Item1, position.Item2] = 'x';
            }

            positionsToRemovePaper.Clear();
        }

        int result = totalRollsOfPaperRemoved;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
