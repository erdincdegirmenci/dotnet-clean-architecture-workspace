using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Security
{
    public class UserContextModel : IUserContextModel
    {
        public object _userData;
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
        public bool IsActive { get; set; }
        public U GetUserData<U>()
        {
            if (_userData != null && !string.IsNullOrEmpty(_userData.ToString()))
                return JsonConvert.DeserializeObject<U>(_userData.ToString());

            return default(U);
        }
        public void SetUserData<U>(U value)
        {
            _userData = value;
        }
    }
}
