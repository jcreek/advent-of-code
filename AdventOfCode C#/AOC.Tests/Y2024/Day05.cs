using System.Text.RegularExpressions;

namespace AOC.Tests.Y2024;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Day05
{
    [SetUp]
    public void Setup()
    {
        realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2024", "Data",
            $"{GetThisClassName()}.dat"));
    }

    private record Rule(int BeforeNum, int AfterNum);

    private List<Rule> ProcessPageOrderingRules(List<string> rules)
    {
        List<Rule> pageOrderingRules = new();

        // The notation X|Y means that if both page number X and page number Y are to be produced as part of an update,
        // page number X must be printed at some point before page number Y
        foreach (string rule in rules)
        {
            string pattern = @"(?<beforeNum>\d+)\|(?<afterNum>\d+)";

            Regex regex = new(pattern);
            Match match = regex.Match(rule);

            if (!match.Success)
            {
                throw new Exception($"Rule '{rule}' is not in the correct format");
            }

            int beforeNum = int.Parse(match.Groups["beforeNum"].Value);
            int afterNum = int.Parse(match.Groups["afterNum"].Value);

            pageOrderingRules.Add(new Rule(beforeNum, afterNum));
        }

        return pageOrderingRules;
    }

    private bool PageNumbersMeetRules(List<Rule> pageOrderingRules, List<int> pageNumbers)
    {
        foreach (Rule rule in pageOrderingRules)
        {
            int beforeNumIndex = pageNumbers.IndexOf(rule.BeforeNum);
            int afterNumIndex = pageNumbers.IndexOf(rule.AfterNum);

            if (beforeNumIndex == -1 || afterNumIndex == -1)
            {
                continue;
            }

            if (beforeNumIndex > afterNumIndex)
            {
                return false;
            }
        }

        return true;
    }

    private void ApplyRulesToPageNumbers(ref List<int> pageNumbers, List<Rule> pageOrderingRules)
    {
        foreach (Rule rule in pageOrderingRules)
        {
            int beforeNumIndex = pageNumbers.IndexOf(rule.BeforeNum);
            int afterNumIndex = pageNumbers.IndexOf(rule.AfterNum);

            if (beforeNumIndex == -1 || afterNumIndex == -1)
            {
                continue;
            }

            if (beforeNumIndex > afterNumIndex)
            {
                (pageNumbers[beforeNumIndex], pageNumbers[afterNumIndex]) =
                    (pageNumbers[afterNumIndex], pageNumbers[beforeNumIndex]);
            }
        }
    }

    private List<string> ReorderInvalidPageNumbers(List<Rule> pageOrderingRules, List<string> invalidPageNumbers)
    {
        List<string> correctedPageNumbers = new();

        foreach (string invalidPageNumbersSet in invalidPageNumbers)
        {
            List<int> pageNumbers = invalidPageNumbersSet.Split(",").Select(int.Parse).ToList();

            // Apply the rules to the page numbers in a loop until there are no more changes
            bool hasChanges;
            do
            {
                hasChanges = false;
                List<int> originalPageNumbers = new(pageNumbers);

                ApplyRulesToPageNumbers(ref pageNumbers, pageOrderingRules);

                if (!pageNumbers.SequenceEqual(originalPageNumbers))
                {
                    hasChanges = true;
                }
            } while (hasChanges);

            correctedPageNumbers.Add(string.Join(",", pageNumbers));
        }

        return correctedPageNumbers;
    }

    protected string GetThisClassName() { return GetType().Name; }
    private string[] realData;

    [TestCase(@"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47", 143)]
    [TestCase(null, 4766)] // The actual answer
    public void Part1(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        List<string> rules = new();
        List<string> pagesToProduce = new();

        bool isRules = true;
        foreach (string line in lines)
        {
            // The rules appear above a blank line, and the page numbers appear below it
            if (string.IsNullOrWhiteSpace(line))
            {
                isRules = false;
                continue;
            }

            if (isRules)
            {
                rules.Add(line);
            }
            else
            {
                pagesToProduce.Add(line);
            }
        }

        List<Rule> pageOrderingRules = ProcessPageOrderingRules(rules);
        List<string> validPagesToProduce = new();

        foreach (string pageSet in pagesToProduce)
        {
            List<int> pageNumbers = pageSet.Split(",").Select(int.Parse).ToList();

            if (PageNumbersMeetRules(pageOrderingRules, pageNumbers))
            {
                validPagesToProduce.Add(pageSet);
            }
        }

        // Find the middle page number of each set of page numbers that meet the rules and will be printed
        List<int> middlePageNumbers = new();
        foreach (string pageSet in validPagesToProduce)
        {
            List<int> pageNumbers = pageSet.Split(",").Select(int.Parse).ToList();
            middlePageNumbers.Add(pageNumbers[pageNumbers.Count / 2]);
        }

        int result = middlePageNumbers.Sum();


        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 1: {result}");
    }

    [TestCase(@"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47", 123)]
    [TestCase(null, 6257)] // The actual answer
    public void Part2(string? input, int? expected)
    {
        string[] lines = input != null ? input.Split("\n") : realData;

        List<string> rules = new();
        List<string> pagesToProduce = new();

        bool isRules = true;
        foreach (string line in lines)
        {
            // The rules appear above a blank line, and the page numbers appear below it
            if (string.IsNullOrWhiteSpace(line))
            {
                isRules = false;
                continue;
            }

            if (isRules)
            {
                rules.Add(line);
            }
            else
            {
                pagesToProduce.Add(line);
            }
        }

        List<Rule> pageOrderingRules = ProcessPageOrderingRules(rules);
        List<string> invalidPagesToProduce = new();

        foreach (string pageSet in pagesToProduce)
        {
            List<int> pageNumbers = pageSet.Split(",").Select(int.Parse).ToList();

            if (!PageNumbersMeetRules(pageOrderingRules, pageNumbers))
            {
                invalidPagesToProduce.Add(pageSet);
            }
        }

        List<string> correctedInvalidPagesToProduce =
            ReorderInvalidPageNumbers(pageOrderingRules, invalidPagesToProduce);

        // Find the middle page number of each set of page numbers that did not originally meet the rules but have now been fixed
        List<int> middlePageNumbers = new();
        foreach (string pageSet in correctedInvalidPagesToProduce)
        {
            List<int> pageNumbers = pageSet.Split(",").Select(int.Parse).ToList();
            middlePageNumbers.Add(pageNumbers[pageNumbers.Count / 2]);
        }

        int result = middlePageNumbers.Sum();


        if (expected != null)
        {
            Assert.That(result, Is.EqualTo(expected.Value));
        }

        Console.WriteLine($"Part 2: {result}");
    }
}
