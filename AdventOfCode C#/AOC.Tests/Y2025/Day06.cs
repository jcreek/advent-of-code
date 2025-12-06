using System.Diagnostics;

namespace AOC.Tests.Y2025;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day06
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2025", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    internal record Calculation(IReadOnlyList<long> Numbers, char Operation);

    private List<Calculation> GetCalculations(string[] lines)
    {
        int numberRowCount = lines.Length - 1;
        List<string[]> numberRows = new(numberRowCount);

        // Split the number rows
        for (int row = 0; row < numberRowCount; row++)
        {
            string[] parts = lines[row].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            numberRows.Add(parts);
        }

        // Split the operations row (last line)
        string[] operations = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int columnCount = numberRows[0].Length;

        List<Calculation> calculations = new(columnCount);

        for (int col = 0; col < columnCount; col++)
        {
            long[] numbers = new long[numberRowCount];

            for (int row = 0; row < numberRowCount; row++)
            {
                numbers[row] = long.Parse(numberRows[row][col]);
            }

            char op = operations[col][0];

            calculations.Add(new Calculation(numbers, op));
        }

        return calculations;
    }

    private List<Calculation> GetCalculationsVertically(string[] lines)
    {
        int rows = lines.Length;
        if (rows == 0)
        {
            return new List<Calculation>();
        }

        // Make all lines the same width so indexing is safe
        int columns = lines.Max(l => l.Length);
        string[] grid = lines
            .Select(l => l.PadRight(columns, ' '))
            .ToArray();

        int operatorRow = rows - 1;

        List<Calculation> calculations = new();

        int col = 0;
        while (col < columns)
        {
            // Is this a separator column? (all spaces)
            bool isSeparator = true;
            for (int r = 0; r < rows; r++)
            {
                if (grid[r][col] != ' ')
                {
                    isSeparator = false;
                    break;
                }
            }

            if (isSeparator)
            {
                col++;
                continue;
            }

            int startCol = col;
            col++;

            while (col < columns)
            {
                bool sep = true;
                for (int r = 0; r < rows; r++)
                {
                    if (grid[r][col] != ' ')
                    {
                        sep = false;
                        break;
                    }
                }

                if (sep)
                {
                    break;
                }

                col++;
            }

            int endCol = col - 1;

            // Operator is at the bottom of the leftmost column
            char op = grid[operatorRow][startCol];
            List<long> numbers = new();

            // Read numbers right-to-left within the problem (endCol to startCol)
            for (int c = endCol; c >= startCol; c--)
            {
                List<char> digits = new();

                // Digits are from all rows above the operator row
                for (int r = 0; r < operatorRow; r++)
                {
                    char ch = grid[r][c];
                    if (char.IsDigit(ch))
                    {
                        digits.Add(ch);
                    }
                }

                long value = long.Parse(new string(digits.ToArray()));
                numbers.Add(value);
            }

            calculations.Add(new Calculation(numbers, op));
        }

        return calculations;
    }

    [TestCase(@"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  ", (long)4277556)]
    [TestCase(null, 5667835681547)] // The actual answer
    public void Part1(string? input, long? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        List<Calculation> calculations = GetCalculations(lines);

        long runningTotal = 0;

        foreach (Calculation c in calculations)
        {
            long answer = 0;

            switch (c.Operation)
            {
                case '+':
                    answer = 0;
                    foreach (long n in c.Numbers)
                    {
                        answer += n;
                    }

                    break;

                case '*':
                    answer = 1;
                    foreach (long n in c.Numbers)
                    {
                        answer *= n;
                    }

                    break;
            }

            runningTotal += answer;
        }

        long result = runningTotal;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  ", (long)3263827)]
    [TestCase(null, 9434900032651)] // The actual answer
    public void Part2(string? input, long? expected)
    {
        //string[] lines = input != null ? new[] { input } : realData;
        string[] lines = input != null ? input.Split("\n") : realData;

        List<Calculation> calculations = GetCalculationsVertically(lines);

        long runningTotal = 0;

        foreach (Calculation c in calculations)
        {
            long answer = 0;

            switch (c.Operation)
            {
                case '+':
                    answer = 0;
                    foreach (long n in c.Numbers)
                    {
                        answer += n;
                    }

                    break;

                case '*':
                    answer = 1;
                    foreach (long n in c.Numbers)
                    {
                        answer *= n;
                    }

                    break;
            }

            runningTotal += answer;
            Debug.WriteLine($"{c.Numbers} {c.Operation} {answer}");
        }

        long result = runningTotal;

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
