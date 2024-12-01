namespace AOC.Tests.Y2023;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day03
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private bool CheckSurroundingCellsForSymbols(char[,] grid, List<(int, int)> coords, bool isTesting = false)
    {
        char[,] grid2 = new char[grid.GetLength(0), grid.GetLength(1)];
        foreach ((int, int) coord in coords)
        {
            int x = coord.Item1;
            int y = coord.Item2;

            grid2[x, y] = grid[x, y];

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1) && !coords.Contains((i, j)))
                    {
                        if (isTesting)
                        {
                            // Console.WriteLine($"{i},{j}");
                            grid2[i, j] = 'b';
                        }

                        char cell = grid[i, j];
                        if (!char.IsDigit(cell) && cell != '.')
                        {
                            if (isTesting)
                            {
                                grid2[i, j] = 'a';
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        if (isTesting)
        {
            // Print out the grid2
            for (int i = 0; i < grid2.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < grid2.GetLength(1); j++)
                {
                    line += grid2[i, j] != '\u0000' ? grid2[i, j] : '.';
                }

                Console.WriteLine(line);
            }
        }

        return false;
    }

    private List<int> GetAllValidPartNumbers(string[] lines)
    {
        // Create a 2D array to represent the grid.
        int rows = lines.Length;
        int cols = lines[0].Length;
        char[,] grid = new char[rows, cols];

        // Populate the grid
        for (int i = 0; i < rows; i++)
        {
            string line = lines[i];
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = line[j];
            }
        }

        List<int> numbers = new();

        // Iterate over each cell in the grid.
        for (int i = 0; i < rows; i++)
        {
            Console.WriteLine($"Line {i + 1}");
            for (int j = 0; j < cols; j++)
            {
                List<(int, int)> numberCoords = new();

                // If the cell contains a digit and it's the start of a number (either the left cell is not a digit or it's the left boundary), start collecting the digits to form a number until you reach a cell that's not a digit.
                if (char.IsDigit(grid[i, j]) && (j == 0 || !char.IsDigit(grid[i, j - 1])))
                {
                    // Collect the digits to form a number until you reach a cell that's not a digit.
                    string number = "";

                    while (char.IsDigit(grid[i, j]))
                    {
                        number += grid[i, j];
                        numberCoords.Add((i, j));
                        j++;
                        if (j >= cols)
                        {
                            break;
                        }
                    }

                    // If the number is valid, add it to a list.
                    if (number.Length >= 1)
                    {
                        int formattedNumber = int.Parse(number);

                        Console.WriteLine(formattedNumber);

                        // Check the eight surrounding cells of each digit in the number.
                        if (numberCoords.Count > 0)
                        {
                            bool isValidPartNumber = CheckSurroundingCellsForSymbols(grid, numberCoords);
                            // If any of the surrounding cells contain a symbol, add the number to a list.
                            if (isValidPartNumber)
                            {
                                Console.WriteLine("Valid part number");
                                numbers.Add(formattedNumber);
                            }
                        }
                    }
                }
            }
        }

        // Return the list of numbers.
        return numbers;
    }

    private int CountNumbers(List<(int, int)> numberCoords)
    {
        // Group coordinates by the X-axis and sort each group by the Y-axis
        IEnumerable<IOrderedEnumerable<(int, int)>> groupedCoords = numberCoords.GroupBy(coord => coord.Item1)
            .Select(group => group.OrderBy(coord => coord.Item2));

        int numberCount = 0;
        foreach (IOrderedEnumerable<(int, int)> group in groupedCoords)
        {
            int lastY = int.MinValue;
            foreach ((int, int) coord in group)
            {
                // If there is a gap in the Y-axis, increment the number count
                if (coord.Item2 - lastY > 1)
                {
                    numberCount++;
                }

                lastY = coord.Item2;
            }
        }

        return numberCount;
    }

    private List<int> ExtractNumbers(List<(int, int)> numberCoords, char[,] grid)
    {
        // Initialize a list to hold the extracted numbers
        List<int> numbers = new();

        // Sort the coordinates by Y-axis (Item2) then by X-axis (Item1)
        List<(int, int)> sortedCoords =
            numberCoords.OrderBy(coord => coord.Item2).ThenBy(coord => coord.Item1).ToList();

        // Create a HashSet to keep track of processed coordinates
        HashSet<(int, int)> processedCoords = new();

        foreach ((int x, int y) in sortedCoords)
        {
            // Skip this coordinate if it has already been processed
            if (processedCoords.Contains((x, y)))
            {
                continue;
            }

            // Start from the current coordinate and scan to the left until a non-digit is found or it reaches the beginning of the row
            int startX = x;
            while (startX > 0 && char.IsDigit(grid[startX - 1, y]))
            {
                startX--;
            }

            // Build the number by scanning to the right from the startX position
            string currentNumberStr = "";
            int currentX = startX;
            while (currentX < grid.GetLength(0) && char.IsDigit(grid[currentX, y]))
            {
                currentNumberStr += grid[currentX, y];
                // Mark the coordinate as processed
                processedCoords.Add((currentX, y));
                currentX++;
            }

            // If a number is formed, add it to the list
            if (currentNumberStr.Length > 0)
            {
                numbers.Add(int.Parse(currentNumberStr));
            }
        }

        // Return the list of extracted numbers
        return numbers;
    }


    private int GetGearRatioFromExactlyTwoNumbersInSurroundingCells(char[,] grid, (int, int) gearCoord)
    {
        List<(int, int)> numberCoords = new();
        int x = gearCoord.Item1;
        int y = gearCoord.Item2;
        int gearRatio = 0;

        // Check the eight surrounding cells of the gear 
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1) && (i != x || j != y))
                {
                    if (char.IsDigit(grid[i, j]))
                    {
                        numberCoords.Add((i, j));
                    }
                }
            }
        }

        // Consider each set of cells in the same x axis with a digit to their immediate right as a single number
        foreach ((int, int) numberCoord in numberCoords)
        {
            Console.WriteLine($"{numberCoord.Item1},{numberCoord.Item2}");
        }

        Console.WriteLine("---");

        List<int> numbers = ExtractNumbers(numberCoords, grid);

        foreach (int number in numbers)
        {
            Console.WriteLine($"Number: {number}");
        }

        Console.WriteLine("===");

        if (numbers.Count == 2)
        {
            // If there are exactly two numbers around the gear, generate the gear ratio
            gearRatio = numbers[0] * numbers[1];
            Console.WriteLine($"{numbers[0]} * {numbers[1]} = {gearRatio}");
        }

        return gearRatio;
    }


    private int GetSumOfAllGearRatios(string[] lines)
    {
        // Create a 2D array to represent the grid.
        int cols = lines[0].Length;
        int rows = lines.Length;
        char[,] grid = new char[cols, rows];

        // Populate the grid
        for (int j = 0; j < rows; j++)
        {
            string line = lines[j];
            for (int i = 0; i < cols; i++)
            {
                grid[i, j] = line[i];
            }
        }

        List<(int, int)> gearCoords = new();

        // Iterate over each cell in the grid to find all gears
        for (int i = 0; i < cols; i++)
        {
            // Console.WriteLine($"Line {i + 1}");
            for (int j = 0; j < rows; j++)
            {
                if (grid[i, j] == '*')
                {
                    gearCoords.Add((i, j));
                }
            }
        }

        int gearRatios = 0;
        // Now find all instances where exactly two numbers are adjacent to a gear and sum their gear ratios
        foreach ((int, int) gear in gearCoords)
        {
            gearRatios += GetGearRatioFromExactlyTwoNumbersInSurroundingCells(grid, gear);
        }

        return gearRatios;
    }


    [TestCase("blah", 1)]
    public void TestCheckSurroundingCellsForSymbolsFunctionWorksCorrectly(string input, int? expected)
    {
        string exampleSchematic = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";
        string[] lines = exampleSchematic.Split("\n");
        int rows = lines.Length;
        int cols = lines[0].Length;
        char[,] grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string line = lines[i];
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = line[j];
            }
        }

        List<(int, int)> coords = new()
        {
            (0, 0),
            (0, 1),
            (0, 2),
            (0, 5),
            (0, 6),
            (0, 7),
            (2, 2),
            (2, 3),
            (2, 6),
            (2, 7),
            (2, 8),
            (4, 0),
            (4, 1),
            (4, 2),
            (5, 7),
            (5, 8),
            (6, 2),
            (6, 3),
            (6, 4),
            (7, 6),
            (7, 7),
            (7, 8),
            (9, 1),
            (9, 2),
            (9, 3),
            (9, 5),
            (9, 6),
            (9, 7)
        };

        CheckSurroundingCellsForSymbols(grid, coords, true);

        //if (expected != null)
        //{
        //    Assert.That(result, Is.EqualTo(expected.Value));
        //}

        //Console.WriteLine($"Part 2: {result}");
    }

    [TestCase(@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..", 4361)]
    [TestCase(null, 550934)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        // any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)
        List<int> validPartNumbers = GetAllValidPartNumbers(lines);

        // The result is the sum of all the valid part numbers
        int result = validPartNumbers.Sum();

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..", 467835)]
    [TestCase(null, 81997870)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        // A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.
        int gearRatiosSum = GetSumOfAllGearRatios(lines);

        // The result is the sum of all the gear ratios
        int result = gearRatiosSum;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
