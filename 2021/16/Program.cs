using System.Text;
using System.Text.RegularExpressions;

namespace Day16
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
            string hex = lines[0];
            string binary = Convert.ToString(Convert.ToInt32(hex, 16), 2);

            List<int> versionNumbers = new List<int>();
            List<int> outputList = new List<int>();

            HandlePacket(ref versionNumbers, ref outputList, binary);

            int total = versionNumbers.Sum(x => x);

            Console.WriteLine($"The sum of version numbers in all packets is: {total}");
        }

        static void HandlePacket(ref List<int> versionNumbers, ref List<int> outputList, string binary)
        {
            int packetVersion = Convert.ToInt32(binary.Substring(0, 3), 2);
            versionNumbers.Add(packetVersion);

            int typeId = Convert.ToInt32(binary.Substring(3, 3), 2);

            string remainingBinary = binary.Substring(6, binary.Length - 6);

            if (typeId == 4)
            {
                // Packets represent a literal value
                /*
                 * Literal value packets encode a single binary number. To do this, the binary number 
                 * is padded with leading zeroes until its length is a multiple of four bits, and then 
                 * it is broken into groups of four bits. Each group is prefixed by a 1 bit except the last 
                 * group, which is prefixed by a 0 bit. These groups of five bits immediately follow the packet header.
                */
                StringBuilder binaryLiteral = new StringBuilder();
                string unprocessedBinary = string.Empty;

                for (int i = 0; i < remainingBinary.Length; i += 5)
                {
                    binaryLiteral.Append(remainingBinary.Substring(i + 1, 4));

                    if (remainingBinary[i] == '0')
                    {
                        // last group, end of packet
                        unprocessedBinary = remainingBinary.Substring(i + 5, remainingBinary.Length - i);
                        break;
                    }
                }

                string binaryLiteralString = binaryLiteral.ToString();
                int output = Convert.ToInt32(binaryLiteralString, 2);

                outputList.Add(output);

                // if the unprocessed binary doesn't just contain zeros process it again
                if (!Regex.IsMatch(unprocessedBinary, @"^(0+)$"))
                {
                    HandlePacket(ref versionNumbers, ref outputList, unprocessedBinary);
                }
            }
            else
            {
                // Packets represent an operator
                char lengthTypeId = remainingBinary[0];

                if (lengthTypeId == '0')
                {
                    // the next _15_ bits are a number that represents the _total length in bits_ of the
                    // sub-packets contained by this packet.

                    int totalLengthInBitsOfSubPacketsContainedByThisPacket = Convert.ToInt32(remainingBinary.Substring(1, 15), 2);

                    string binaryToStillProcess = remainingBinary.Substring(16, totalLengthInBitsOfSubPacketsContainedByThisPacket);

                    HandlePacket(ref versionNumbers, ref outputList, binaryToStillProcess);
                }
                else if (lengthTypeId == '1')
                {
                    // the next _11_ bits are a number that represents the _number of sub-packets
                    // immediately contained_ by this packet.

                    int numberOfSubPacketsImmediatelyContainedByThisPacket = Convert.ToInt32(remainingBinary.Substring(1, 11), 2);

                    // TODO - I can't see how I can even use this information

                    string binaryToStillProcess = remainingBinary.Substring(16, remainingBinary.Length - 16);

                    HandlePacket(ref versionNumbers, ref outputList, binaryToStillProcess);
                }
            }
        }
    }
}