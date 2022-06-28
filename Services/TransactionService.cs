using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Exceptions;
using BikesTest.Models.GlueModels;
using BikesTest.ServiceExtentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BikesTest.Services
{
    public class TransactionService : ITransactionService<Transaction>
    {
        private readonly Context _db;
        private readonly IUserService<Admin> _aService;
        private readonly IUserService<Customer> _cService;
        private readonly IBicycleService<Bicycle> _bService;
        public TransactionService(Context db,
                                  IUserService<Admin> aService,
                                  IUserService<Customer> cService,
                                  IBicycleService<Bicycle> bService)
        {
            _db = db;
            _aService = aService;
            _cService = cService;
            _bService = bService;
        }

        public Transaction Create(Transaction row)
        {
            Customer customer = _cService.GetById(row.customer_Id);
            if (customer == null)
                throw new CustomerDoesntExistException("Customer doesn't exist in data base");
            Bicycle bicycle = _bService.GetById(row.bicycle_Id);
            if (bicycle == null)
                throw new BikeDoesntExistExeption("Bicycle doesn't exist in data base");
            Admin admin = _aService.GetByUserId(row.admin_Id);
            if (admin == null)
                throw new AdminDoesntExistException("Admin doesn't exist in data base");


            this.SetTransactionNotDeleted(row);

            if (row.returnDate == null)
            {
                if (customer.isCurrentlyBiking)
                    throw new CurrentlyBikingException("This customer is currently biking");
                else if (bicycle.isCurrentlyRented)
                    throw new CurrentlyRentException("This bicycle is currenty rented");
                else if (admin.isSuspended)
                    throw new SuspendedAdminException("This admin is suspended and cannot perform this operation");


                row.customer_Id = customer.id;
                row.admin_Id = admin.id;
                row.bicycle_Id = bicycle.id;

                
                _cService.SetIsCurrentlyBikingTrue(customer);
                _bService.SetIsCurrentlyRentedTrue(bicycle);
                //_bService.SetLastTransactionDate(bicycle);
                _bService.IncrementTimesRented(bicycle);
                //_bService.SetLastCustomerId(bicycle, row);

                row.admin = null;
                row.bicycle = null;
                row.customer = null;

            }
            else
            {
                Transaction lastTransaction = bicycle.transactions.LastOrDefault();
                if (!customer.isCurrentlyBiking)
                    throw new CustomerDidntRentException("This customer didn't rent any bicycle");
                else if (!bicycle.isCurrentlyRented)
                    throw new UnrentBikeExcpeiton("This bicycle has not been rented recently");
                else if (admin.isSuspended)
                    throw new SuspendedAdminException("This admin is suspended and cannot perform this operation");
                else if (lastTransaction != null)
                {
                    if (lastTransaction.customer_Id != customer.id)
                        throw new BikeMissmatchCustomerException("This Customer did not rent this bicycle");
                }

                lastTransaction.returnDate = row.returnDate;

                _cService.SetIsCurrentlyBikingFalse(customer);
                _bService.SetIsCurrentlyRentedFalse(bicycle);
                this.SetTransactionDuration(lastTransaction);
                this.CalculateTransactionCost(lastTransaction, bicycle);
                _bService.IncreaseEarningsToDate(bicycle, lastTransaction);
                _cService.IncreaseTimeBiked(customer, (decimal)lastTransaction.durationOfTransaction);
                _cService.IncrementBikesRented(customer);

                lastTransaction.bicycle = null;
                lastTransaction.customer = null;
                lastTransaction.admin = null;

                _db.Transactions.Update(lastTransaction);
                _db.Customers.Update(customer);
                _db.Bicycles.Update(bicycle);
                _db.Admins.Update(admin);
                _db.SaveChanges();

                return lastTransaction;
            }

            _db.Transactions.Add(row);
            _db.Customers.Update(customer);
            _db.Bicycles.Update(bicycle);
            _db.Admins.Update(admin);
            _db.SaveChanges();

            return row;
        }

        public Transaction Update(Transaction row)
        {
            row = Create(row);
            return row;
        }

        public void Delete(int id)
        {
            _db.Remove(GetById(id));
            _db.SaveChanges();
        }

        public void Delete(Transaction row)
        {
            row = _db.Transactions.Where(o => o.id == row.id)
                                  .Include(o => o.customer).ThenInclude(m => m.user)
                                  .Include(m => m.admin).ThenInclude(m => m.user)
                                  .Include(m => m.bicycle).AsNoTracking()
                                  .SingleOrDefault();

            //_db.DeletedTransactions.Add(row);

            this.SetTransactionDeleted(row);
            row = NegateTransaction(row);

            _db.Update(row);
            _db.SaveChanges();
        }

        public List<Transaction> GetAll()
        {
            return _db.Transactions.Where(o => o.isDeleted == false)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .AsNoTracking()
                                   .ToList<Transaction>();
        }

        public List<Transaction> GetAllDeleted()
        {
            return _db.Transactions.Where(o => o.isDeleted == true)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .AsNoTracking()
                                   .ToList<Transaction>();
        }

        public Transaction GetById(int? id)
        {
            return _db.Transactions.AsNoTracking()
                                   .Where(o => o.id == id)
                                   .Where(o => o.isDeleted == false)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .SingleOrDefault();
        }

        public Transaction GetByDeletedId(int? id)
        {
            return _db.Transactions.AsNoTracking()
                                   .Where(o => o.id == id)
                                   .Where(o => o.isDeleted == true)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .SingleOrDefault();
        }

        public ICollection<Transaction> GetByBicycleId(int? id)
        {
            return _db.Transactions.AsNoTracking()
                                   .Where(o => o.bicycle_Id == id)
                                   .Where(o => o.isDeleted == false)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .ToList<Transaction>();
        }

        public Transaction GetByUsername(string username)
        {
            return _db.Transactions.AsNoTracking()
                      .Where(o => o.customer.id == _cService.GetByUsername(username).id && o.returnDate == null)
                      .Where(o => o.isDeleted == false)
                      .Include(o => o.customer).ThenInclude(m => m.user)
                      .Include(m => m.admin).ThenInclude(m => m.user)
                      .Include(m => m.bicycle)
                      .SingleOrDefault();
        }

        public List<Transaction> Search()
        {
            throw new NotImplementedException();
        }

        public Transaction NegateTransaction(Transaction row)
        {
            row.costOfTransaction = -row.costOfTransaction;
            row.durationOfTransaction = -row.durationOfTransaction;

            Customer customer = row.customer;
            if (customer == null)
                throw new CustomerDoesntExistException("Customer doesn't exist in data base");
            Bicycle bicycle = row.bicycle;
            if (bicycle == null)
                throw new BikeDoesntExistExeption("Bicycle doesn't exist in data base");
            Admin admin = row.admin;
            if (admin == null)
                throw new AdminDoesntExistException("Admin doesn't exist in data base");

            if(row.returnDate != null)
            {
                _bService.IncreaseEarningsToDate(bicycle, row);
                _cService.IncreaseTimeBiked(customer, (decimal)row.durationOfTransaction);
                _cService.DecrementBikesRented(customer);
            }
            else
            {
                _bService.SetIsCurrentlyRentedFalse(bicycle);
                _bService.DecrementTimesRented(bicycle);
                _cService.SetIsCurrentlyBikingFalse(customer);
            }

            _db.Customers.Update(customer);
            _db.Bicycles.Update(bicycle);

            return row;
        }

    }
}
