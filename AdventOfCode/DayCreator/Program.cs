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
var newDayCreatorBase = currentLocation.Substring(0, ix + "DayCreator".Length);

string yearPath = Path.GetFullPath(Path.Combine(newPathBase, "AOC.Tests", $"Y{year}"));
string dataPath = Path.GetFullPath(Path.Combine(yearPath, "Data", $"Day{day}.dat"));
string tasksPath = Path.GetFullPath(Path.Combine(yearPath, "Tasks", $"Day{day}.txt"));

// Create data file
using (FileStream fs = File.Create(dataPath));

// Create task file
using (FileStream fs = File.Create(tasksPath));

// Create code file
var dayXPath = Path.GetFullPath(Path.Combine(newDayCreatorBase, "DayX.cs"));
string text = File.ReadAllText(dayXPath);
text = text.Replace("YX", $"Y{year}");
text = text.Replace("DayX", $"Day{day}");
var writePath = Path.GetFullPath(Path.Combine(yearPath, $"Day{day}.cs"));
File.WriteAllText(writePath, text);
