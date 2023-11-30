namespace Day11
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
            int[,] octopusses = new int[10, 10];

            // Load initial values
            for (int i = 0; i < lines.Length; i++)
            {
                for (int k = 0; k < lines[i].Length; k++)
                {
                    octopusses[i, k] = int.Parse(lines[i][k].ToString());
                }
            }

            Console.WriteLine("Begin");

            int totalFlashes = 0; 
            // How many total flashes are there after 100 steps?
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Start {i}");
                PerformStep(ref octopusses, ref totalFlashes);
                Console.WriteLine($"End {i}");
            }

            Console.WriteLine($"There were {totalFlashes} total flashes after 100 steps.");
        }

        static void PerformStep(ref int[,] octopusses, ref int totalFlashes)
        {
            // First, the energy level of each octopus increases by 1.
            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    octopusses[i, k] += 1;
                }
            }

            bool[,] octopussFlashedDuringThisStep = new bool[10, 10];

            // Then, any octopus with an energy level greater than 9 flashes.
            // This increases the energy level of all adjacent octopuses by 1, including octopuses that
            // are diagonally adjacent. If this causes an octopus to have an energy level greater than 9,
            // it also flashes.This process continues as long as new octopuses keep having their energy
            // level increased beyond 9. (An octopus can only flash at most once per step.)

            bool stillFlashing = true;

            while (stillFlashing)
            {
                // Loop through all
                for (int i = 0; i < 10; i++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        // mark any with a 9 as having flashed
                        if (octopusses[i, k] == 9)
                        {
                            if (!octopussFlashedDuringThisStep[i, k])
                            {
                                octopussFlashedDuringThisStep[i, k] = true;
                                totalFlashes += 1;

                                // handle any incrementing of neighbours of those who have flashed
                                List<(int x, int y)> neighbours = FindAdjacentCellsInclDiagonals((i, k), ref octopusses);

                                foreach ((int x, int y) neighbour in neighbours)
                                {
                                    octopusses[neighbour.x, neighbour.y] += 1;
                                }
                            }
                        }
                    }
                }

                // if there are no 9s left then stop flashing
                if (!ValueIsIn2dArray(9, octopusses))
                {
                    stillFlashing = false;
                }
            }



            // Finally, any octopus that flashed during this step has its energy level set to 0, as it
            // used all of its energy to flash.
            for (int i = 0; i < 10; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    if (octopussFlashedDuringThisStep[i, k] == true)
                    {
                        octopusses[i, k] = 0; ;
                    }
                }
            }
        }

        static bool ValueIsIn2dArray(int value, int[,] twoDimensionalArray)
        {
            for (int x = 0; x < twoDimensionalArray.GetLength(0); x++)
            {
                for (int y = 0; y < twoDimensionalArray.GetLength(1); y++)
                {
                    if (twoDimensionalArray[x, y] == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static List<(int x, int y)> FindAdjacentCellsInclDiagonals((int x, int y) currentCell, ref int[,] twoDimensionalArray)
        {
            List<(int x, int y)> list = new List<(int x, int y)>();

            if (currentCell.x >= 1 && currentCell.y >= 1)
            {
                list.Add((currentCell.x - 1, currentCell.y - 1));
            }

            if (currentCell.y >= 1)
            {
                list.Add((currentCell.x, currentCell.y - 1));
            }

            if (currentCell.x < twoDimensionalArray.GetLength(0) - 1 && currentCell.y >= 1)
            {
                list.Add((currentCell.x + 1, currentCell.y - 1));
            }

            if (currentCell.x >= 1)
            {
                list.Add((currentCell.x - 1, currentCell.y));
            }

            if (currentCell.x < twoDimensionalArray.GetLength(0) - 1)
            {
                list.Add((currentCell.x + 1, currentCell.y));
            }

            if (currentCell.x >= 1 && currentCell.y < twoDimensionalArray.GetLength(1) - 1)
            {
                list.Add((currentCell.x - 1, currentCell.y + 1));
            }

            if (currentCell.y < twoDimensionalArray.GetLength(1) - 1)
            {
                list.Add((currentCell.x, currentCell.y + 1));
            }

            if (currentCell.x < twoDimensionalArray.GetLength(0) - 1 && currentCell.y < twoDimensionalArray.GetLength(1) - 1)
            {
                list.Add((currentCell.x + 1, currentCell.y + 1));
            }

            return list;
        }
    }
}