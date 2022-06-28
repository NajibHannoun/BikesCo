using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Models
{
    public class Bicycle
    {
        public enum BicycleSizes
        {
            S, M, L, XL
        }

        [Display(Name = "Id")]
        public int id { get; set; }

        [Display(Name = "Size")]
        public BicycleSizes size { get; set; }

        [Display(Name = "Lease Price")]
        public double leassPrice { get; set; }

        [Display(Name = "Currently Rented")]
        public bool isCurrentlyRented { get; set; } = false;

        [DataType(DataType.Date)]
        [Display(Name = "Last CheckUp")]
        public DateTime? lastCheckupDate { get; set; }
        
        [Display(Name = "Times Rented")]
        public int timesRented { get; set; } = 0;

        [Display(Name = "Total Earnings")]
        public double earningsToDate { get; set; } = 0;

        [DataType(DataType.Date)]
        [Display(Name = "Aquisition Date")]
        public DateTime aquisutionDate { get; set; } = DateTime.Today;

        [Display(Name = "Purchase Price")]
        public double purchasePrice { get; set; }

        public List<Transaction> transactions { get; set; }

    }
}
