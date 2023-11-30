using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Day06
{
    public class FishContext : DbContext
    {
        public DbSet<Fish> Fishes { get; set; }

        public string DbPath { get; }

        public FishContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "fishes.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Fish
    {
        public int FishId { get; set; }
        public int InternalTimer { get; set; }
    }
}