using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class BikeDoesntExistExeption : InvalidOperationException
    {
        public BikeDoesntExistExeption()
        {

        }
        public BikeDoesntExistExeption(string message) : base(message)
        {

        }
        public BikeDoesntExistExeption(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
