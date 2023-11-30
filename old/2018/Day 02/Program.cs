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

        static void Part1()
        {
            int twiceCounter = 0; 
            int thriceCounter = 0;

            var lines = File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                List<char> searchCharHistory = new List<char>();
                int lineTwiceCounter = 0;
                int lineThriceCounter = 0;

                for (int i = 0; i < line.Length; i++)
                {
                    char searchChar = line[i];

                    if(!searchCharHistory.Any(s => s == searchChar))
                    {
                        // If we haven't already counted this letter 

                        int matchCharCount = line.Count(f => f == searchChar);

                        // Check if a letter appears twice (and increment the counter) 
                        if (matchCharCount == 2) {
                            lineTwiceCounter += 1;
                        }
                        // Check if a letter appears three times (and increment the counter)
                        else if (matchCharCount == 3) {
                            lineThriceCounter += 1;
                        }
                    }
                    searchCharHistory.Add(searchChar);
                }

                if (lineTwiceCounter > 0) {
                    twiceCounter += 1;
                }
                if (lineThriceCounter > 0) {
                    thriceCounter += 1;
                }
            }
            Console.WriteLine(twiceCounter);
            Console.WriteLine(thriceCounter);
            // multiply the two counters together 
            Console.WriteLine("The checksum is " + (twiceCounter * thriceCounter));
        }

        static void Part2()
        {
            bool endLoop = false;

            var lines = File.ReadAllLines("input.txt");
            // For each line of the input file
            for (int i = 0; i < lines.Length; i++)
            {
                // Compare this line with each other line in the array 
                for (int j = 0; j < lines.Length; j++) {
                    if (CompareStrings(lines[i],lines[j])) {
                        // If it finds a string with just one letter wrong
                        endLoop = true; 
                        break;
                    }
                }
                if (endLoop) {
                    break;
                }
            }
        }

        static bool CompareStrings(string string1, string string2)
        {
            if (string1 == string2) {
                // Do nothing, it's the same string
            }
            
            // Split the characters of both strings into arrays 
            List<char> arr1 = new List<char>();
            List<char> arr2 = new List<char>();
            
            for (int ii = 0; ii < string1.Length; ii++) {
                arr1.Add(string1[ii]);
            }
            for (int jj = 0; jj < string2.Length; jj++) {
                arr2.Add(string2[jj]);
            }

            int wrongCharCounter = 0; 
            List<char> sameChars = new List<char>();

            // For each letter in the first array, check if it corresponds to the appropriate letter in the second array
            for (int kk = 0; kk < arr1.Count(); kk++) {
                if (arr1[kk] != arr2[kk]) {
                    wrongCharCounter += 1;
                }
                else { 
                    sameChars.Add(arr1[kk]);
                }
            }

            if (wrongCharCounter == 1) {
                Console.WriteLine("Common letters are: " + string.Join( ",", sameChars).Replace(",", ""));
                return true;
            }
            else {
                return false;
            }
        }
    }
}