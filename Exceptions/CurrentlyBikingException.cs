using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class CurrentlyBikingException : InvalidOperationException
    {
        public CurrentlyBikingException()
        {

        }
        public CurrentlyBikingException(string message) : base(message)
        {

        }
        public CurrentlyBikingException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
