using System.Reflection;

namespace AOC.Tests.Y2022
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class Day02
    {
        protected string GetThisClassName() { return this.GetType().Name; }
        private string[] realData;

        [SetUp]
        public void Setup()
        {
            realData = File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Y2022", "Data", $"{GetThisClassName()}.dat"));
        }

        [TestCase(@"A Y
B X
C Z", 15)]
        [TestCase(null, 8392)] // The actual answer
        public void Part1(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            int result = CalculateTotalScore(lines);

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 1: {result}");
        }

        [TestCase(@"A Y
B X
C Z", 12)]
        [TestCase(null, 1783)] // The actual answer
        public void Part2(string input, int? expected)
        {
            string[] lines = input != null ? input.Split("\n") : realData;

            int result = CalculateTotalScoreV2(lines);

            if (expected != null)
            {
                Assert.That(result, Is.EqualTo(expected.Value));
            }

            Console.WriteLine($"Part 2: {result}");
        }

        private int CalculateTotalScore(string[] lines)
        {
            int totalScore = 0;
            foreach (string line in lines)
            {
                // The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
                int scoreForMyShape = line[2] switch
                {
                    'X' => 1, // Rock
                    'Y' => 2, // Paper
                    'Z' => 3, // Scissors
                    _ => 0 // Invalid
                };

                // plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).
                int scoreForOutcome = DetermineWinOrLoss(line[0], line[2]);

                totalScore += scoreForMyShape + scoreForOutcome;
            }

            return totalScore;
        }

        private int DetermineWinOrLoss(char opponentShape, char myShape)
        {
            if ((opponentShape is 'A' && myShape is 'Y')
                || (opponentShape is 'B' && myShape is 'Z')
                || (opponentShape is 'C' && myShape is 'X'))
            {
                // I win
                return 6;
            }
            else if ((opponentShape is 'A' && myShape is 'Z')
                || (opponentShape is 'B' && myShape is 'X')
                || (opponentShape is 'C' && myShape is 'Y'))
            {
                // They win
                return 0;
            }
            else
            {
                // Draw
                return 3;
            }
        }

        private int CalculateTotalScoreV2(string[] lines)
        {
            // the second column says how the round needs to end:
            // X means you need to lose, Y means you need to end the round in a draw,
            // and Z means you need to win.
            int totalScore = 0;
            foreach (string line in lines)
            {
                // Determine what the outcome should be
                int outcome = line[2] switch
                {
                    'X' => 0, // Lose
                    'Y' => 3, // Draw
                    'Z' => 6, // Win
                    _ => 0 // Invalid
                };

                char myShape = GetShapeForOutcome(outcome, line[0]);

                // The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
                int scoreForMyShape = myShape switch
                {
                    'X' => 1, // Rock
                    'Y' => 2, // Paper
                    'Z' => 3, // Scissors
                    _ => 0 // Invalid
                };

                // plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).
                int scoreForOutcome = DetermineWinOrLoss(line[0], myShape);

                totalScore += scoreForMyShape + scoreForOutcome;
            }

            return totalScore;
        }

        private char GetShapeForOutcome(int outcome, char opponentShape)
        {
            if (outcome is 0)
            {
                // Lose
                return opponentShape switch
                {
                    'A' => 'Z',
                    'B' => 'X',
                    'C' => 'Y',
                    _ => throw new InvalidDataException("Invalid opponent shape") // Invalid
                };
            }
            else if (outcome is 3)
            {
                // Draw
                return opponentShape switch
                {
                    'A' => 'X',
                    'B' => 'Y',
                    'C' => 'Z',
                    _ => throw new InvalidDataException("Invalid opponent shape") // Invalid
                };
            }
            else if (outcome is 6)
            {
                // Win
                return opponentShape switch
                {
                    'A' => 'Y',
                    'B' => 'Z',
                    'C' => 'X',
                    _ => throw new InvalidDataException("Invalid opponent shape") // Invalid
                };
            }
            else
            {
                throw new InvalidDataException("Invalid outcome");
            }
        }
    }
}