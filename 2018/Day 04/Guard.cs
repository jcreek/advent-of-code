using System;
using System.Collections.Generic;

namespace Day_04
{
    public class Guard
    {
        public int GuardId { get; set; }   
        public int TotalMinutesAsleep { get; set; }
        public List<int> TimesSleptPerMinute { get; set; }
    }
}