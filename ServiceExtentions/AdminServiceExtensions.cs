using BikesTest.Exceptions;
using BikesTest.Interfaces;
using BikesTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.ServiceExtentions
{
    public static class AdminServiceExtensions
    {
        public static void CheckSuspended(this IUserService<Admin> aService, int id)
        {
            if (aService.GetByUserId(id).isSuspended)
                throw new SuspendedAdminException("This admin is currently suspended and cannot perform this operation");
        }
    }
}
