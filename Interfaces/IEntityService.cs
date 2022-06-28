using BikesTest.Models;
using BikesTest.Models.GlueModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Interfaces
{
    public interface IEntityService<T>
    {
        T Create(T row);
        T Update(T row);
        void Delete(int id);
        void Delete(T row);
        T GetById(int? id);
        List<T> GetAll();
        List<T> Search();
        T GetByUsername(string username);

    }
}
