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
            Queue<int> bitQueue = new Queue<int>();

            for (int i = 0; i < hex.Length; i++)
            {
                // Convert each hex character into binary
                string currentHexCharacter = Convert.ToString(Convert.ToInt32(hex[i].ToString(), 16), 2);

                // Store each bit into the list
                for (int j = 0; j < currentHexCharacter.Length; j++)
                {
                    bitQueue.Enqueue(int.Parse(currentHexCharacter[j].ToString()));
                }
            }

            List<int> versionNumbers = new List<int>();
            List<int> outputList = new List<int>();

            HandlePacket(ref versionNumbers, ref outputList, bitQueue);

            foreach (var item in versionNumbers)
            {
                Console.WriteLine(item);
            }

            int total = versionNumbers.Sum(x => x);

            Console.WriteLine($"The sum of version numbers in all packets is: {total}");
        }

        static void HandlePacket(ref List<int> versionNumbers, ref List<int> outputList, Queue<int> bitQueue)
        {
            int packetVersion = Convert.ToInt32($"{bitQueue.Dequeue()}{bitQueue.Dequeue()}{bitQueue.Dequeue()}", 2);
            versionNumbers.Add(packetVersion);

            int typeId = Convert.ToInt32($"{bitQueue.Dequeue()}{bitQueue.Dequeue()}{bitQueue.Dequeue()}", 2);

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
                bool isEndOfPacket = false;

                while (!isEndOfPacket)
                {
                    int endOfPacketIdentifier = bitQueue.Dequeue();

                    binaryLiteral.Append($"{bitQueue.Dequeue()}{bitQueue.Dequeue()}{bitQueue.Dequeue()}{bitQueue.Dequeue()}");

                    if (endOfPacketIdentifier == 0)
                    {
                        // last group, end of packet
                        isEndOfPacket = true;
                    }
                }

                string binaryLiteralString = binaryLiteral.ToString();
                int output = Convert.ToInt32(binaryLiteralString, 2);

                outputList.Add(output);

                // if the unprocessed binary doesn't just contain zeros process it again
                if (bitQueue.Count >= 11 && bitQueue.Any(b => (b != 0)))
                {
                    HandlePacket(ref versionNumbers, ref outputList, bitQueue);
                }
            }
            else
            {
                // Packets represent an operator
                int lengthTypeId = bitQueue.Dequeue();

                if (lengthTypeId == 0)
                {
                    // the next _15_ bits are a number that represents the _total length in bits_ of the
                    // sub-packets contained by this packet.

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < 15 && bitQueue.Count > 0; i++)
                    {
                        sb.Append(bitQueue.Dequeue());
                    }

                    int totalLengthInBitsOfSubPacketsContainedByThisPacket = Convert.ToInt32(sb.ToString(), 2);


                    Queue<int> newBitQueue = new Queue<int>();

                    for (int i = 0; i < totalLengthInBitsOfSubPacketsContainedByThisPacket && bitQueue.Count > 0; i++)
                    {
                        newBitQueue.Enqueue(bitQueue.Dequeue());
                    }

                    if (bitQueue.Count >= 11)
                    {
                        HandlePacket(ref versionNumbers, ref outputList, newBitQueue);
                    }
                }
                else if (lengthTypeId == 1)
                {
                    // the next _11_ bits are a number that represents the _number of sub-packets
                    // immediately contained_ by this packet.

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < 11 && bitQueue.Count > 0; i++)
                    {
                        sb.Append(bitQueue.Dequeue());
                    }

                    int numberOfSubPacketsImmediatelyContainedByThisPacket = Convert.ToInt32(sb.ToString(), 2);

                    // TODO - I can't see how I can even use this information

                    if (bitQueue.Count >= 11)
                    {
                        HandlePacket(ref versionNumbers, ref outputList, bitQueue);
                    }
                }
            }
        }
    }
}