using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Models
{
    public class Transaction
    {
        public enum TransactionType
        {
            Long,
            Short,
        }

        [Display(Name = "Id")]
        public int id { get; set; }

        [Display(Name = "Bicycle Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Bicycle Id")]
        public int bicycle_Id { get; set; }
        public Bicycle bicycle { get; set; }


        [Display(Name = "Customer Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Customer Id")]
        public int customer_Id { get; set; }
        public Customer customer { get; set; }


        [Display(Name = "Admin Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Admin Id")]
        public int admin_Id { get; set; }
        public Admin admin { get; set; }


        [Display(Name = "Rental Date")]
        public DateTime rentalDate { get; set; } = DateTime.Today;

        [Display(Name = "Return Date")]
        public DateTime? returnDate { get; set; }

        [Display(Name = "Duration Of Transaction")]
        [Range(0, int.MaxValue)]
        public decimal? durationOfTransaction { get; set; }

        [Display(Name = "Transaction Type")]
        public TransactionType transactionType { get; set; }

        [Display(Name = "Price")]
        public double? costOfTransaction { get; set; }

        public bool isDeleted { get; set; }
    }
}
