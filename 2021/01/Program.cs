using Creek.HelpfulExtensions;

// Part 1
int numberOfBiggerMeasurements = 0;

string[] lines = File.ReadAllLines("input.txt");

foreach (var (line, index) in lines.WithIndex())
{
    if (index > 0)
    {
        int previousReading = Int32.Parse(lines[index-1]);
        int currentReading = Int32.Parse(line);
        
        if (currentReading > previousReading)
        {
            numberOfBiggerMeasurements += 1;
        }
    }
}

Console.WriteLine($"Part 1: {numberOfBiggerMeasurements}");


// Part 2
int numberOfBiggerSlidingTotals = 0;

List<int> slidingTotals = new List<int>();

for (int i = 0; i < lines.Count(); i++) 
{
  if (lines.Count() > i+2)
  {
      slidingTotals.Add(Int32.Parse(lines[i]) + Int32.Parse(lines[i+1]) + Int32.Parse(lines[i+2]));
  }
}


foreach (var (total, index) in slidingTotals.WithIndex())
{
    if (index > 0)
    {
        int previousTotal = slidingTotals[index-1];
        int currentTotal = total;
        
        if (currentTotal > previousTotal)
        {
            numberOfBiggerSlidingTotals += 1;
        }
    }
}

Console.WriteLine($"Part 2: {numberOfBiggerSlidingTotals}");