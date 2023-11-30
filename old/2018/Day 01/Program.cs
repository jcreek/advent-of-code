using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day_01
{
    class Program
    {
        static void Main(string[] args)
        {
            //Part1();
            Part2();            
        }

        static void Part1()
        {
            var frequency = 0;

            var lines = File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                if (line.Substring(0,1) == "+") 
                {
                    frequency += Int32.Parse(line.Substring(1));
                }
                else 
                {
                    frequency -= Int32.Parse(line.Substring(1));
                }
            }
            Console.WriteLine("Part 1: The resulting frequency is " + frequency);
        }

        static void Part2()
        {
            int frequency = 0;
            List<int> frequencyHistory = new List<int>();
            int loopCount = 0;
            bool foundDuplicate = false;

            var lines = File.ReadAllLines("input.txt");
            while (!foundDuplicate) {
                foreach (var line in lines)
                {
                    if (line.Substring(0,1) == "+") 
                    {
                        frequency += Int32.Parse(line.Substring(1));
                    }
                    else 
                    {
                        frequency -= Int32.Parse(line.Substring(1));
                    }

                    if (frequencyHistory.Contains(frequency)) {
                        Console.WriteLine("The first repeated frequency is " + frequency);
                        foundDuplicate = true;
                        break;
                    }
                    else {
                        frequencyHistory.Add(frequency);
                    }
                }
                loopCount += 1;
            }
            Console.WriteLine("The loop count is " + loopCount);
        }
    }
}
