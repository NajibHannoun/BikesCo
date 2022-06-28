using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikesTest.Interfaces
{
    public interface IUserService<T> : IEntityService<T>
    {
        bool IsUsernameExist(string username);

        T GetByUserId(int id);
    }
}
