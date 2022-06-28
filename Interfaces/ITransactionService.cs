using BikesTest.Models.GlueModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Interfaces
{
    public interface ITransactionService<T> : IEntityService<T>
    {
        T NegateTransaction(T row);

        List<T> GetAllDeleted();

        T GetByDeletedId(int? id);
        
    }
}
