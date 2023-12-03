using System.Reflection;

namespace AOC.Tests.Y2023
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day03
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2023", "Data", $"{GetThisClassName()}.dat"));
        }

        private bool CheckSurroundingCellsForSymbols(char[,] grid, List<(int, int)> coords, bool isTesting = false)
        {
            char[,] grid2 = new char[grid.GetLength(0), grid.GetLength(1)];
            foreach (var coord in coords)
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
                Console.WriteLine($"Line {i+1}");
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
                                bool isValidPartNumber = CheckSurroundingCellsForSymbols(grid, numberCoords, false);
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
        
        public bool CheckSurroundingCellsForNumbers(char[,] grid, List<(int, int)> coords, bool isTesting = false)
        {
            foreach (var gearCoord in coords)
            {
                List<(int, int)> numberCoords = new();
                int x = gearCoord.Item1;
                int y = gearCoord.Item2;
                
                // Check the eight surrounding cells of each digit in the number.
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

                if (numberCoords.Count == 2)
                {
                    int number1 = int.Parse(grid[numberCoords[0].Item1, numberCoords[0].Item2].ToString());
                    int number2 = int.Parse(grid[numberCoords[1].Item1, numberCoords[1].Item2].ToString());
                    int gearRatio = number1 * number2;
                    // gearRatios.Add(gearRatio);
                }
            }
            return false;
        }

        
        private List<int> GetAllGearRatios(string[] lines)
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

            List<int> gearRatios = new();
            List<(int, int)> gearCoords = new();
            
            // Iterate over each cell in the grid to find all gears
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine($"Line {i+1}");
                for (int j = 0; j < cols; j++)
                {
                    

                    if (grid[i, j] == '*')
                    {
                        gearCoords.Add((i, j));
                    }
                }
            }
            
            // Now find all instances where exactly two numbers are adjacent to a gear
            

            // Return the list of numbers.
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
                (0,0),
                (0,1),
                (0,2),
                (0,5),
                (0,6),
                (0,7),
                (2,2),
                (2,3),
                (2,6),
                (2,7),
                (2,8),
                (4,0),
                (4,1),
                (4,2),
                (5,7),
                (5,8),
                (6,2),
                (6,3),
                (6,4),
                (7,6),
                (7,7),
                (7,8),
                (9,1),
                (9,2),
                (9,3),
                (9,5),
                (9,6),
                (9,7),
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
        public void Part1(string input, int? expected)
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
        [TestCase(null, 1783)] // The actual answer
        public void Part2(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;
            
            // A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.
            List<int> gearRatios = GetAllGearRatios(lines);

            // The result is the sum of all the gear ratios
            int result = gearRatios.Sum();

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }
    }
}