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
                _bService.IncrementTimesRented(bicycle);
                _bService.IncreaseEarningsToDate(bicycle, (double)lastTransaction.costOfTransaction);
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
            Transaction dbTransaction = this.GetById(row.id);

            if (row.returnDate != null && dbTransaction.returnDate == null)
                throw new CurrentlyRentException("This transaction is not over, go through the return bike process if you want to complete this transaction");
            else if (row.returnDate != null)
            {
                if(row.returnDate != dbTransaction.returnDate || row.rentalDate != dbTransaction.rentalDate)
                {
                    dbTransaction.rentalDate = row.rentalDate;
                    dbTransaction.returnDate = row.returnDate;
                }

                if (row.bicycle_Id != dbTransaction.bicycle_Id)
                {
                    Bicycle oldBicycle = dbTransaction.bicycle;
                    _bService.IncreaseEarningsToDate(oldBicycle, -(double)dbTransaction.costOfTransaction); //old bike - old transaction cost
                    _bService.DecrementTimesRented(oldBicycle);

                    _db.Bicycles.Update(oldBicycle);

                    Bicycle newBicycle = _bService.GetById(row.bicycle_Id);
                    if (newBicycle == null)
                        throw new BikeDoesntExistExeption("Bicycle doesn't exist in data base");
                    else if (newBicycle.isCurrentlyRented)
                        throw new CurrentlyRentException("Bicycle is currently rented");

                    this.SetTransactionDuration(row);
                    this.CalculateTransactionCost(row, newBicycle);

                    _bService.IncreaseEarningsToDate(newBicycle, (double)dbTransaction.costOfTransaction);
                    _bService.IncrementTimesRented(newBicycle);

                    _db.Bicycles.Update(newBicycle); //maybe tracking problem

                    if(dbTransaction.customer_Id != row.customer_Id)
                    {
                        Customer oldCustomer = dbTransaction.customer;
                        _cService.IncreaseTimeBiked(oldCustomer, -(decimal)dbTransaction.durationOfTransaction);
                        _cService.DecrementBikesRented(oldCustomer);
                        _db.Customers.Update(oldCustomer);

                        Customer newCustomer = _cService.GetById(row.customer_Id);
                        if (newCustomer == null)
                            throw new CustomerDoesntExistException("Customer doesn't exist in data base");
                        else if (newCustomer.isCurrentlyBiking && newCustomer.id != dbTransaction.customer_Id)
                            throw new CurrentlyBikingException("Customer is currently biking");

                        _cService.IncreaseTimeBiked(newCustomer, (decimal)row.durationOfTransaction);
                        _cService.IncrementBikesRented(newCustomer);
                        _db.Customers.Update(newCustomer);
                    }
                    else
                    {
                        Customer oldCustomer = dbTransaction.customer;
                        _cService.IncreaseTimeBiked(oldCustomer, -(decimal)dbTransaction.durationOfTransaction);
                        _cService.IncreaseTimeBiked(oldCustomer, (decimal)row.durationOfTransaction);
                        _db.Customers.Update(oldCustomer);
                    }
                    this.SetTransactionDuration(dbTransaction);
                    this.CalculateTransactionCost(dbTransaction, newBicycle);
                }
                else
                {
                    Bicycle oldBicycle = dbTransaction.bicycle;
                    _bService.IncreaseEarningsToDate(oldBicycle, -(double)dbTransaction.costOfTransaction);

                    this.SetTransactionDuration(row);
                    this.CalculateTransactionCost(row, oldBicycle);

                    if (dbTransaction.customer_Id != row.customer_Id)
                    {
                        Customer oldCustomer = dbTransaction.customer;
                        _cService.IncreaseTimeBiked(oldCustomer, -(decimal)dbTransaction.durationOfTransaction);
                        _cService.DecrementBikesRented(oldCustomer);
                        _db.Customers.Update(oldCustomer);

                        Customer newCustomer = _cService.GetById(row.customer_Id);
                        if (newCustomer == null)
                            throw new CustomerDoesntExistException("Customer doesn't exist in data base");
                        else if (newCustomer.isCurrentlyBiking && newCustomer.id != dbTransaction.customer_Id)
                            throw new CurrentlyBikingException("Customer is currently biking");

                        _cService.IncreaseTimeBiked(newCustomer, (decimal)row.durationOfTransaction);
                        _cService.IncrementBikesRented(newCustomer);
                        _db.Customers.Update(newCustomer);
                    }
                    else
                    {
                        Customer oldCustomer = dbTransaction.customer;
                        _cService.IncreaseTimeBiked(oldCustomer, -(decimal)dbTransaction.durationOfTransaction);
                        _cService.IncreaseTimeBiked(oldCustomer, (decimal)row.durationOfTransaction);
                        _db.Customers.Update(oldCustomer);
                    }

                    this.SetTransactionDuration(dbTransaction);
                    this.CalculateTransactionCost(dbTransaction, oldBicycle);

                    _bService.IncreaseEarningsToDate(oldBicycle, (double)row.costOfTransaction);

                    _db.Bicycles.Update(oldBicycle);
                }
            }
            else if (row.returnDate == null)
            {
                if(row.rentalDate != dbTransaction.rentalDate)
                {
                    dbTransaction.rentalDate = row.rentalDate;
                    if (dbTransaction.customer_Id != row.customer_Id)
                    {
                        Customer oldCustomer = _cService.GetById(dbTransaction.customer_Id);
                        _cService.SetIsCurrentlyBikingFalse(oldCustomer);
                        _db.Customers.Update(oldCustomer);
                        Customer newCustomer = _cService.GetById(row.customer_Id);
                        _cService.SetIsCurrentlyBikingTrue(newCustomer);
                        _db.Customers.Update(newCustomer);
                    }

                    if (dbTransaction.bicycle_Id != row.bicycle_Id)
                    {
                        Bicycle oldBicycle = _bService.GetById(dbTransaction.bicycle_Id);
                        _bService.SetIsCurrentlyRentedFalse(oldBicycle);
                        _db.Bicycles.Update(oldBicycle);
                        Bicycle newBicycle = _bService.GetById(row.bicycle_Id);
                        _bService.SetIsCurrentlyRentedTrue(newBicycle);
                        _db.Bicycles.Update(newBicycle);
                    }
                } 
            }

            Admin admin = _aService.GetByUserId(row.admin_Id);

            dbTransaction.customer_Id = row.customer_Id;
            dbTransaction.bicycle_Id = row.bicycle_Id;
            dbTransaction.admin_Id = admin.id;
            
            _db.Transactions.Update(dbTransaction);  
            _db.SaveChanges();

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
                                   .Where(o => o.isDeleted == false)
                                   .Where(o => o.id == id)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .SingleOrDefault();
        }

        public Transaction GetByDeletedId(int? id)
        {
            return _db.Transactions.AsNoTracking()
                                   .Where(o => o.isDeleted == true)
                                   .Where(o => o.id == id)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .SingleOrDefault();
        }

        public ICollection<Transaction> GetByBicycleId(int? id)
        {
            return _db.Transactions.AsNoTracking()
                                   .Where(o => o.isDeleted == false)
                                   .Where(o => o.bicycle_Id == id)
                                   .Include(o => o.customer).ThenInclude(m => m.user)
                                   .Include(m => m.admin).ThenInclude(m => m.user)
                                   .Include(m => m.bicycle)
                                   .ToList<Transaction>();
        }

        public Transaction GetByUsername(string username)
        {
            return _db.Transactions.AsNoTracking()
                      .Where(o => o.isDeleted == false)
                      .Where(o => o.customer.id == _cService.GetByUsername(username).id && o.returnDate == null)
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
                _bService.IncreaseEarningsToDate(bicycle, (double)row.costOfTransaction);
                _cService.IncreaseTimeBiked(customer, (decimal)row.durationOfTransaction);
                _bService.DecrementTimesRented(bicycle);
                _cService.DecrementBikesRented(customer);
            }
            else
            {
                _bService.SetIsCurrentlyRentedFalse(bicycle);
                _cService.SetIsCurrentlyBikingFalse(customer);
            }

            _db.Customers.Update(customer);
            _db.Bicycles.Update(bicycle);

            return row;
        }

    }
}
