using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MetrolinkTimes.Models;

namespace MetrolinkTimes.Models
{
    public class MetrolinkTimesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MetrolinkTimesContext() : base("name=MetrolinkTimesContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MetrolinkTimesContext>(new MigrateDatabaseToLatestVersion<MetrolinkTimesContext, Migrations.Configuration>());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<StationTrain> StationTrains { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
