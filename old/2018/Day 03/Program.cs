using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_03
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare an empty 2D array with 1000x1000 cells, material[x,y] from left and top respectively
            int[,] material = new int[1000, 1000];
            List<Claim> claims = new List<Claim>();

            var lines = File.ReadAllLines("input.txt");
            // For each line of the input file
            foreach (var line in lines)
            {
                // A claim like #123 @ 3,2: 5x4 means that claim ID 123 specifies a rectangle 3 inches from the left edge, 2 inches from the top edge, 5 inches wide, and 4 inches tall.

                // Remove the id number and everything prior to the left edge measurement 
                int startIndex = line.IndexOf("@") + 1;
                int endIndex = line.Length;
                var lineData = line.Substring(startIndex, endIndex - startIndex);

                // Store all the data 
                Regex re1 = new Regex(@"(@ [0-9]+,)");
                var leftPositionList = re1.Matches(line);
                string leftPosition = leftPositionList[0].Value.Substring(2, leftPositionList[0].Value.Length -1 - 2);

                Regex re2 = new Regex(@"(,[0-9]+:)");
                var topPositionList = re2.Matches(line);
                string topPosition = topPositionList[0].Value.Substring(1, topPositionList[0].Value.Length -1 - 1);

                Regex re3 = new Regex(@"(: [0-9]+x)");
                var widthList = re3.Matches(line);
                int width = Int32.Parse(widthList[0].Value.Substring(1, widthList[0].Value.Length -1 - 1));

                Regex re4 = new Regex(@"(x[0-9]+)");
                var lengthList = re4.Matches(line);
                int length = Int32.Parse(lengthList[0].Value.Substring(1));

                // Store data in all cells horizontally and vertically 
                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < length; j++) {
                        // Store a count of how many times the cell is used at the appropriate co-ordinates
                        material[(Int32.Parse(leftPosition) + i), (Int32.Parse(topPosition) + j)] += 1; 
                    }
                }
            }
            
            // Part 1

            // Loop through both arrays and count how many cells have a recorded overlap
            int overlapCounter = 0;
            for(int i = 0; i < 1000; i++){
                for(int j = 0; j < 1000; j++){
                    if(material[i, j] > 1){
                        overlapCounter += 1; 
                    }
                }
            }
            Console.WriteLine(material[0, 0]);
            Console.WriteLine(material.Length);
            Console.WriteLine("The number of overlapping square inches is " + overlapCounter); 


            // Part 2 

            // Loop through all claims (lines in the file) and find the one where the material is only used once in every cell

            // For each line of the input file
            foreach (var line in lines)
            {
                var claim = new Claim();
                // A claim like #123 @ 3,2: 5x4 means that claim ID 123 specifies a rectangle 3 inches from the left edge, 2 inches from the top edge, 5 inches wide, and 4 inches tall.

                // Remove the id number and everything prior to the left edge measurement 
                int startIndex = line.IndexOf("@") + 1;
                int endIndex = line.Length;
                var lineData = line.Substring(startIndex, endIndex - startIndex);

                // Store all the data 
                Regex re0 = new Regex(@"(#(\d+))");
                var claimIdList = re0.Matches(line);
                string claimId = claimIdList[0].Value.Substring(1);
                claim.ClaimId = Int32.Parse(claimId);

                Regex re1 = new Regex(@"(@ [0-9]+,)");
                var leftPositionList = re1.Matches(line);
                string leftPosition = leftPositionList[0].Value.Substring(2, leftPositionList[0].Value.Length -1 - 2);

                Regex re2 = new Regex(@"(,[0-9]+:)");
                var topPositionList = re2.Matches(line);
                string topPosition = topPositionList[0].Value.Substring(1, topPositionList[0].Value.Length -1 - 1);

                Regex re3 = new Regex(@"(: [0-9]+x)");
                var widthList = re3.Matches(line);
                int width = Int32.Parse(widthList[0].Value.Substring(1, widthList[0].Value.Length -1 - 1));

                Regex re4 = new Regex(@"(x[0-9]+)");
                var lengthList = re4.Matches(line);
                int length = Int32.Parse(lengthList[0].Value.Substring(1));

                // Store data in all cells horizontally and vertically 
                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < length; j++) {
                        // Check how many times the cell has been used at the appropriate co-ordinates
                        // If it has been used more than once, set CellsUsedMoreThanOnce to true
                        if (material[(Int32.Parse(leftPosition) + i), (Int32.Parse(topPosition) + j)] > 1) 
                        {
                            claim.CellsUsedMoreThanOnce = true;
                        }
                    }
                }

                claims.Add(claim);
            }

            // Find the only claim that has no overlap 

            int finalClaimId = claims.First(c => !c.CellsUsedMoreThanOnce).ClaimId;
            Console.WriteLine("The ID of the only claim that doesn't overlap is: " + finalClaimId);
        }
    }
}