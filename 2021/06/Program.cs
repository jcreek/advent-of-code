namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllLines("input.txt")[0];
            List<int> lanternFish = input.Split(',').Select(int.Parse).ToList();


            //Part1(lanternFish);
            Part2(lanternFish);
        }

        static void SimulateLanternFish(ref List<int> lanternFishPopulation, int daysToSimulate)
        {
            // A lanternfish that creates a new fish resets its timer to 6, not 7 (because 0 is included as a valid timer value). The new lanternfish starts with an internal timer of 8 and does not start counting down until the next day.

            int currentDay = 1;

            while (currentDay <= daysToSimulate)
            {
                Console.WriteLine($"Day: {currentDay} - there are {lanternFishPopulation.Count()} lantern fish");
                for (int i = 0; i < lanternFishPopulation.Count; i++)
                {
                    switch(lanternFishPopulation[i]) 
                    {
                    case 0:
                        // its internal timer would reset to 6, and it would create a new lanternfish with an internal timer of 8.
                        lanternFishPopulation[i] = 6;
                        // Adding a new one with 9 though it should be 8 as this loop will catch new fish and decrement them. It's dirty, but fast way to solve the issue
                        lanternFishPopulation.Add(9);
                        break;
                    default:
                        // remove one day from the fish's countdown
                        lanternFishPopulation[i] -= 1;
                        break;
                    }
                }

                currentDay += 1;
            }
        }

        static void Part1(List<int> lanternFish)
        {
            SimulateLanternFish(ref lanternFish, 80);

            Console.WriteLine($"After 80 days there are {lanternFish.Count()} lantern fish.");
        }

        static void Part2(List<int> lanternFish)
        {
            SimulateLanternFish(ref lanternFish, 256);

            // TODO - redo this using a database for large datasets, and entity framework, so as to avoid the HUGE memory usage
            
            Console.WriteLine($"After 256 days there are {lanternFish.Count()} lantern fish.");
        }
    }
}