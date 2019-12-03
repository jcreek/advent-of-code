using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day_02
{
    class Program
    {
        static void Main(string[] args)
        {
            //Part1();
            Part2();
        }

        static List<int> RunProgram(List<int> inputList)
        {
            // Once you're done processing an opcode, move to the next one by stepping forward 4 positions.
            for (int i = 0; i < inputList.Count(); i+=4)
            {
                int num1position = inputList[i+1];
                int num1 = inputList[num1position];

                int num2position = inputList[i+2];
                int num2 = inputList[num2position];

                switch (inputList[i])
                {
                    case 1:
                        // Opcode 1 adds together numbers read from two positions and stores the result in a third position. The three integers immediately after the opcode tell you these three positions - the first two indicate the positions from which you should read the input values, and the third indicates the position at which the output should be stored.

                        // For example, if your Intcode computer encounters 1,10,20,30, it should read the values at positions 10 and 20, add those values, and then overwrite the value at position 30 with their sum.

                        int calculatedValue1 = num1 + num2;
                        int calculatedValue1Position = inputList[i+3];
                        
                        inputList[calculatedValue1Position] = calculatedValue1;
                        break;
                    case 2:
                        // Opcode 2 works exactly like opcode 1, except it multiplies the two inputs instead of adding them. Again, the three integers after the opcode indicate where the inputs and outputs are, not their values.

                        int calculatedValue2 = num1 * num2;
                        int calculatedValue2Position = inputList[i+3];
                        
                        inputList[calculatedValue2Position] = calculatedValue2;
                        break;
                    default:
                        // Finally return the list
                        return inputList;
                }
            }

            // Finally return the list
            return inputList;
        }

        static void Part1()
        {
            // There is only one line so get it
            var input = File.ReadAllLines("input.txt")[0];
            List<int> inputList = input.Split(',').Select(int.Parse).ToList();

            // Replace position 1 with value 12 
            inputList[1] = 12;

            // Replace position 2 with value 2 
            inputList[2] = 2;

            // Run the program stored in the input 
            List<int> outputList = RunProgram(inputList);

            Console.WriteLine("Part 1: The value is left at position 0 after the program halts is " + outputList[0]);
        }

        static void Part2()
        {
            // There is only one line so get it
            var input = File.ReadAllLines("input.txt")[0];
            List<int> inputList = input.Split(',').Select(int.Parse).ToList();

            int finalNoun = 0;
            int finalVerb = 0;

            List<int> inputListModified = new List<int>(){};

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    // Reset the input list - .ToList() is needed here to ensure it's actually a new list, not just a reference to the original list
                    inputListModified = inputList.ToList();

                    // Replace position 1 with noun
                    inputListModified[1] = noun;
                    
                    // Replace position 2 with verb
                    inputListModified[2] = verb;
                    
                    // Run the program stored in the input 
                    List<int> outputList = RunProgram(inputListModified);
                    if(outputList[0] == 19690720)
                    {
                        finalNoun = noun;
                        finalVerb = verb;
                        Console.WriteLine(noun + " - " + verb);
                    }
                }
            }

            Console.WriteLine("Part 2: The pair of inputs that produce the output 19690720 are " + finalNoun + " and " + finalVerb);
            Console.WriteLine($"100 * noun + verb is {100 * finalNoun + finalVerb}");
        }
    }
}
