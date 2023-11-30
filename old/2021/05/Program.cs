

using Creek.HelpfulExtensions;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            //Part1(lines);
            Part2(lines);
        }

        static void Part1(string[] lines)
        {
            int[,] ventLines = CreateVentLinesArray(lines, true);

            // determine the number of points where at least two lines overlap
            int counter = 0;

            for (int i = 0; i < ventLines.GetLength(0); i++)
            {
                for (int j = 0; j < ventLines.GetLength(1); j++)
                {
                    if (ventLines[i, j] >= 2)
                    {
                        counter += 1;
                    }
                }
            }

            Console.WriteLine($"At least two lines overlap at {counter} points");
        }

        static void Part2(string[] lines)
        {
            int[,] ventLines = CreateVentLinesArray(lines, false);

            // determine the number of points where at least two lines overlap
            int counter = 0;

            for (int i = 0; i < ventLines.GetLength(0); i++)
            {
                for (int j = 0; j < ventLines.GetLength(1); j++)
                {
                    if (ventLines[i, j] >= 2)
                    {
                        counter += 1;
                    }
                }
            }

            Console.WriteLine($"At least two lines overlap at {counter} points");
        }

        static int[,] CreateVentLinesArray(string[] lines, bool onlyVerticalAndHorizontal)
        {

            int[,] ventLines = new int[1000,1000];

            foreach (var line in lines)
            {
                // e.g. 645,570 -> 517,570
                int startX = int.Parse(line.Substring(0, line.IndexOf(",")));
                int startY = int.Parse(line.SubstringBetween(",", " ").Substring(1));
                int endX = int.Parse(line.SubstringBetween("-> ", ",", true).Substring(3));
                int endY = int.Parse(line.Substring(line.LastIndexOf(",") + 1));


                if (onlyVerticalAndHorizontal)
                {
                    if(startX == endX || startY == endY)
                    {
                        // They are not always in size order, so get their size order
                        int smallX = startX > endX ? endX : startX;
                        int bigX = startX > endX ? startX : endX;
                        int smallY = startY > endY ? endY : startY;
                        int bigY = startY > endY ? startY : endY;

                        for (int i = smallX; i <= bigX; i++)
                        {
                            for (int j = smallY; j <= bigY; j++)
                            {
                                ventLines[i, j] += 1;
                            }
                        }
                    }                
                }
                else
                {
                    if (startX == endX || startY == endY)
                    {
                        // They are not always in size order, so get their size order
                        int smallX = startX > endX ? endX : startX;
                        int bigX = startX > endX ? startX : endX;
                        int smallY = startY > endY ? endY : startY;
                        int bigY = startY > endY ? startY : endY;

                        for (int i = smallX; i <= bigX; i++)
                        {
                            for (int j = smallY; j <= bigY; j++)
                            {
                                ventLines[i, j] += 1;
                            }
                        }
                    }
                    else
                    {
                        // Diagonal

                        int currentX = startX;
                        int currentY = startY;

                        while (true)
                        {
                            ventLines[currentX, currentY] += 1;

                            if (currentX == endX && currentY == endY)
                            {
                                break;
                            }

                            if (startX > endX)
                            {
                                currentX -= 1;
                            }
                            else
                            {
                                currentX += 1;
                            }

                            if (startY > endY)
                            {
                                currentY -= 1;
                            }
                            else
                            {
                                currentY += 1;
                            }
                        }
                    }
                }
            }

            return ventLines;
        }
    }
}
