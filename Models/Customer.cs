using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Models
{
    public class Customer
    {
        public int id { get; set; }
        [Display(Name = "Currently Biking")]
        public bool isCurrentlyBiking { get; set; }

        [Display(Name = "Total Biking Time")]
        [Range(0, int.MaxValue)]
        public decimal timeBiked { get; set; } //in hours

        [Display(Name = "Total Bikes Rented")]
        public int numberOfBikesRented { get; set; }

        public int? user_id { get; set; }
        public User user { get; set; }

        public List<Transaction> transactions { get; set; }
    }
}
