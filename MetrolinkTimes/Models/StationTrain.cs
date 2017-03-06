using System;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetrolinkTimes.Models
{
    public class StationTrain
    {
        public int Id { get; set; }
        [Required]
        public virtual Station station { get; set; }
        public virtual Train train { get; set; }
        public virtual DateTime time { get; set; }
        public virtual User user { get; set; }
        public int Day { get; set; }

        public static int WEEKDAY = 0;
        public static int WEEKEND = 1;
    }
}