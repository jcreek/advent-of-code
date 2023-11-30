namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllLines("input.txt")[0];
            List<int> initialCrabPositions = input.Split(',').Select(int.Parse).ToList();

            //Part1(initialCrabPositions);
            Part2(initialCrabPositions);
        }

        static void Part1(List<int> initialCrabPositions)
        {
            // double mean = initialCrabPositions.Average();

            // int meanAverage = Convert.ToInt32(mean);

            int medianAverage = initialCrabPositions.OrderBy(x=>x).Skip(initialCrabPositions.Count()/2).First();

            // int modalAverage = initialCrabPositions.GroupBy(n=> n).
            //     OrderByDescending(g=> g.Count()).
            //     Select(g => g.Key).FirstOrDefault();

            // Console.WriteLine($"Mean average: {meanAverage}");
            Console.WriteLine($"Median average: {medianAverage}");
            // Console.WriteLine($"Modal average: {modalAverage}");

            int totalFuelConsumption = 0; 

            foreach (int position in initialCrabPositions)
            {
                // Find the difference
                totalFuelConsumption += Math.Abs(position - medianAverage);
            }

            Console.WriteLine($"Total fuel consumption: {totalFuelConsumption}");
        }

        static void Part2(List<int> initialCrabPositions)
        {
            // Calculate optimal position 
            // Loop through all possible positions and work out the total fuel consumption
            int minPosition = initialCrabPositions.Min();
            int maxPosition = initialCrabPositions.Max();

            Dictionary<int, int> possiblePositions = new Dictionary<int, int>();

            for (int i = minPosition; i <= maxPosition; i++)
            {
                possiblePositions.Add(i,CalculateTotalFuelConsumption(initialCrabPositions, i));
            }

            // Find lowest fuel consumption
            var lowestFuelConsumptionPosition = possiblePositions.OrderBy(kvp => kvp.Value).First();
            Console.WriteLine($"Position: {lowestFuelConsumptionPosition.Key} Fuel consumption: {lowestFuelConsumptionPosition.Value}");
        }

        public static int CalculateTotalFuelConsumption(List<int> initialCrabPositions, int targetPosition)
        {
            int totalFuelConsumption = 0; 

            foreach (int position in initialCrabPositions)
            {
                // Find the difference
                int difference = Math.Abs(position - targetPosition);
                totalFuelConsumption += Convert.ToInt32(GetTriangularNumber(difference));
            }

            return totalFuelConsumption;
        }

        public static double GetTriangularNumber(int n)
        {
            return n*(n + 1) / 2;
        }
    }
}