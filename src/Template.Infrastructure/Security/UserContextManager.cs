using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Security
{
    public class UserContextManager : IUserContextManager<IUserContextModel>
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IUserContextService _userContextService;
        public UserContextManager(IHttpContextAccessor httpContextAccessor, IUserContextService userContextService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userContextService = userContextService;
        }
        public void DeleteUser()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Remove("UserContext");
            }
        }

        public IUserContextModel GetUser()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                string userContextString = _httpContextAccessor.HttpContext.Session.GetString("UserContext");
                UserContextModel sessionModel = null;

                if (!string.IsNullOrEmpty(userContextString))
                {
                    sessionModel = JsonConvert.DeserializeObject<UserContextModel>(userContextString);
                }

                return sessionModel;
            }
            return null;
        }

        public bool HasRole(string role)
        {
            IUserContextModel user = GetUser();
            if (user == null || user.Roles == null)
                return false;

            return user.Roles.Any(x => x.Equals(role));

        }
        public bool HasPermission(string permission)
        {
            IUserContextModel user = GetUser();
            if (user == null || user.Permissions == null)
                return false;

            return user.Permissions.Any(x => x.Equals(permission));

        }

        public bool SetUser(IUserContextModel user)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserContext", JsonConvert.SerializeObject(user));
            }
            return true;
        }
        public bool SetUser(string userName)
        {

            if (string.IsNullOrEmpty(userName))
                return false;

            IUserContextModel user = _userContextService.GetUser(userName);

            if (user == null)
                return false;

            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserContext", JsonConvert.SerializeObject(user));
            }

            return true;
        }
    }
}
