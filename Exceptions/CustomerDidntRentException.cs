using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class CustomerDidntRentException : InvalidOperationException
    {
        public CustomerDidntRentException()
        {

        }
        public CustomerDidntRentException(string message) : base(message)
        {

        }
        public CustomerDidntRentException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
