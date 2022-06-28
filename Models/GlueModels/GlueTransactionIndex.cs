using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BikesTest.Models.Transaction;

namespace BikesTest.Models.GlueModels
{
    public class GlueTransactionIndex
    {
        public Transaction transaction { get; set; }
        [Display(Name = "Customer")]
        public Customer customer { get; set; }
        [Display(Name = "Bicycle")]
        public Bicycle bicycle { get; set; }

        public GlueTransactionIndex(Transaction transaction, Customer customer, Bicycle bicycle)
        {
            this.transaction = transaction;
            this.customer = customer;
            this.bicycle = bicycle;
        }
    }
}
