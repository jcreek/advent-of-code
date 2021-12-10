﻿namespace Day10
{
    class Program
    {
        static List<char> openingCharacters = new List<char>() { '(', '[', '{', '<' };
        static List<char> closingCharacters = new List<char>() { ')', ']', '}', '>' };

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");


            Part1(lines);
            //Part2(lines);
        }

        static void Part1(string[] lines)
        {
            List<string> uncorruptedLines = new List<string>();
            int syntaxErrorScore = 0;

            foreach (string line in lines)
            {
                if (!IsLineCorrupted(line, ref syntaxErrorScore))
                {
                    uncorruptedLines.Add(line);
                }
            }

            Console.WriteLine($"Syntax error score: {syntaxErrorScore}");
        }

        static bool IsLineCorrupted(string line, ref int syntaxErrorScore)
        {
            Stack<char> stackOfExpectedClosingCharacters = new Stack<char>();

            for (int i = 0; i < line.Length; i++)
            {
                if (openingCharacters.Contains(line[i]))
                {
                    // If it's an opening character then add the expected closing character to the stack
                    stackOfExpectedClosingCharacters.Push(GetExpectedClosingCharacter(line[i]));
                }
                else if (closingCharacters.Contains(line[i]))
                {
                    // If it's a closing character, then check if it's the one we're expecting
                    // Make sure there's items in the stack so we don't get an InvalidOperationException
                    if (stackOfExpectedClosingCharacters.Count > 0)
                    {
                        char expectedClosingCharacter = stackOfExpectedClosingCharacters.Pop();
                        if (line[i] != expectedClosingCharacter)
                        {
                            // It's not the one we're expecting so the line is corrupted
                            Console.WriteLine($"Expected {expectedClosingCharacter}, but found {line[i]} instead.");

                            // Update the syntax error score
                            switch (line[i])
                            {
                                case ')':
                                    syntaxErrorScore += 3;
                                    break;
                                case ']':
                                    syntaxErrorScore += 57;
                                    break;
                                case '}':
                                    syntaxErrorScore += 1197;
                                    break;
                                case '>':
                                    syntaxErrorScore += 25137;
                                    break;
                                default:
                                    break;
                            }

                            // Report the corrupted line
                            return true;
                        }

                        // If it is the one we're expecting we can safely carry on with the line
                    }
                }
            }

            // Having reached the end of the line with no wrong closing characters the line cannot be corrupted
            return false;
        }

        static char GetExpectedClosingCharacter(char openingCharacter)
        {
            int index = openingCharacters.FindIndex(x => x == openingCharacter);

            return closingCharacters[index];
        }
    }
}