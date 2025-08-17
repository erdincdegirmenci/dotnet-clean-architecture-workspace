using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Security
{
    public interface IUserContextService
    {
        public UserContextModel GetUser(string userName);
        public UserContextModel BasicAuthenticateUser(string username, string password);
    }
}
