using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class BikeMissmatchCustomerException : InvalidOperationException
    {
        public BikeMissmatchCustomerException()
        {

        }
        public BikeMissmatchCustomerException(string message) : base(message)
        {

        }
        public BikeMissmatchCustomerException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
