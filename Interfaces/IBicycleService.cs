using BikesTest.Models;
using BikesTest.Models.GlueModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Interfaces
{
    public interface IBicycleService<T> : IEntityService<T>
    {
        //List<GlueBicycle> GlueBicycleList(List<T> bicycles);
        //GlueBicycle GetGlueBicycleById(int id);
        //Customer GetCustomerById(int? id);
    }
}
