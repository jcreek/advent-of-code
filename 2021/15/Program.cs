namespace Day15
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
            Dictionary<Position, int> cavernMap = GenerateCavernMap(lines);

            // Disjktra's algorithm

            Position startPosition = new Position(0, 0);
            Position endPosition = new Position(cavernMap.Keys.MaxBy(p => p.x).x, cavernMap.Keys.MaxBy(p => p.y).y);

            PriorityQueue<Position, int> priorityQueue = new PriorityQueue<Position, int>();
            Dictionary<Position, int> totalRiskMap = new Dictionary<Position, int>();

            totalRiskMap[startPosition] = 0;
            priorityQueue.Enqueue(startPosition, 0);

            while (priorityQueue.Count > 0)
            {
                Position position = priorityQueue.Dequeue();

                IEnumerable<Position> nearbyPositions = GetNearbyNonDiagonalPositions(position);

                foreach (Position nearbyPosition in nearbyPositions)
                {
                    // If it's in the cavern but we haven't calculated its risk yet
                    if (cavernMap.ContainsKey(nearbyPosition) && !totalRiskMap.ContainsKey(nearbyPosition))
                    {
                        // The total risk for this nearby position is the total risk for the current position,
                        // plus the value at this nearby position
                        int totalRisk = totalRiskMap[position] + cavernMap[nearbyPosition];
                        totalRiskMap[nearbyPosition] = totalRisk;

                        // If we've reached the end position of the cavern, then stop
                        if (nearbyPosition == endPosition)
                        {
                            break;
                        }

                        // Add the position to the queue
                        priorityQueue.Enqueue(nearbyPosition, totalRisk);
                    }
                }
            }

            // What is the lowest total risk of any path from the top left to the bottom right?
            Console.WriteLine(totalRiskMap[endPosition]);
        }

        static IEnumerable<Position> GetNearbyNonDiagonalPositions(Position position)
        {
            return new[] {
               position with {y = position.y + 1},
               position with {y = position.y - 1},
               position with {x = position.x + 1},
               position with {x = position.x - 1},
            };
        }

        static Dictionary<Position, int> GenerateCavernMap(string[] lines)
        {
            Dictionary<Position, int> keyValuePairs = new Dictionary<Position, int>();
            for (int x = 0; x < lines.Length; x++)
            {
                for (int y = 0; y < lines[x].Length; y++)
                {
                    // TODO - why is this "lines[y][x] - '0'" rather than "lines[x][y]"
                    // as I think it should be?
                    keyValuePairs.Add(new Position(x, y), lines[y][x] - '0');
                }
            }

            return keyValuePairs;
        }


    }

    record Position(int x, int y);
}
