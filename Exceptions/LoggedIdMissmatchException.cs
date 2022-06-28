using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class LoggedIdMissmatchException : InvalidOperationException
    {
        public LoggedIdMissmatchException()
        {

        }
        public LoggedIdMissmatchException(string message) : base(message)
        {

        }
        public LoggedIdMissmatchException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
