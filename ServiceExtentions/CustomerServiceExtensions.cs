using BikesTest.Controllers;
using BikesTest.Exceptions;
using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Service;
using BikesTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.ServiceExtentions
{
    public static class CustomerServiceExtensions
    {
        
        public static void SetIsCurrentlyBikingTrue(this IUserService<Customer> _cService, Customer customer)
        {
            customer.isCurrentlyBiking = true;
        }
        public static void SetIsCurrentlyBikingFalse(this IUserService<Customer> _cService, Customer customer)
        {
            customer.isCurrentlyBiking = false;
        }
        public static void IncrementBikesRented(this IUserService<Customer> _cService, Customer customer)
        {
            customer.numberOfBikesRented++;
        }
        public static void DecrementBikesRented(this IUserService<Customer> _cService, Customer customer)
        {
            customer.numberOfBikesRented--;
        }
        public static void IncreaseTimeBiked(this IUserService<Customer> _cService, Customer customer, decimal numberOfHours)
        {
            customer.timeBiked += numberOfHours;
        }

        public static Customer ChangePassword(this IUserService<Customer> cService,
                                          Customer row, string password)
        {
            row.user.password = LoginServices.HashPassword(password, row.user.birthday.ToString());

            cService.Update(row);
            return row;
        }
        
        public static List<List<string>> GetUsernamesAndIds(this IUserService<Customer> cService)
        {
            List<Customer> customersList = cService.GetAll();
            List<List<string>> usernames_ids = new List<List<string>>();
            for (int i = 0; i < customersList.Count; i++)
            {
                usernames_ids.Add(new List<string>());
                usernames_ids[i].Add(customersList.ElementAt(i).user.username);
                usernames_ids[i].Add(customersList.ElementAt(i).id.ToString());
            }
            return usernames_ids;
        }
        public static void IsIdAndConnectedCustomerMatch(this CustomerController controller, int id)
        {
            if (Int32.Parse(controller.User.Identities.FirstOrDefault().FindFirst("Id").Value) != id)
            {
                throw new LoggedIdMissmatchException("This Id doesn't match the logged in Id");
            }
        }
    }
}
