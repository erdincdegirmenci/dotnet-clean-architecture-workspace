using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Interfaces;
using Template.Application.Managers;
using Template.Domain.Interfaces;

namespace Template.Infrastructure.Security
{
    public class UserContextService : IUserContextService
    {
        private IUserRepository _userContextDAO;
        private readonly IMapper _mapper;
        private readonly IConfigManager _configManager;

        // Sadece frameworkten gelen JWT authenticatin sonrası user bilgisini almak için kullanılır
        public UserContextService(IUserRepository userContextDAO, IMapper mapper, IConfigManager configManager)
        {
            _userContextDAO = userContextDAO;
            _mapper = mapper;
            _configManager = configManager;

        }

        public UserContextModel GetUser(string userName)
        {
            //UserContextUserDAOModel userDAO = _userContextDAO.GetUser(userName);
            //if (userDAO == null)
            //    return null;
            //UserContextModel user = _mapper.Map<UserContextUserDAOModel, UserContextModel>(userDAO);
            //user.SetUserData<UserDetailBusinessResponseModel>(_mapper.Map<UserContextUserDAOModel, UserDetailBusinessResponseModel>(userDAO));
            //return user;
            return new UserContextModel();
        }

        public UserContextModel BasicAuthenticateUser(string username, string password)
        {
            //string hashPassword = HashingHelper.SHA256Hash(_configManager.GetConfig("AppSettings:PasswordHashKey"), password);
            //UserContextUserDAOModel userDAO = _userContextDAO.GetAuthenticatedUser(username, hashPassword);
            //if (userDAO == null)
            //    return null;
            //UserContextModel user = _mapper.Map<UserContextUserDAOModel, UserContextModel>(userDAO);
            //user.SetUserData<UserDetailBusinessResponseModel>(_mapper.Map<UserContextUserDAOModel, UserDetailBusinessResponseModel>(userDAO));
            //return user;

            return new UserContextModel();
        }
    }
}
