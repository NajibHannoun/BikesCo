using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class InvalidPasswordException : InvalidOperationException
    {
        public InvalidPasswordException()
        {

        }
        public InvalidPasswordException(string message) : base(message)
        {

        }
        public InvalidPasswordException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
