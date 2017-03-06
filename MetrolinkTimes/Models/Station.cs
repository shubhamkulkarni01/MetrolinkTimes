using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MetrolinkTimes.Models
{
    public class Station : IEqualityComparer<Station>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int Line { get; set; }
        
        public static int ORANGE_COUNTY = 1;
        public static int ANTELOPE_VALLEY = 2;
        public static int INLAND_EMPIRE = 3;
        public static int RIVERSIDE = 4;
        public static int SAN_BERNADINO = 5;
        public static int VENTURA_COUNTY = 6;
        public static int PERRIS_VALLEY = 7;

        public bool Equals(Station x, Station y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(Station obj)
        {
            throw new NotImplementedException();
        }
    }
}