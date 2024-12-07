using AOC.Tests.Helpers;

namespace AOC.Tests.Y2024;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day06
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2024", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private void TraverseGridAsGuard(ref char[,] grid, ref List<string> possibleExtraLoops)
    {
        // Traverse the grid as the guard, marking each cell as visited with an X
        // ^ means guard facing up, > means guard facing right, v means guard facing down, < means guard facing left

        // Find the guard
        int guardRow = -1;
        int guardCol = -1;

        for (int r = 0; r < grid.GetLength(0); r++)
        {
            for (int c = 0; c < grid.GetLength(1); c++)
            {
                if (grid[r, c] == '^' || grid[r, c] == '>' || grid[r, c] == 'v' || grid[r, c] == '<')
                {
                    guardRow = r;
                    guardCol = c;
                    break;
                }
            }

            if (guardRow != -1)
            {
                break;
            }
        }

        // Record the direction the guard is facing
        char guardDirection = grid[guardRow, guardCol];

        // Mark the initial cell as visited
        grid[guardRow, guardCol] = 'X';

        Console.WriteLine($"{guardCol},{guardRow} {guardDirection}");

        // int tempCounter = 0;
        // While the guard is still on the grid
        while (guardRow >= 0 && guardRow < grid.GetLength(0) && guardCol >= 0 && guardCol < grid.GetLength(1))
        {
            // Move the guard following the rules
            MoveGuard(ref grid, ref guardRow, ref guardCol, ref guardDirection, ref possibleExtraLoops);
            // ArraySearching.Print2DCharArray(grid);
            // Console.WriteLine();
            // Console.WriteLine($"{guardRow},{guardCol} {guardDirection}");
            // tempCounter++;
            // if (tempCounter > 100)
            // {
            //     break;
            // }
        }
    }

    private void SetGuardDirection(ref int moveX, ref int moveY, char guardDirection)
    {
        switch (guardDirection)
        {
            case '^':
                moveY = -1;
                break;
            case '>':
                moveX = 1;
                break;
            case 'v':
                moveY = 1;
                break;
            case '<':
                moveX = -1;
                break;
            default:
                throw new Exception("Invalid guard direction");
                break;
        }
    }

    private void MoveGuard(ref char[,] grid, ref int guardRow, ref int guardCol, ref char guardDirection,
        ref List<string> possibleExtraLoops)
    {
        // If there is something directly in front of you, turn right 90 degrees.
        // Otherwise, take a step forward.

        int moveX = 0;
        int moveY = 0;

        SetGuardDirection(ref moveX, ref moveY, guardDirection);

        try
        {
            // Check if there is something (#) directly in front of the guard
            char nextCell = grid[guardRow + moveY, guardCol + moveX];
            if (nextCell == '#')
            {
                // Turn right 90 degrees
                guardDirection = guardDirection switch
                {
                    '^' => '>',
                    '>' => 'v',
                    'v' => '<',
                    '<' => '^',
                    _ => throw new Exception("Invalid guard direction")
                };
            }
            else
            {
                // Move forward
                guardRow += moveY;
                guardCol += moveX;

                // Mark the cell as visited
                grid[guardRow, guardCol] = 'X';
            }

            // Now check to see if the next cell could be a new obstruction
            // The guard will end up in a loop if it visits the same cell twice and is facing the same direction due to a new hazard

            // If the guard is on a path towards a hazard and there is another previously visited hazard to one side then a new hazard could be placed to
            // force the guard to loop back to the previously visited hazard

            int potentialMoveX = 0;
            int potentialMoveY = 0;
            SetGuardDirection(ref potentialMoveX, ref potentialMoveY, guardDirection);

            int searchArea = potentialMoveX == 0 ? grid.GetLength(0) - guardRow : grid.GetLength(1) - guardCol;

            // Check if there is something (#) directly in front of the guard in the new direction with a previously visited cell in front of it
            for (int i = 0; i < searchArea; i++)
            {
                if (potentialMoveX == 0)
                {
                    // We are moving up or down
                    if (grid[guardRow + potentialMoveY, guardCol] == '#')
                    {
                        // We have found a hazard
                        // Check if there is a previously visited cell before it
                        int potentialMoveYPreviousCell = potentialMoveY > 0 ? guardRow + 1 : guardRow - 1;

                        if (grid[potentialMoveYPreviousCell, guardCol] == 'X')
                        {
                            // Mark the cell as a potential loop
                            Console.WriteLine($"Potential loop at {guardCol + moveX},{guardRow + moveY}");

                            // Add to possibleExtraLoops unless it is already there
                            string loop = $"{guardCol + moveX},{guardRow + moveY}";
                            if (!possibleExtraLoops.Contains(loop))
                            {
                                possibleExtraLoops.Add(loop);
                            }
                        }
                    }
                }
                else
                {
                    // We are moving sideways
                    if (grid[guardRow, guardCol + potentialMoveX] == '#')
                    {
                        // We have found a hazard
                        // Check if there is a previously visited cell before it
                        int potentialMoveXPreviousCell = potentialMoveX > 0 ? guardCol - 1 : guardCol + 1;

                        if (grid[guardRow, potentialMoveXPreviousCell] == 'X')
                        {
                            // We have found a potential loop
                            Console.WriteLine($"Potential loop at {guardCol + moveX},{guardRow + moveY}");

                            // Add to possibleExtraLoops unless it is already there
                            string loop = $"{guardCol + moveX},{guardRow + moveY}";
                            if (!possibleExtraLoops.Contains(loop))
                            {
                                possibleExtraLoops.Add(loop);
                            }
                        }
                    }
                }
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            // Move the guard out of the grid
            guardRow += moveY;
            guardCol += moveX;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }


    [TestCase(@"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...", 41)]
    [TestCase(null, 4711)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        char[,] grid = ArraySearching.ConvertListTo2DArray([.. lines]);

        List<string> possibleExtraLoops = new();

        TraverseGridAsGuard(ref grid, ref possibleExtraLoops);

        // Count how many cells the guard visited
        int result = 0;

        for (int r = 0; r < grid.GetLength(0); r++)
        {
            for (int c = 0; c < grid.GetLength(1); c++)
            {
                if (grid[r, c] == 'X')
                {
                    result++;
                }
            }
        }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...", 6)]
    [TestCase(null, 1783)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        char[,] grid = ArraySearching.ConvertListTo2DArray([.. lines]);

        List<string> possibleExtraLoops = new();

        TraverseGridAsGuard(ref grid, ref possibleExtraLoops);

        if (expected != null)
        {
            Assert.That(possibleExtraLoops.Count, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {possibleExtraLoops.Count}");
    }
}
