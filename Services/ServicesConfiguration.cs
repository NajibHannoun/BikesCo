using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Services
{
    public class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IUserService<User>),
                typeof(UserService));
            services.AddScoped(typeof(IUserService<Admin>),
                typeof(AdminService));
            services.AddScoped(typeof(IBicycleService<Bicycle>),
                typeof(BicycleService));
            services.AddScoped(typeof(IUserService<Customer>),
                typeof(CustomerService));
            services.AddScoped(typeof(ITransactionService<Transaction>),
                typeof(TransactionService));
            services.AddScoped(typeof(LoginServices),
                typeof(LoginServices));
            
        }
    }
}
