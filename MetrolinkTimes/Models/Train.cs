using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MetrolinkTimes.Models
{
    public class Train : IEqualityComparer<Train>
    {
        public int Id { get; set; }
        [Required]
        public int train_id { get; set; }

        public bool Equals(Train x, Train y)
        {
            return x.train_id.Equals(y.train_id);
        }

        public int GetHashCode(Train obj)
        {
            throw new NotImplementedException();
        }
    }
}