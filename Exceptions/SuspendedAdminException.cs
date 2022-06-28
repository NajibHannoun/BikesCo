using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class SuspendedAdminException : InvalidOperationException
    {
        public SuspendedAdminException()
        {

        }
        public SuspendedAdminException(string message) : base(message)
        {

        }
        public SuspendedAdminException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
