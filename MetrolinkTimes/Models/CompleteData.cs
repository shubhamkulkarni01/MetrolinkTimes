using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrolinkTimes.Models
{
    public class CompleteData
    {
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int line { get;set;}
        public List<string> time { get; set; }
        public List<int> train_id { get; set; }
        public List<int> day { get; set; }

        public CompleteData()
        {

        }
        public CompleteData(List<TempData> tempdata)
        {
            name = tempdata.ElementAt(0).Name;
            latitude = tempdata.ElementAt(0).latitude;
            longitude = tempdata.ElementAt(0).longitude;
            line = tempdata.ElementAt(0).line;
            time = new List<string>();
            train_id = new List<int>();
            day = new List<int>();
            foreach (TempData item in tempdata)
            {
                train_id.Add(item.train_id);
                time.Add(item.time.ToString("HH:mm:ss"));
                day.Add(item.day);
            }
        }
    }
    public class TempData
    {
        public string Name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public DateTime time { get; set; }
        public int train_id { get; set; }
        public int line { get; set; }
        public int day { get; set; }

        public TempData(StationTrain stationtrain)
        {
            Name = stationtrain.station.Name;
            longitude = stationtrain.station.longitude;
            latitude = stationtrain.station.latitude;
            time = stationtrain.time;
            train_id = stationtrain.train.train_id;
            line = stationtrain.station.Line;
            day = stationtrain.Day;
        }
    }
}