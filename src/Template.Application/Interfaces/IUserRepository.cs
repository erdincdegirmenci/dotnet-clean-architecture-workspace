using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities;

namespace Template.Application.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        User? GetUserById(Guid id);
        User? GetUserByUserName(string username);
        int CreateUser(User user);
    }

}
