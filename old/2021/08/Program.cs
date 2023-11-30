namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            

            //Part1(lines);
            Part2(lines);
        }

        static void Part1(string[] lines)
        {
            // 8 has only 7 segments/letters 

            // 7 has only 3 segments/letters

            // 4 has only 4 segments/letters

            // 1 has only 2 segments/letters

            // Count how often groups of letters with 2/3/4/7 segments occur after the split 
            int count = 0;
            foreach (string line in lines)
            {
                string secondHalf = line.Substring(line.IndexOf("|") + 1);
                List<string> numberSegments = secondHalf.Split(' ').ToList();

                foreach (string numberSegment in numberSegments)
                {
                    switch (numberSegment.Length)
                    {
                        case 7:
                        case 3:
                        case 4:
                        case 2:
                            count += 1;
                            break;
                        default:
                            // code block
                            break;
                    }
                }
            }

            Console.WriteLine(count);
        }

        static void Part2(string[] lines)
        {
            int total = 0;
            foreach (string line in lines)
            {
                string secondHalf = line.Substring(line.IndexOf("|") + 1);
                List<string> numberSegments = secondHalf.Split(' ').ToList();






                string lineTotal = string.Empty;

                foreach (string numberSegment in numberSegments)
                {
                    // Go through sorted in alphabetical order
                    string alphabetisedNumberSegment = String.Concat(numberSegment.OrderBy(c => c));
                    Console.WriteLine(alphabetisedNumberSegment);

                    if(alphabetisedNumberSegment == String.Concat("acedgfb".OrderBy(c => c)))
                    {
                        lineTotal += "8";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("cdfbe".OrderBy(c => c)))
                    {
                        lineTotal += "5";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("gcdfa".OrderBy(c => c)))
                    {
                        lineTotal += "2";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("fbcad".OrderBy(c => c)))
                    {
                        lineTotal += "3";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("dab".OrderBy(c => c)))
                    {
                        lineTotal += "7";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("cefabd".OrderBy(c => c)))
                    {
                        lineTotal += "9";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("cdfgeb".OrderBy(c => c)))
                    {
                        lineTotal += "6";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("eafb".OrderBy(c => c)))
                    {
                        lineTotal += "4";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("cagedb".OrderBy(c => c)))
                    {
                        lineTotal += "0";
                    }
                    else if(alphabetisedNumberSegment == String.Concat("ab".OrderBy(c => c)))
                    {
                        lineTotal += "1";
                    }

                    Console.WriteLine(lineTotal);
                }

                Console.WriteLine(lineTotal);
                if (!string.IsNullOrEmpty(lineTotal))
                {
                    total += int.Parse(lineTotal);
                }
            }

            Console.WriteLine(total);
        }
    
        static Dictionary<int, string> DetermineMapping(string[] lines)
        {
            Dictionary<int, string> mapping = new Dictionary<int, string>();

            foreach (string line in lines)
            {
                string secondHalf = line.Substring(line.IndexOf("|") + 1);
                List<string> numberSegments = secondHalf.Split(' ').ToList();

                foreach (string numberSegment in numberSegments)
                {
                    switch (numberSegment.Length)
                    {
                        case 7:
                            // 8 has only 7 segments/letters 
                            mapping.Add(8, numberSegment);
                            break;
                        case 3:
                            // 7 has only 3 segments/letters
                            mapping.Add(7, numberSegment);
                            break;
                        case 4:
                            // 4 has only 4 segments/letters 
                            mapping.Add(4, numberSegment);
                            break;
                        case 2:
                            // 1 has only 2 segments/letters
                            mapping.Add(1, numberSegment);
                            break;
                        default:
                            // handle the non-unique lengths (0,2,3,5,6,9)
                            
                            break;
                    }
                }
            }

            return mapping;
        }
    }
}
