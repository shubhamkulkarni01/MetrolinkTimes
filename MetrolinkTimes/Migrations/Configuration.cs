namespace MetrolinkTimes.Migrations
{
    using System;
    using MetrolinkTimes.Models;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using System.Collections;

    internal sealed class Configuration : DbMigrationsConfiguration<MetrolinkTimes.Models.MetrolinkTimesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MetrolinkTimesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //System.Diagnostics.Debugger.Launch();
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            
            
            if (System.Diagnostics.Debugger.IsAttached == false)
            {

                System.Diagnostics.Debugger.Launch();
            }

            Dictionary<int, string> dictionary = new Dictionary<int, string> {
                { 14, Properties.Resources.orange_county_weekday }, { 7, Properties.Resources.orange_county_weekend },
                { 2, Properties.Resources.inland_empire_weekday }, {3,Properties.Resources.inland_empire_weekend },
                { 4, Properties.Resources.antelope_valley_weekday }, 
                { 5, Properties.Resources.antelope_valley_weekend},
                { 6, Properties.Resources. riverside_weekday},
                { 8, Properties.Resources.san_bernadino_weekday },
                { 13, Properties.Resources.san_bernadino_weekend },
                { 10, Properties.Resources.ventura_county_weekday },
                { 12, Properties.Resources.perris_valley_weekday }, { 9, Properties.Resources.perris_valley_weekend }
            };
            int i = 0;
            foreach (KeyValuePair<int,string> asdf in dictionary)
            try
            {
                    string x = asdf.Value;
                    var stuff = JArray.Parse(x);

                /*StationTrain[] a = stuff.Select(p =>
                new StationTrain
                {
                    station = context.Stations.FirstOrDefault(f=>f.Name.Equals((string)p["stop_name"]))??(new Station { Name = (string)p["stop_name"], latitude = (double)p["stop_lat"], longitude = (double)p["stop_lon"] }),
                    train = context.Trains.FirstOrDefault(f => f.train_id==((int)p["trip_short_name"])) ?? new Train { train_id = (int)p["trip_short_name"] },
                    time = DateTime.Parse((string)p["arrival_time"]),
                    user = context.Users.FirstOrDefault(r=>r.fcm_id==Properties.Resources.SYSTEM) ?? new User {fcm_id=Properties.Resources.SYSTEM }
                }
                ).ToArray();*/
                List<Station> l1 = new List<Station>();
                List<Train> l2 = new List<Train>();
                    string s;
                foreach(var z in stuff)
                    { 
                    Station s1 = new Station() { Name = (string)z["stop_name"], latitude = (double)z["stop_lat"], longitude = (double)z["stop_lon"], Line = asdf.Key==14||asdf.Key==7?Station.ORANGE_COUNTY:
                        asdf.Key==2||asdf.Key==3?Station.INLAND_EMPIRE:
                        asdf.Key == 4 || asdf.Key == 5 ? Station.ANTELOPE_VALLEY:
                        asdf.Key == 6 ? Station.RIVERSIDE:
                        (asdf.Key == 8 || asdf.Key == 13) ? Station.SAN_BERNADINO:
                        asdf.Key == 10 ? Station.VENTURA_COUNTY:Station.PERRIS_VALLEY};
                    Train t1 = new Train() { train_id = (s=(string)z["trip_short_name"]).Contains("A")?int.Parse(s.Substring(1,s.Length-2)):s.Contains("B")?-1:(int)z["trip_short_name"] };
                        if (t1.train_id == -1) continue;
                        if (!l1.Contains(s1, new Station())) l1.Add(s1);
                    if (!l2.Contains(t1, new Train())) l2.Add(t1);
                }
                context.Stations.AddOrUpdate(p=>new { p.Name, p.Line},l1.ToArray());
                context.Trains.AddOrUpdate(p=>p.train_id,l2.ToArray());

                User u = new User { fcm_id = Properties.Resources.SYSTEM };
                context.Users.AddOrUpdate(j=>j.fcm_id,u);

                context.SaveChanges();
                List<StationTrain> list = new List<StationTrain>();
                foreach(var p in stuff)
                {
                    string d = (string)p["stop_name"];
                        int n;
                        try
                        {
                            n = (int)p["trip_short_name"];
                        }
                        catch (FormatException e) {
                            Console.WriteLine(e);
                            n = -1;
                            continue;
                        }
                
                        i++;
                        if (i == 9)
                        {
                            Console.WriteLine(p);
                        }
                        Console.WriteLine(i);
                        StationTrain s1 = new StationTrain()
                    {
                        station = l1.FirstOrDefault(f => f.Name.Equals(d)),
                        train = l2.FirstOrDefault(f => f.train_id == n),
                        time = DateTime.Parse((string)p["arrival_time"]),
                        user = context.Users.FirstOrDefault(r => r.fcm_id == Properties.Resources.SYSTEM),
                        Day = asdf.Key%2==0?StationTrain.WEEKDAY: StationTrain.WEEKEND
                    };
                    list.Add(s1);
                }
                context.StationTrains.AddOrUpdate(list.ToArray());
            }
            catch (Exception e) { Console.WriteLine(e); Console.WriteLine(i); }
        }
    }
}
