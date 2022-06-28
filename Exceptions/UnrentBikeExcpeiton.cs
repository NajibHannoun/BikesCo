using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Exceptions
{
    public class UnrentBikeExcpeiton : InvalidOperationException
    {
        public UnrentBikeExcpeiton()
        {

        }
        public UnrentBikeExcpeiton(string message) : base(message)
        {

        }
        public UnrentBikeExcpeiton(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
