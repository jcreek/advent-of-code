using System.Text;

namespace AOC.Helpers;

public static class TwoDimensionalArrays
{
    public static char[,] Make2DArrayFromStringArray(string[] stringArray)
    {
        int rows = stringArray.Length;
        int cols = stringArray[0].Length;

        char[,] array = new char[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                array[r, c] = stringArray[r][c];
            }
        }

        return array;
    }

    public static string TwoDimensionalArrayToString(char[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        StringBuilder sb = new(rows * (cols + 1)); // performance optimisation

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                sb.Append(array[r, c]);
            }

            sb.AppendLine(); // end of row
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Enumerates all cells in a 2D array, returning each cell's row index,
    ///     column index, and value.
    /// </summary>
    /// <typeparam name="T">The element type of the 2D array.</typeparam>
    /// <param name="grid">The 2D array to enumerate.</param>
    /// <returns>
    ///     An <see cref="IEnumerable{T}" /> of tuples containing the row index,
    ///     column index, and value of each cell in the array, iterated in row-major order.
    /// </returns>
    /// <example>
    ///     The following example prints every cell in a character grid:
    ///     <code>
    /// char[,] grid = {
    ///     { 'A', 'B', 'C' },
    ///     { 'D', 'E', 'F' }
    /// };
    /// 
    /// foreach (var (row, column, value) in Cells(grid))
    /// {
    ///     Console.WriteLine($"[{row}, {column}] = {value}");
    /// }
    /// </code>
    ///     This produces:
    ///     <code>
    /// [0, 0] = A
    /// [0, 1] = B
    /// [0, 2] = C
    /// [1, 0] = D
    /// [1, 1] = E
    /// [1, 2] = F
    /// </code>
    /// </example>
    public static IEnumerable<(int row, int column, T value)> Cells<T>(T[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                yield return (r, c, grid[r, c]);
            }
        }
    }

    public static List<(int row, int column)> CheckAllSurroundingCellsForCharacter(
        char[,] array,
        int startRow,
        int startColumn,
        char characterToCheckFor)
    {
        int totalRows = array.GetLength(0);
        int totalColumns = array.GetLength(1);

        List<(int row, int column)> results = new();

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                // Skip the centre cell
                if (rowOffset == 0 && colOffset == 0)
                {
                    continue;
                }

                int row = startRow + rowOffset;
                int column = startColumn + colOffset;

                // Bounds check
                if (row < 0 || row >= totalRows || column < 0 || column >= totalColumns)
                {
                    continue;
                }

                if (array[row, column] == characterToCheckFor)
                {
                    results.Add((row, column));
                }
            }
        }

        return results;
    }
}
