using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class AdminDoesntExistException : InvalidOperationException
    {
        public AdminDoesntExistException()
        {

        }
        public AdminDoesntExistException(string message) : base(message)
        {

        }
        public AdminDoesntExistException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
