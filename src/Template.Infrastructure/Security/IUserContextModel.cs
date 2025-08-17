    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Security
{
    public interface IUserContextModel
    {
        int UserId { get; set; }
        string Username { get; set; }
        List<string> Roles { get; set; }
        List<string> Permissions { get; set; }
        bool IsActive { get; set; }
        U GetUserData<U>();
        void SetUserData<U>(U value);
    }
}
