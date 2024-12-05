namespace AOC.Tests.Y2024;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day04
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2024", "Data",
            $"{GetThisClassName()}.dat"));
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    private bool FindWord(string word, string[] lines, int charNum, int lineNum)
    {
        // This word search allows words to be horizontal, vertical, diagonal, written backwards, or even overlapping other words.
        
        // Start at the current character (if it matches the first letter in the word) and search in all directions for the next letter in the word.
        if (lines[lineNum][charNum] == word[0])
        {
            Console.WriteLine($"Checking char {charNum}");
            try
            {
                Console.WriteLine("Checking right");
                // Check right
                if (lines[lineNum].Substring(charNum, word.Length) == word)
                {
                    Console.WriteLine($"Found to right at line: {lineNum} char: {charNum}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Ignore, probably outside the 3d array
            }
            
            try
            {
                Console.WriteLine("Checking left");
                // Check left
                if (lines[lineNum].Substring(charNum - (word.Length - 1), word.Length) == new string(word.Reverse().ToArray()))
                {
                    Console.WriteLine($"Found to left at line: {lineNum} char: {charNum}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Ignore, probably outside the 3d array
            }
            
            try
            {
                Console.WriteLine("Checking up");
                // Check up
                bool notFoundUpwards = false;
                for (int i = 1; i < (word.Length - 1); i++)
                {
                    if ((lineNum - 1) >= 0)
                    {
                        try
                        {
                            Console.WriteLine(lines[lineNum - i][charNum]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error with accessing the char on the line - {ex.Message}");
                            notFoundUpwards = true;
                            throw;
                        }
                    
                        try
                        {
                            Console.WriteLine(word[i]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error with accessing the char number {i} in the word - {ex.Message}");
                            notFoundUpwards = true;
                            throw;
                        }
                    
                        if (lines[lineNum - i][charNum] != word[i])
                        {
                            notFoundUpwards = true;
                            break;
                        }
                    }
                    else
                    {
                        notFoundUpwards = true;
                    }
                }

                if (!notFoundUpwards)
                {
                    Console.WriteLine($"Found upwards at line: {lineNum} char: {charNum}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Ignore, probably outside the 3d array
            }
            
            try
            {
                Console.WriteLine("Checking down");
                // Check down
                bool notFoundDownwards = false;
                for (int i = 1; i < (word.Length - 1); i++)
                {
                    if ((lineNum + i) <= lines.Length)
                    {
                        try
                        {
                            Console.WriteLine(lines[lineNum + i][charNum]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error with accessing the char on the line - {ex.Message}");
                            notFoundDownwards = true;
                            throw;
                        }
                    
                        try
                        {
                            Console.WriteLine(word[i]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error with accessing the char number {i} in the word - {ex.Message}");
                            notFoundDownwards = true;
                            throw;
                        }
                    
                        if (lines[lineNum + i][charNum] != word[i])
                        {
                            notFoundDownwards = true;
                            break;
                        }
                    }
                    else
                    {
                        notFoundDownwards = true;
                    }
                    
                }

                if (!notFoundDownwards)
                {
                    Console.WriteLine($"Found downwards at line: {lineNum} char: {charNum}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Ignore, probably outside the 3d array
            }

            try
            {
                Console.WriteLine("Checking diagonals");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // TODO - diagonals!!

        return false;

    }

    [TestCase(@"..X...X
.SAMX.M
.A..A.A
XMAS.SS
.X.....", 4)]
    [TestCase(@"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX", 18)]
    [TestCase(null, 232)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        // string[] lines = input != null ? new[] { input } : realData;
         string[] lines = input != null ? input.Split("\n") : realData;

         var result = 0;

         foreach (var (line, index) in lines.WithIndex())
         {
             Console.WriteLine($"=====\nChecking line {index}\n=====");
             for (int i = 0; i < line.Length; i++)
             {
                 if (FindWord("XMAS", lines, i, index))
                 {
                     result += 1;
                 }
             }
         }

        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase("blah", 1)]
    [TestCase(null, 1783)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        //string[] lines = input != null ? new[] { input } : realData;
        // string[] lines = input != null ? input.Split("\n") : realData;

        //if (expected != null)
        //{
        //    Assert.That(result, Is.EqualTo(expected.Value));
        //}

        //Console.WriteLine($"Part 2: {result}");
    }
}
