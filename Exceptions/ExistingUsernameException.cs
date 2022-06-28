using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class ExistingUsernameException : InvalidOperationException
    {
        public ExistingUsernameException()
        {

        }
        public ExistingUsernameException(string message) : base(message)
        {
            
        }
        public ExistingUsernameException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
