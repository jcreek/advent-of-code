using Creek.HelpfulExtensions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var sr = new StreamReader("input.txt");
            var translate = new Dictionary<string, string>();

            string polymer = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.Length > 0)
                {
                    var p = line.Split(" -> ");
                    translate.Add(p[0], p[1]);
                }
            }

            var pairs = new Dictionary<string, ulong>();
            var elements = new Dictionary<string, ulong>();

            for (int k = 0; k < polymer.Length - 1; k++)
            {
                var p = polymer.Substring(k, 2);
                pairs.TryAdd(p, 0);
                pairs[p]++;
            }

            for (int k = 0; k < polymer.Length; k++)
            {
                elements.TryAdd(polymer[k].ToString(), 0);
                elements[polymer[k].ToString()]++;
            }

            for (int i = 0; i < 40; i++)
            {
                var newpairs = new Dictionary<string, ulong>();
                foreach (var p in pairs.Keys)
                {
                    var insert = translate[p];
                    var c = pairs[p];
                    newpairs.TryAdd(p[0] + insert, 0);
                    newpairs[p[0] + insert] += c;
                    newpairs.TryAdd(insert + p[1], 0);
                    newpairs[insert + p[1]] += c;
                    elements.TryAdd(insert, 0);
                    elements[insert] += c;
                }
                pairs = newpairs;
            }

            ulong min = ulong.MaxValue;
            ulong max = ulong.MinValue;
            ulong sum = 0;

            foreach (var a in elements.Values)
            {
                if (a > max) max = a;
                if (a < min) min = a;
                sum += a;
            }

            Console.WriteLine($"Result: {max} - {min} = {max - min}, length = {sum}");
        }

        //static void Main(string[] args)
        //{
        //    string[] lines = File.ReadAllLines("input.txt");


        //    //Part1(lines);
        //    Part2(lines);
        //}

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

        static void Part2(string[] lines)
        {
            string polymerTemplate = lines[0];

            Console.WriteLine($"Template: {polymerTemplate}");

            // Pair insertion rules are stored from index 2 onwards
            // A rule like AB -> C means that when elements A and B are immediately adjacent, element C
            // should be inserted between them.
            // Also, because all pairs are considered simultaneously, inserted elements are not considered
            // to be part of a pair until the next step.

            Dictionary<(char, char), char> rules = new Dictionary<(char, char), char>();
            // Consider each rule
            for (int i = 2; i < lines.Length; i++)
            {
                // e.g. AB -> C
                string pairToMatch = lines[i].Substring(0, 2);
                string characterToInsert = lines[i].Substring(6, 1);

                rules.Add((pairToMatch[0], pairToMatch[1]), characterToInsert[0]);
            }

            // Set the number of steps to run
            for (int i = 0; i < 40; i++)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                
                CompleteStepNew(ref polymerTemplate, rules);

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine($"Step {i + 1} took {elapsedTime}");
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

        static void CompleteStepNew(ref string polymerTemplate, Dictionary<(char, char), char> rules)
        {
            LinkedList<char> temporaryList = new LinkedList<char>();
            char[] temporaryTemplate = polymerTemplate.ToCharArray();

            for (int i = 0; i < temporaryTemplate.Length; i++)
            {
                temporaryList.AddLast(temporaryTemplate[i]);
            }

            LinkedListNode<char> pointer = temporaryList.Find(temporaryList.First());

            // Insert the new character into the linked list but do not change the temporary template we're checking against
            for (int i = 0; i < temporaryTemplate.Count() - 1; i++)
            {
                char insertValue;
                
                //using ("First".DisposableStopWatch())
                //{
                rules.TryGetValue((temporaryTemplate[i], temporaryTemplate[i + 1]), out insertValue);
                //}

                temporaryList.AddAfter(pointer, insertValue);

                // Move on an extra position as we have added a node
                pointer = pointer.Next.Next;
            }

            polymerTemplate = String.Join("", temporaryList);
            //polymerTemplate = string.Concat(temporaryList); // this is slower
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