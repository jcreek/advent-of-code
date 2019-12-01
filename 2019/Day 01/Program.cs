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
            //Test();
        }

        static void Part1()
        {
            // Fuel required to launch a given module is based on its mass. Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.

            var totalFuel = 0;

            var lines = File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                var mass = Int32.Parse(line);
                //divide by 3
                decimal dividedAnswer = mass / 3;
                // round down & subtract 2
                var answer = Convert.ToInt32(Math.Floor(dividedAnswer)) - 2;
                // add to total fuel count
                totalFuel += answer;
                
                
            }
            // The Fuel Counter-Upper needs to know the total fuel requirement. To find it, individually calculate the fuel needed for the mass of each module (your puzzle input), then add together all the fuel values.

            // What is the sum of the fuel requirements for all of the modules on your spacecraft?
            Console.WriteLine("Part 1: The sum of the fuel requirements for all of the modules on your spacecraft is " + totalFuel);
        }

        static int CalculateFuelNeeded(int mass)
        {
            //divide by 3
            decimal dividedAnswer = mass / 3;
            // round down & subtract 2
            var answer = Convert.ToInt32(Math.Floor(dividedAnswer)) - 2;
            // add to total fuel count
            return answer;
        }

        static void Part2()
        {
            List<int> fuels = new List<int>();

            var lines = File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                var mass = Int32.Parse(line);
                
                // Calculate the fuel needed for the mass
                var originalMassFuel = CalculateFuelNeeded(mass);

                var nextFuel = CalculateFuelNeeded(originalMassFuel);
                var totalFuel = originalMassFuel;

                // Now calculate the fuel needed for the mass of the fuel, ad nauseum

                // A module of mass 14 requires 2 fuel. This fuel requires no further fuel (2 divided by 3 and rounded down is 0, which would call for a negative fuel), so the total fuel required is still just 2.
                // At first, a module of mass 1969 requires 654 fuel. Then, this fuel requires 216 more fuel (654 / 3 - 2). 216 then requires 70 more fuel, which requires 21 fuel, which requires 5 fuel, which requires no further fuel. So, the total fuel required for a module of mass 1969 is 654 + 216 + 70 + 21 + 5 = 966.

                var keepCalculating = true;

                if(nextFuel > 0)
                {
                    Console.WriteLine(nextFuel);
                    while(keepCalculating)
                    {
                        totalFuel += nextFuel;
                        nextFuel = CalculateFuelNeeded(nextFuel);
                        Console.WriteLine(nextFuel);
                        if(nextFuel <= 0)
                        {
                            keepCalculating = false;
                        }
                    }
                }
                
                fuels.Add(totalFuel);
                
            }
            // The Fuel Counter-Upper needs to know the total fuel requirement. To find it, individually calculate the fuel needed for the mass of each module (your puzzle input), then add together all the fuel values.

            int finalTotal = fuels.Sum(x => x);
            // What is the sum of the fuel requirements for all of the modules on your spacecraft?
            Console.WriteLine("Part 2: The sum of the fuel requirements for all of the modules on your spacecraft when also taking into account the mass of the added fuel is " + finalTotal);
        }
    }
}
