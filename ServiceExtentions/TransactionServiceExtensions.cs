using BikesTest.Exceptions;
using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Models.GlueModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.ServiceExtentions
{
    public static class TransactionServiceExtensions
    {
        private static int billingUnit = 4;  //4 * 15min per hour 
        public static void SetTransactionDuration(this ITransactionService<Transaction> _tService, 
                                                  Transaction transaction)
        {
            transaction.durationOfTransaction = (decimal)(transaction.returnDate - transaction.rentalDate).GetValueOrDefault().TotalHours;
        }

        public static void CalculateTransactionCost(this ITransactionService<Transaction> _tService, 
                                                    Transaction transaction, Bicycle bike)
        {
            transaction.costOfTransaction = Math.Max(1, (int)(transaction.durationOfTransaction * billingUnit)) * (double)bike.leassPrice;
        }

        public static void SetTransactionDeleted(this ITransactionService<Transaction> _tService,
                                                 Transaction transaction)
        {
            transaction.isDeleted = true;
        }
        public static void SetTransactionNotDeleted(this ITransactionService<Transaction> _tService,
                                                 Transaction transaction)
        {
            transaction.isDeleted = false;
        }
    }
}
