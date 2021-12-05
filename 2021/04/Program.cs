using Creek.HelpfulExtensions;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            // first line is the order in which to draw numbers, comma separated
            List<int> numbersToDraw = lines[0].Split(',').Select(int.Parse).ToList();

            // second line is blank line

            // 5 groups of 5 lines, space separated numbers in rows
            List<(int value, bool isMatched)[,]> grids = new List<(int value, bool isMatched)[,]>();

            // Skip the first line and second blank line, then go ahead 5 lines + the blank line each time 
            // repeat for additional groups and blank lines
            for (int i = 2; i < lines.Length; i+=6)
            {
                (int value, bool isMatched)[,] grid = new(int value, bool isMatched) [5,5];

                for (int j = 0; j < 5; j++)
                {
                    // Get all in each row, space separated + remove starting spaces and double spaces
                    string line = lines[i + j].StartsWith(" ") ? lines[i + j].Substring(1).Replace("  ", " ") : lines[i + j].Replace("  ", " ");
                    List<int> row = line.Split(' ').Select(int.Parse).ToList();

                    for (int k = 0; k < row.Count(); k++)
                    {
                        grid[j,k] = (row[k], false);
                    }
                }

                grids.Add(grid);
            }


            //Part1(numbersToDraw, grids);
            Part2(numbersToDraw, grids);
        }

        static void Part1(List<int> numbersToDraw, List<(int value, bool isMatched)[,]> grids) 
        {
            int winningNumber = 0;
            (int value, bool isMatched)[,] winningGrid = new(int value, bool isMatched) [5,5];

            // Start going through the numbers now the grids are set up
            foreach (int number in numbersToDraw)
            {
                bool breakLoops = false;

                // need to be able to record when each number in a grid has been matched
                foreach (var grid in grids)
                {
                    RecordAMatchInAGrid(grid, number);
                }

                // need to be able to record when a row OR column has had all numbers matched 
                foreach (var grid in grids)
                {
                    // when one grid has a complete row or column, that  grid wins 
                    if (ARowOrColumnHasAllNumbersMatched(grid))
                    {
                        Console.WriteLine($"Winning number: {number}");
                        // Exit the two foreach loops
                        // and record the final number called for the win
                        winningNumber = number;
                        winningGrid = grid;

                        breakLoops = true;
                        break;
                    }
                }

                if (breakLoops)
                {
                    break;
                }
            }
            
            // find the sum of all unmatched numbers of the board 
            int sumOfAllUnmatchedFromWinningBoard = CalculateSumOfAllUnmatchedFromWinningBoard(winningGrid);

            // multiply that by the final number that gave a grid the win
            Console.WriteLine(sumOfAllUnmatchedFromWinningBoard * winningNumber);
        }

        static void Part2(List<int> numbersToDraw, List<(int value, bool isMatched)[,]> grids)
        {
            // Figure out which board will win last

            

            int lastWinningNumber = 0;
            (int value, bool isMatched)[,] lastWinningGrid = new(int value, bool isMatched) [5,5];
            List<int> gridsThatHaveAlreadyWon = new List<int>();

            // Start going through the numbers now the grids are set up
            foreach (int number in numbersToDraw)
            {
                bool breakLoops = false;

                // need to be able to record when each number in a grid has been matched
                foreach (var grid in grids)
                {
                    RecordAMatchInAGrid(grid, number);
                }

                // need to be able to record when a row OR column has had all numbers matched 
                foreach (var (grid, index) in grids.WithIndex())
                {
                    // when one grid has a complete row or column, that  grid wins 
                    if (ARowOrColumnHasAllNumbersMatched(grid))
                    {
                        if (!gridsThatHaveAlreadyWon.Contains(index))
                        {
                            gridsThatHaveAlreadyWon.Add(index);
                        
                            if (gridsThatHaveAlreadyWon.Count == grids.Count)
                            {
                                // Exit the two foreach loops
                                // and record the final number called for the win
                                lastWinningNumber = number;
                                lastWinningGrid = grid;

                                breakLoops = true;
                                break;
                            }
                        }
                    }
                }

                if (breakLoops)
                {
                    break;
                }
            }
            
            // find the sum of all unmatched numbers of the board 
            int sumOfAllUnmatchedFromWinningBoard = CalculateSumOfAllUnmatchedFromWinningBoard(lastWinningGrid);

            // multiply that by the final number that gave a grid the win
            Console.WriteLine(sumOfAllUnmatchedFromWinningBoard * lastWinningNumber);
        }

        static int CalculateSumOfAllUnmatchedFromWinningBoard((int value, bool isMatched)[,] grid)
        {
            int runningTotal = 0; 

            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (!grid[i,k].isMatched)
                    {
                        runningTotal += grid[i,k].value;
                    }
                }
            }

            return runningTotal;
        }

        static void RecordAMatchInAGrid((int value, bool isMatched)[,] grid, int number)
        {
            // Go through all numbers in the grid and record any matches
            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (grid[i,k].value == number)
                    {
                        grid[i,k].isMatched = true;
                    }
                }
            }
        }
    
        static bool ARowOrColumnHasAllNumbersMatched((int value, bool isMatched)[,] grid)
        {
            for (int i = 0; i < 5; i++)
            {
                // Check the row, then the column
                if (grid[i,0].isMatched && grid[i,1].isMatched && grid[i,2].isMatched && grid[i,3].isMatched && grid[i,4].isMatched)
                {
                    return true;
                }
                else if (grid[0,i].isMatched && grid[1,i].isMatched && grid[2,i].isMatched && grid[3,i].isMatched && grid[4,i].isMatched)
                {
                    return true;
                }
            }
            return false;
        } 
    }
}
