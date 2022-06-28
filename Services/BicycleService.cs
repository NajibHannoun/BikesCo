using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Models.GlueModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Services
{
    public class BicycleService : IBicycleService<Bicycle>
    {
        private readonly Context _db;

        public BicycleService(Context db)
        {
            _db = db;
        }

        public List<Bicycle> GetAll()
        {
            return _db.Bicycles.AsNoTracking().ToList<Bicycle>();
        }

        public Bicycle GetById(int? id)
        {
            return _db.Bicycles.Where(o => o.id == id)
                               .Include(o => o.transactions)
                               .AsNoTracking()
                               .SingleOrDefault();
        }

        public Bicycle Create(Bicycle row)
        {
            _db.Bicycles.Add(row);
            _db.SaveChanges();
            return row;
        }

        public void Delete(int id)
        {
            _db.Remove(GetById(id));
            _db.SaveChanges();
        }

        public void Delete(Bicycle row)
        {
            _db.Bicycles.Remove(row);
            _db.SaveChanges();
        }

        public List<Bicycle> Search()
        {
            throw new NotImplementedException();
        }

        public Bicycle Update(Bicycle row)
        {
            Bicycle oldBicycle = GetById(row.id);

            row.isCurrentlyRented = oldBicycle.isCurrentlyRented;
            //row.lastCustomer = oldBicycle.lastCustomer;
            //row.lastTransactionTime = oldBicycle.lastTransactionTime;
            row.timesRented = oldBicycle.timesRented;
            row.earningsToDate = oldBicycle.earningsToDate;

            _db.Bicycles.Update(row);
            _db.SaveChanges();

            return row;
        }

        public Bicycle GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        //public List<GlueBicycle> GlueBicycleList(List<Bicycle> bicycles)
        //{
        //    List<GlueBicycle> listGlueBicycle = new List<GlueBicycle>();
        //    for (int i = 0; i < bicycles.Count; i++)
        //    {
        //        Bicycle bicycle = bicycles.ElementAt(i);
        //        Customer customer = GetCustomerById(bicycle.lastCustomerId);
        //        listGlueBicycle.Add(new GlueBicycle(bicycle, customer));
        //    }
        //    return listGlueBicycle;
        //}

        //public GlueBicycle GetGlueBicycleById(int id)
        //{
        //    Bicycle bike = GetById(id);
        //    Customer customer = GetCustomerById(bike.lastCustomerId);
        //    return new GlueBicycle(bike, customer);
        //}

        //public Customer GetCustomerById(int? id)
        //{
        //    return _db.Customers.AsNoTracking().Where(o => o.id == id).SingleOrDefault();
        //}
    }
}
