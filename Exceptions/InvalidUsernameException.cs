using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class InvalidUsernameException : InvalidOperationException
    {
        public InvalidUsernameException()
        {

        }
        public InvalidUsernameException(string message) : base(message)
        {

        }
        public InvalidUsernameException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
