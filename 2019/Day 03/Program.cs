using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day_03
{
    class Program
    {
        static void Main(string[] args)
        {
            //Part1();
            Part2();
        }

        public class WireTouchPoint
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        static List<WireTouchPoint> FindAllWireTouchPoints(List<string> wireCommands)
        {
            List<WireTouchPoint> touchPoints = new List<WireTouchPoint>(){};
            int currentX = 0;
            int currentY = 0;
            WireTouchPoint initialWTP = new WireTouchPoint();
            initialWTP.x = currentX;
            initialWTP.y = currentY;
            touchPoints.Add(initialWTP);

            // For each command, record every co-ordinate the wire touches
            foreach(string wireCommand in wireCommands)
            {
                // Not bothering with error catching for converting strings to ints as the data cannot be wrongly typed in the input file
                int distance = System.Convert.ToInt32(wireCommand.Substring(1));

                // Handle the different commands in the first letter of each command string
                switch (wireCommand[0])
                {
                    case 'U':
                        // Up
                        for(int i = 0; i < distance; i++)
                        {
                            currentY += 1;

                            WireTouchPoint wtp = new WireTouchPoint();
                            wtp.x = currentX;
                            wtp.y = currentY;
                            touchPoints.Add(wtp);
                        }
                        break;
                    case 'D':
                        // Down
                        for(int i = 0; i < distance; i++)
                        {
                            currentY -= 1;

                            WireTouchPoint wtp = new WireTouchPoint();
                            wtp.x = currentX;
                            wtp.y = currentY;
                            touchPoints.Add(wtp);
                        }
                        break;
                    case 'L':
                        // Left
                        for(int i = 0; i < distance; i++)
                        {
                            currentX -= 1;

                            WireTouchPoint wtp = new WireTouchPoint();
                            wtp.x = currentX;
                            wtp.y = currentY;
                            touchPoints.Add(wtp);
                        }
                        break;
                    case 'R':
                        // Right
                        for(int i = 0; i < distance; i++)
                        {
                            currentX += 1;

                            WireTouchPoint wtp = new WireTouchPoint();
                            wtp.x = currentX;
                            wtp.y = currentY;
                            touchPoints.Add(wtp);
                        }
                        break;
                    default:
                        break;
                }
            }
            return touchPoints;
        }

        internal class WireTouchPointComparer : IEqualityComparer<WireTouchPoint>
        {
            public bool Equals(WireTouchPoint wtp1, WireTouchPoint wtp2)
            {
                if(wtp1.x == wtp2.x && wtp1.y == wtp2.y)
                {
                    return true;
                }
                return false;
            }

            // public int GetHashCode(WireTouchPoint obj)
            // {
            //     return obj.x.GetHashCode();
            // }

            public int GetHashCode(WireTouchPoint obj)
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + obj.x.GetHashCode();
                    hash = hash * 23 + obj.y.GetHashCode();
                    return hash;
                }
            }
        }

        static void Part1()
        {
            // Each line is one wire's path trace
            var wire1 = File.ReadAllLines("input.txt")[0];
            var wire2 = File.ReadAllLines("input.txt")[1];

            List<string> wire1Commands = wire1.Split(',').ToList();
            List<string> wire2Commands = wire2.Split(',').ToList();

            // Find all points each wire touches 
            List<WireTouchPoint> wire1TouchPoints = FindAllWireTouchPoints(wire1Commands);
            List<WireTouchPoint> wire2TouchPoints = FindAllWireTouchPoints(wire2Commands);

            // Find all points where the two wires cross 
            var crossingPoints = wire1TouchPoints.Intersect(wire2TouchPoints, new WireTouchPointComparer()).Where(cp => cp.x != 0 && cp.y != 0);

            // Declare a large default shortest distance, probably should be setting as null and null-checking later but multitasking while writing this so cba
            int shortestManhattanDistance = 9999999;
            // For each crossing point, work out the Manhattan distance to the central port 
            foreach(WireTouchPoint crossingPoint in crossingPoints)
            {
                // Convert co-ordinates to be positive numbers for calculating distance
                int xVal = crossingPoint.x > 0 ? crossingPoint.x : crossingPoint.x * -1;
                int yVal = crossingPoint.y > 0 ? crossingPoint.y : crossingPoint.y * -1;
                
                int manhattanDistance = (xVal - 0) + (yVal - 0);
                // Work out if it is shorter than the previously found one
                if(manhattanDistance < shortestManhattanDistance)
                {
                    shortestManhattanDistance = manhattanDistance;
                }
            }
            Console.WriteLine("Part 1: The Manhattan distance from the central port to the closest intersection is " + shortestManhattanDistance);
        }

        static void Part2()
        {
           // Each line is one wire's path trace
            var wire1 = File.ReadAllLines("input.txt")[0];
            var wire2 = File.ReadAllLines("input.txt")[1];

            List<string> wire1Commands = wire1.Split(',').ToList();
            List<string> wire2Commands = wire2.Split(',').ToList();

            // Find all points each wire touches 
            List<WireTouchPoint> wire1TouchPoints = FindAllWireTouchPoints(wire1Commands);
            List<WireTouchPoint> wire2TouchPoints = FindAllWireTouchPoints(wire2Commands);

            // Find all points where the two wires cross 
            var crossingPoints = wire1TouchPoints.Intersect(wire2TouchPoints, new WireTouchPointComparer()).Where(cp => cp.x != 0 && cp.y != 0);

            // Declare a large default minimum steps, probably should be setting as null and null-checking later but multitasking while writing this so cba
            int minimumSteps = 9999999;
            foreach(WireTouchPoint crossingPoint in crossingPoints)
            {
                // Calculate the number of steps each wire takes to reach each the intersection
                // If a wire visits a position on the grid multiple times, use the steps value from the first time it visits that position when calculating the total value of a specific intersection.
                // The number of steps a wire takes is the total number of grid squares the wire has entered to get to that location, including the intersection being considered.

                // Calculate the minimum number of steps each wire takes to reach the crossing point (using the index rather than index + 1 as the lists are zero-indexed ordered by the route taken but we want to ignore the start co-ordinates) - this will always return the first (i.e. closest) index
                int steps1 = wire1TouchPoints.FindIndex(tp => tp.x == crossingPoint.x && tp.y == crossingPoint.y);
                int steps2 = wire2TouchPoints.FindIndex(tp => tp.x == crossingPoint.x && tp.y == crossingPoint.y);
                
                int totalSteps = steps1 + steps2;
                // Work out if it is fewer steps than the previously found one
                if(totalSteps < minimumSteps)
                {
                    minimumSteps = totalSteps;
                }
            }
            Console.WriteLine("Part 2: The fewest combined steps the wires must take to reach an intersection is " + minimumSteps);
        }
    }
}
