using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class CustomerDoesntExistException : InvalidOperationException
    {
        public CustomerDoesntExistException()
        {

        }
        public CustomerDoesntExistException(string message) : base(message)
        {

        }
        public CustomerDoesntExistException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
