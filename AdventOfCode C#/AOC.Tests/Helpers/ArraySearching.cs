namespace AOC.Tests.Helpers;

/// <summary>
///     Provides utility methods for searching within 2D character arrays.
/// </summary>
public static class ArraySearching
{
    // Directions: Right, Left, Down, Up, Diagonal Down-Right, Diagonal Down-Left, Diagonal Up-Right, Diagonal Up-Left
    private static readonly (int RowOffset, int ColOffset)[] Directions = new[]
    {
        (0, 1), // Right
        (0, -1), // Left
        (1, 0), // Down
        (-1, 0), // Up
        (1, 1), // Diagonal down-right
        (1, -1), // Diagonal down-left
        (-1, 1), // Diagonal up-right
        (-1, -1) // Diagonal up-left
    };

    public static char[,] ConvertListTo2DArray(List<string> list)
    {
        int rows = list.Count;
        int cols = list[0].Length;
        char[,] grid = new char[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            string row = list[r];
            for (int c = 0; c < cols; c++)
            {
                grid[r, c] = row[c];
            }
        }

        return grid;
    }

    public static void Print2DCharArray(char[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++) // Iterate rows
        {
            for (int j = 0; j < array.GetLength(1); j++) // Iterate columns
            {
                Console.Write(array[i, j] + " ");
            }

            Console.WriteLine(); // New line after each row
        }
    }

    /// <summary>
    ///     Finds all occurrences of the specified target string within the given 2D character array.
    ///     Occurrences are searched in all eight directions (horizontal, vertical, and diagonals).
    ///     Returns a collection of matches, where each match is represented as a collection of coordinates.
    /// </summary>
    /// <param name="grid">The 2D character array to search.</param>
    /// <param name="target">The string to find.</param>
    /// <returns>
    ///     A list of matches, where each match is a list of (row, column) tuples indicating the positions of the found
    ///     string.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if grid or target is null.</exception>
    /// <exception cref="ArgumentException">Thrown if target is empty.</exception>
    public static List<List<(int Row, int Col)>> FindStringOccurrences(char[,] grid, string target)
    {
        if (grid == null)
        {
            throw new ArgumentNullException(nameof(grid));
        }

        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (target.Length == 0)
        {
            throw new ArgumentException("Target string cannot be empty.", nameof(target));
        }

        List<List<(int, int)>>? matches = new();
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        char firstChar = target[0];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                // Optimization: Only start search if first char matches
                if (grid[r, c] == firstChar)
                {
                    foreach ((int rowOffset, int colOffset) in Directions)
                    {
                        if (CanFit(grid, r, c, rowOffset, colOffset, target.Length))
                        {
                            List<(int, int)>? coords = TryMatch(grid, r, c, rowOffset, colOffset, target);
                            if (coords != null)
                            {
                                matches.Add(coords);
                            }
                        }
                    }
                }
            }
        }

        return matches;
    }

    /// <summary>
    ///     Extracts all integer values from a 2D character array.
    ///     Consecutive digits are combined to form a number. Non-digit characters separate numbers.
    ///     Returns a list of all parsed integers.
    /// </summary>
    /// <param name="grid">The 2D character array to search.</param>
    /// <returns>A list of integers found in the grid.</returns>
    /// <exception cref="ArgumentNullException">Thrown if grid is null.</exception>
    public static List<int> ExtractNumbers(char[,] grid)
    {
        if (grid == null)
        {
            throw new ArgumentNullException(nameof(grid));
        }

        List<int>? numbers = new();
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // We'll parse row-by-row for simplicity
        for (int r = 0; r < rows; r++)
        {
            int currentNumber = 0;
            bool buildingNumber = false;

            for (int c = 0; c < cols; c++)
            {
                char ch = grid[r, c];
                if (char.IsDigit(ch))
                {
                    buildingNumber = true;
                    currentNumber = currentNumber * 10 + (ch - '0');
                }
                else
                {
                    if (buildingNumber)
                    {
                        numbers.Add(currentNumber);
                        currentNumber = 0;
                        buildingNumber = false;
                    }
                }
            }

            // If the row ended but we were still building a number
            if (buildingNumber)
            {
                numbers.Add(currentNumber);
            }
        }

        return numbers;
    }

    /// <summary>
    ///     Checks if the target string can fit starting at position (r, c) in the given direction.
    /// </summary>
    private static bool CanFit(char[,] grid, int r, int c, int rowOffset, int colOffset, int length)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        int endRow = r + (length - 1) * rowOffset;
        int endCol = c + (length - 1) * colOffset;

        return endRow >= 0 && endRow < rows && endCol >= 0 && endCol < cols;
    }

    /// <summary>
    ///     Attempts to match the target string starting at (r, c) in a given direction.
    ///     If matched, returns the list of coordinates. Otherwise, returns null.
    /// </summary>
    private static List<(int, int)> TryMatch(char[,] grid, int r, int c, int rowOffset, int colOffset, string target)
    {
        List<(int, int)>? coords = new(target.Length);
        for (int i = 0; i < target.Length; i++)
        {
            int rr = r + i * rowOffset;
            int cc = c + i * colOffset;
            if (grid[rr, cc] != target[i])
            {
                return null;
            }

            coords.Add((rr, cc));
        }

        return coords;
    }
}
