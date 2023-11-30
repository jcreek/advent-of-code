namespace Day03
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
            // Part 1
            string[] lines = File.ReadAllLines("input.txt");
            List<BinaryData> binaryDataList = new List<BinaryData>();

            foreach (string line in lines)
            {
                BinaryData binaryData = new BinaryData()
                {
                    Digit0 = Int32.Parse(line[0].ToString()),
                    Digit1 = Int32.Parse(line[1].ToString()),
                    Digit2 = Int32.Parse(line[2].ToString()),
                    Digit3 = Int32.Parse(line[3].ToString()),
                    Digit4 = Int32.Parse(line[4].ToString()),
                    Digit5 = Int32.Parse(line[5].ToString()),
                    Digit6 = Int32.Parse(line[6].ToString()),
                    Digit7 = Int32.Parse(line[7].ToString()),
                    Digit8 = Int32.Parse(line[8].ToString()),
                    Digit9 = Int32.Parse(line[9].ToString()),
                    Digit10 = Int32.Parse(line[10].ToString()),
                    Digit11 = Int32.Parse(line[11].ToString()),
                };

                binaryDataList.Add(binaryData);
            }

            // for first digit find most common, 2nd digit most common, etc to get the gamma rate
            int mostOccurred0 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit0).ToList());
            int mostOccurred1 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit1).ToList());
            int mostOccurred2 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit2).ToList());
            int mostOccurred3 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit3).ToList());
            int mostOccurred4 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit4).ToList());
            int mostOccurred5 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit5).ToList());
            int mostOccurred6 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit6).ToList());
            int mostOccurred7 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit7).ToList());
            int mostOccurred8 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit8).ToList());
            int mostOccurred9 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit9).ToList());
            int mostOccurred10 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit10).ToList());
            int mostOccurred11 = GetMostOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit11).ToList());

            string mostOccurred = "" + mostOccurred0 + mostOccurred1 + mostOccurred2 + mostOccurred3 + mostOccurred4 + mostOccurred5 + mostOccurred6 + mostOccurred7 + mostOccurred8 + mostOccurred9 + mostOccurred10 + mostOccurred11;

            // for first digit find least common, 2nd digit least common, etc to get the epsilon rate
            int leastOccurred0 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit0).ToList());
            int leastOccurred1 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit1).ToList());
            int leastOccurred2 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit2).ToList());
            int leastOccurred3 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit3).ToList());
            int leastOccurred4 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit4).ToList());
            int leastOccurred5 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit5).ToList());
            int leastOccurred6 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit6).ToList());
            int leastOccurred7 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit7).ToList());
            int leastOccurred8 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit8).ToList());
            int leastOccurred9 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit9).ToList());
            int leastOccurred10 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit10).ToList());
            int leastOccurred11 = GetLeastOccurredCharacterAtPosition(binaryDataList.Select(x => x.Digit11).ToList());

            string leastOccurred = "" + leastOccurred0 + leastOccurred1 + leastOccurred2 + leastOccurred3 + leastOccurred4 + leastOccurred5 + leastOccurred6 + leastOccurred7 + leastOccurred8 + leastOccurred9 + leastOccurred10 + leastOccurred11;

            Console.WriteLine($"MO: {mostOccurred} LO: {leastOccurred}");

            string gammaRate = string.Join("", mostOccurred);
            int gammaRateDenary = Convert.ToInt32(gammaRate, 2);
            string epsilonRate = string.Join("", leastOccurred);
            int epsilonRateDenary = Convert.ToInt32(epsilonRate, 2);

            // multiply gamma rate by epislon rate to get power consumption
            int powerConsumption = gammaRateDenary * epsilonRateDenary;

            Console.WriteLine($"Gamma rate: {gammaRate} Epsilon rate: {epsilonRate} Power consumption: {powerConsumption}");
        }
        
        static void Part2()
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<string> linesForOxygen = lines.ToList();
            List<string> linesForCo2 = lines.ToList();

            for (int i = 0; i < lines[0].Length; i++)
            {
                if (linesForOxygen.Count > 1)
                {
                    int mostCommon = GetMostOccurredCharacterAtPositionWithEqualHandling(linesForOxygen.Select(x => Int32.Parse(x[i].ToString())).ToList());
                    linesForOxygen = linesForOxygen.Where(x => x[i] == mostCommon.ToString()[0]).ToList();
                }

                if (linesForCo2.Count > 1)
                {
                    int leastCommon = GetLeastOccurredCharacterAtPositionWithEqualHandling(linesForCo2.Select(x => Int32.Parse(x[i].ToString())).ToList());
                    linesForCo2 = linesForCo2.Where(x => x[i] == leastCommon.ToString()[0]).ToList();
                }
            }

            Console.WriteLine($"Oxygen: {linesForOxygen[0]} CO2: {linesForCo2[0]}");

            int oxygenDenary = Convert.ToInt32(linesForOxygen[0], 2);
            int co2Denary = Convert.ToInt32(linesForCo2[0], 2);

            // life support rating = oxygen generator rating * co2 scrubber rating
            int lifeSupportRating = oxygenDenary * co2Denary;

            Console.WriteLine($"Oxygen: {oxygenDenary} CO2: {co2Denary} Life support rating: {lifeSupportRating}");
        }


        public static int GetMostOccurredCharacterAtPositionWithEqualHandling(List<int> list)
        {
            var count0 = list.Where(x => x == 0).Count();
            var count1 = list.Where(x => x == 1).Count();

            if (count0 == count1)
            {
                return 1;
            }
            else
            {
                if (count0 > count1)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        public static int GetLeastOccurredCharacterAtPositionWithEqualHandling(List<int> list)
        {
            var count0 = list.Where(x => x == 0).Count();
            var count1 = list.Where(x => x == 1).Count();

            if (count0 == count1)
            {
                return 0;
            }
            else
            {
                if (count0 > count1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static int GetMostOccurredCharacterAtPosition(List<int> list) 
        {
            return list
                .GroupBy(x => x)
                .OrderByDescending(group => group.Count())
                .Select(x => x.Key)
                .First();
        }

        public static int GetLeastOccurredCharacterAtPosition(List<int> list) 
        {
            return list
                .GroupBy(x => x)
                .OrderBy(group => group.Count())
                .Select(x => x.Key)
                .First();
        }
    }
}