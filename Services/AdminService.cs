using BikesTest.Exceptions;
using BikesTest.Interfaces;
using BikesTest.Models;
using BikesTest.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Services
{
    public class AdminService : IUserService<Admin>
    {
        private readonly Context _db;
        private IUserService<User> _uService;

        public AdminService(Context db,
                            IUserService<User> uService)
        {
            _db = db;
            _uService = uService;
        }

        public Admin Create(Admin row)
        {
            _uService.Create(row.user);

            row.isSuspended = false;
            row.isCurrentlyLogged = false;

            _db.Add(row);
            _db.SaveChanges();

            return row;
        }

        public void Delete(Admin row)
        {
            _db.Remove(row);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            _db.Remove(_db.Admins.Where(o => o.user.id == id).SingleOrDefault());
            _db.SaveChanges();
        }

        public List<Admin> GetAll()
        {
            return _db.Admins.AsNoTracking()
                             .Include(o => o.user)   
                             .ToList();
        }

        public Admin GetById(int? id)
        {
            return _db.Admins
                    .AsNoTracking()
                    .Include(o => o.user)
                    .Where(o => o.id == id)
                    .SingleOrDefault();
        }

        public Admin GetByUserId(int id)
        {
            return _db.Admins
                    .AsNoTracking()
                    .Include(o => o.user)
                    .Where(o => o.user.id == id)
                    .SingleOrDefault();
        }

        public Admin GetByUsername(string username)
        {
            return _db.Admins
                .AsNoTracking()
                .Include(o => o.user)
                .Where(o => o.user.username == username)
                .SingleOrDefault();
        }

        public bool IsUsernameExist(string username)
        {
            return _db.Admins.AsNoTracking()
                            .Include(o => o.user)
                            .Any(o => o.user.username == username);
        }

        public List<Admin> Search()
        {
            throw new NotImplementedException();
        }

        public Admin Update(Admin row)
        {
            Admin dbAdmin = GetById(row.id);

            if (dbAdmin.user.username != row.user.username)
            {
                if (this.IsUsernameExist(row.user.username))
                    throw new ExistingUsernameException("This username already exists, try somehting else");
            }

            dbAdmin.user.username = row.user.username;
            dbAdmin.user.firstName = row.user.firstName;
            dbAdmin.user.lastName = row.user.lastName;
            dbAdmin.user.email = row.user.email;

            User user = dbAdmin.user;

            dbAdmin.user = null;

            _db.Admins.Update(dbAdmin);
            _db.Users.Update(user);
            _db.SaveChanges();
            return row;
        }        
    }
}
