using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class ExistingEmailException : InvalidOperationException
    {
        public ExistingEmailException()
        {

        }
        public ExistingEmailException(string message) : base(message)
        {

        }
        public ExistingEmailException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
