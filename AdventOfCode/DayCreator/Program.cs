using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

Console.Write("Year: ");
var year = Console.ReadLine();

Console.Write("Day: ");
var day = Console.ReadLine().PadLeft(2, '0'); ;

var currentLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
int ix = currentLocation.IndexOf("DayCreator");
var newPathBase = currentLocation.Substring(0, ix - 1);

string yearPath = Path.GetFullPath(Path.Combine(newPathBase, $@"AOC.Tests\Y{year}\"));
string dataPath = Path.GetFullPath(Path.Combine(yearPath, $@"Data\"));
string tasksPath = Path.GetFullPath(Path.Combine(yearPath, $@"Tasks\"));

// Create data file
using (FileStream fs = File.Create($"{dataPath}Day{day}.dat"));

// Create task file
using (FileStream fs = File.Create($"{tasksPath}Day{day}.txt"));

// Create code file
string text = File.ReadAllText("DayX.cs");
text = text.Replace("YX", $"Y{year}");
text = text.Replace("DayX", $"Day{day}");
File.WriteAllText($"{yearPath}Day{day}.cs", text);
