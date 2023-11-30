// Part 1
string[] lines = File.ReadAllLines("input.txt");

int horizontalPosition = 0;
int verticalPosition = 0;

foreach (string line in lines)
{
    string[] parts = line.Split(' ');

    if (parts[0] == "forward")
    {
        horizontalPosition += Int32.Parse(parts[1]);
    }
    else if (parts[0] == "down")
    {
        verticalPosition += Int32.Parse(parts[1]);
    }
    else if (parts[0] == "up")
    {
        verticalPosition -= Int32.Parse(parts[1]);
    }
}

Console.WriteLine($"Final position: {horizontalPosition},{verticalPosition}");

Console.WriteLine($"Multiplied together, that is: {horizontalPosition * verticalPosition}");


// Part 2
int newHorizontalPosition = 0;
int newVerticalPosition = 0;
int aim = 0;

foreach (string line in lines)
{
    string[] parts = line.Split(' ');

    if (parts[0] == "forward")
    {
        newHorizontalPosition += Int32.Parse(parts[1]);
        newVerticalPosition += (aim * Int32.Parse(parts[1]));
    }
    else if (parts[0] == "down")
    {
        aim += Int32.Parse(parts[1]);
    }
    else if (parts[0] == "up")
    {
        aim -= Int32.Parse(parts[1]);
    }
}

Console.WriteLine($"Final position: {newHorizontalPosition},{newVerticalPosition}");

Console.WriteLine($"Multiplied together, that is: {newHorizontalPosition * newVerticalPosition}");