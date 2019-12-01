using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_04
{
    class Program
    {
        static void Main(string[] args)
        {
            // Find the guard that has the most minutes asleep. 
            // What minute does that guard spend asleep the most?



            // Make an empty list of guards
            List<Guard> guards = new List<Guard>();

            // Take in an unordered list of times and dates when guards start shifts, fall asleep and wake up 
            var lines = File.ReadAllLines("input.txt");

            // Sort that into an ordered list, by date then time
            List<Action> actions = new List<Action>();
            foreach (var line in lines)
            {
                Action action = new Action();

                Regex re1 = new Regex(@"((\d\d\d\d-\d\d-\d\d))");
                var dateString = re1.Matches(line)[0].Value;
                action.Date = dateString.Substring(0, dateString.Length);

                Regex re2 = new Regex(@"((\d\d:\d\d))");
                var timeString = re2.Matches(line)[0].Value;
                action.Time = timeString.Substring(0, timeString.Length);

                Regex re3 = new Regex(@"(\] (.*))");
                Match match3 = re3.Match(line);
                action.Description = match3.Groups[2].Value;

                actions.Add(action);
            }
            actions = actions.OrderBy(a => a.Date).ThenBy(a => a.Time).ToList();

            // Use the ordered list to create a list of days with the guard id and a breakdown of minutes slept and awake
            List<Day> days = new List<Day>(); 
            foreach (var action in actions)
            {
                Day day = new Day();
                if(days.Any(d => d.Date == action.Date))
                {
                    day = days.First
                }

                day.Date = action.Date;

                Regex re1 = new Regex(@"(#(\d+))");
                Match match1 = re1.Match(action.Description);
                if (match1.Success)
                {
                    day.GuardId = Int32.Parse(match1.Groups[2].Value);
                }
                else
                {
                    // Use the ID from a previous action on the same day
                    day.GuardId = days.First(d => d.Date == day.Date).GuardId;
                }
                


                // Date
                // GuardId 
                // IsAsleepThisMinute 
            }

            // Use the list of days to populate the list of guards 

        }
    }
}
