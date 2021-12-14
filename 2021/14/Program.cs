namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");


            Part1(lines);
            //Part2(lines);
        }

        static void Part1(string[] lines)
        {
            string polymerTemplate = lines[0];

            Console.WriteLine($"Template: {polymerTemplate}");

            // Pair insertion rules are stored from index 2 onwards
            // A rule like AB -> C means that when elements A and B are immediately adjacent, element C
            // should be inserted between them.
            // Also, because all pairs are considered simultaneously, inserted elements are not considered
            // to be part of a pair until the next step.

            List<Rule> rules = new List<Rule>();
            // Consider each rule
            for (int i = 2; i < lines.Length; i++)
            {
                // e.g. AB -> C
                string pairToMatch = lines[i].Substring(0, 2);
                string characterToInsert = lines[i].Substring(6, 1);

                rules.Add(new Rule()
                {
                    First = pairToMatch[0],
                    Second = pairToMatch[1],
                    Insert = characterToInsert[0],
                });
            }

            // Set the number of steps to run
            for (int i = 0; i < 10; i++)
            {
                CompleteStep(ref polymerTemplate, ref lines, rules);
                //Console.WriteLine($"After step {i+1}: {polymerTemplate}");
            }

            // Count occurrences of each character
            char[] chars = polymerTemplate.ToCharArray();
            List<char> uniqueCharacters = chars.Select(x => x).Distinct().ToList();
            List<(char character, int count)> countList = new List<(char character, int count)>();

            foreach (char character in uniqueCharacters)
            {
                countList.Add((character, polymerTemplate.Count(c => c == character)));
            }

            (char character, int count) mostCommon = countList.MaxBy(x => x.count);
            (char character, int count) leastCommon = countList.MinBy(x => x.count);

            Console.WriteLine(mostCommon.count - leastCommon.count);
        }

        static void CompleteStep(ref string polymerTemplate, ref string[] lines, List<Rule> rules)
        {
            LinkedList<char> temporaryList = new LinkedList<char>();
            List<char> temporaryTemplate = new List<char>();

            foreach (char character in polymerTemplate)
            {
                temporaryList.AddLast(character);
                temporaryTemplate.Add(character);
            }

            LinkedListNode<char> pointer = temporaryList.Find(temporaryList.First());

            // Insert the new character into the linked list but do not change the temporary template we're checking against
            for (int i = 0; i < temporaryTemplate.Count() - 1; i++)
            {
                Rule matchingRule = rules.FirstOrDefault(r => r.First == temporaryTemplate.ElementAt(i) && r.Second == temporaryTemplate.ElementAt(i + 1));

                if (matchingRule is not null)
                {
                    temporaryList.AddAfter(pointer, matchingRule.Insert);

                    // Move on an extra position as we have added a node
                    pointer = pointer.Next;
                }

                pointer = pointer.Next;
            }

            polymerTemplate = String.Join("", temporaryList);
        }

        class Rule
        {
            public char First { get; set; }
            public char Second { get; set; }
            public char Insert { get; set; }
        }
    }
}