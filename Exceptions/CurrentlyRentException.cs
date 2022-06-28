using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{

    public class CurrentlyRentException : InvalidOperationException
    {
        public CurrentlyRentException()
        {

        }
        public CurrentlyRentException(string message) : base(message)
        {

        }
        public CurrentlyRentException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
