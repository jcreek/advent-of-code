using Creek.HelpfulExtensions;

namespace Day09
{
    class Program
    {
        public static int arraySizeX;
        public static int arraySizeY;
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            

            //Part1(lines);
            Part2(lines);
        }

        static void Part1(string[] lines)
        {
            arraySizeX = lines[0].Length;
            arraySizeY = lines.Count();
            int[,] array = new int[arraySizeX, arraySizeY];
            foreach (var (line, index) in lines.WithIndex())
            {
                for (int i = 0; i < line.Length; i++)
                {
                    array[i,index] = int.Parse(line[i].ToString());
                }
            }

            List<int> lowPoints = new List<int>();

            // Check each number in the array
            for (int i = 0; i < arraySizeX; i++)
            {
                for (int j = 0; j < arraySizeY; j++)
                {
                    if (IsLowPoint(array, i, j))
                    {
                        lowPoints.Add(array[i,j]);
                    }
                }
            }

            // Risk level of a low point is 1 plus height 
            int totalRiskLevel = lowPoints.Sum() + lowPoints.Count();

            Console.WriteLine($"Total Risk Level: {totalRiskLevel}");
        }

        static void Part2(string[] lines)
        {
            arraySizeX = lines[0].Length;
            arraySizeY = lines.Count();
            int[,] array = new int[arraySizeX, arraySizeY];
            foreach (var (line, index) in lines.WithIndex())
            {
                for (int i = 0; i < line.Length; i++)
                {
                    array[i, index] = int.Parse(line[i].ToString());
                }
            }

            List<(int x, int y)> lowPointLocations = new List<(int x, int y)>();

            // Check each number in the array
            for (int i = 0; i < arraySizeX; i++)
            {
                for (int j = 0; j < arraySizeY; j++)
                {
                    if (IsLowPoint(array, i, j))
                    {
                        lowPointLocations.Add((i, j));
                    }
                }
            }

            // There is a basin for every low point, bounded by 9s and the edge of the array
            // Find these basins, along with the count of how many locations are in each basin
            List<int> basinLocationCount = new List<int>();

            foreach ((int x, int y) location in lowPointLocations)
            {
                // For each low point, find all the locations within its basin
                Console.WriteLine("Starting new basin");

                List<(int x, int y)> locationsInBasin = new List<(int x, int y)>();

                FindBasinLocations(array, ref locationsInBasin, location);

                basinLocationCount.Add(locationsInBasin.Count());
            }



            Console.WriteLine($"Total Basin Locations: {basinLocationCount.Count()}");
        }

        static void FindBasinLocations(int[,] array, ref List<(int x, int y)> locationsInBasin, (int x, int y) location)
        {
            List<(int x, int y)> relativeLocations = new List<(int x, int y)>()
            {
                //(location.x - 1, location.y - 1),
                //(location.x, location.y - 1),
                //(location.x + 1, location.y - 1),
                //(location.x - 1, location.y),
                //(location.x + 1, location.y),
                //(location.x - 1, location.y + 1),
                //(location.x, location.y + 1),
                //(location.x + 1, location.y + 1),
            };

            try
            {
                relativeLocations.Add((location.x - 1, location.y - 1));
                relativeLocations.Add((location.x, location.y - 1));
                relativeLocations.Add((location.x + 1, location.y - 1));
                relativeLocations.Add((location.x - 1, location.y));
                relativeLocations.Add((location.x + 1, location.y));
                relativeLocations.Add((location.x - 1, location.y + 1));
                relativeLocations.Add((location.x, location.y + 1));
                relativeLocations.Add((location.x + 1, location.y + 1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{location.x},{location.y} errored");
            }

            // For every direction, if there's a 9 or no element stop, otherwise, go again from that location, excluding locations we've already considered
            foreach ((int x, int y) relativeLocation in relativeLocations)
            {
                int newX = location.x + relativeLocation.x;
                int newY = location.y + relativeLocation.y;

                bool condition1 = IsNotHighPointOrOutOfArray(array, newX, newY);
                bool condition2 = condition1 ? array[newX, newY] < 9 : false;
                bool condition3 = condition1 ? !locationsInBasin.Contains((newX, newY)) : false;

                //Console.WriteLine($"{condition1} - {condition2} - {condition3}");

                if (condition1 && condition2 && condition3)
                {
                    locationsInBasin.Add((newX, newY));
                    Console.WriteLine($"{newX},{newY} added with value {array[newX, newY]}");
                    FindBasinLocations(array, ref locationsInBasin, relativeLocation);
                }
            }
        }

        static bool IsNotHighPointOrOutOfArray(int[,] array, int x, int y)
        {
            if (IsInArray(x, y) && array[x, y] < 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool IsLowPoint(int[,] array, int x, int y)
        {
            // top left x-1, y-1
            // top middle x, y-1
            // top right x+1, y-1
            // left x-1, y
            // right x+1, y
            // bottom left x-1, y+1
            // bottom middle x, y+1
            // bottom right x+1, y+1 

            // Compare all EXISTING positions around the current position and if any of them are lowest than the value in the current position return false
            int currentPosition = array[x, y];

            if (IsInArray(x-1, y-1) && array[x-1, y-1] < currentPosition)
            {
                return false;
            }
            else if (IsInArray(x, y - 1) && array[x, y - 1] < currentPosition)
            {
                return false;
            }
            else if (IsInArray(x + 1, y - 1) && array[x + 1, y - 1] < currentPosition)
            {
                return false;
            }
            else if (IsInArray(x - 1, y) && array[x - 1, y] < currentPosition)
            {
                return false;
            }
            else if (IsInArray(x + 1, y) && array[x + 1, y] < currentPosition)
            {
                return false;
            }
            else if (IsInArray(x - 1, y + 1) && array[x - 1, y + 1] < currentPosition)
            {
                return false;
            }
            else if (IsInArray(x, y + 1) && array[x, y + 1] < currentPosition)
            {
                return false;
            }
            else if (IsInArray(x + 1, y + 1) && array[x + 1, y + 1] < currentPosition)
            {
                return false;
            }
            else
            {
                Console.WriteLine($"{x},{y}: {currentPosition}");
                return true;
            }
        }

        static bool IsInArray(int x, int y)
         {
            if (x >= 0 && x < arraySizeX && y >= 0 && y < arraySizeY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}