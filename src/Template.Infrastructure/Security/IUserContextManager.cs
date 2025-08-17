using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Security
{
    public interface IUserContextManager<T>
    {
        bool SetUser(T user);
        bool SetUser(string userName);
        T GetUser();
        bool HasRole(string role);
        bool HasPermission(string permission);
        void DeleteUser();

    }
}
