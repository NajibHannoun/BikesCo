using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BikesTest.Models.Transaction;

namespace BikesTest.Models.GlueModels
{
    public class GlueTransactionDetails
    {
        public Transaction transaction { get; set; }
        [Display(Name = "Customer")]
        public Customer customer { get; set; }
        [Display(Name = "Admin")]
        public Admin admin { get; set; }
        [Display(Name = "Bicycle")]
        public Bicycle bicycle { get; set; }

        public GlueTransactionDetails(Transaction transaction, Customer customer, Admin admin, Bicycle bicycle)
        {
            this.transaction = transaction;
            this.customer = customer;
            this.admin = admin;
            this.bicycle = bicycle;
        }
    }
}
