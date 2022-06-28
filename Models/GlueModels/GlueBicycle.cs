using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BikesTest.Models.Bicycle;

namespace BikesTest.Models.GlueModels
{
    public class GlueBicycle
    {
        [Display(Name = "Last Customer")]
        public Customer lastCustomer { get; set; }

        public Bicycle bicycle { get; set; }
        public GlueBicycle(Bicycle bicycle, Customer lastCustomer)
        {
            this.bicycle = bicycle;
            this.lastCustomer = lastCustomer;
        }
    }
}
