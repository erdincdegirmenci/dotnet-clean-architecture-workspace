using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Interfaces;
using Template.Domain.Entities;
using Template.Domain.Interfaces;
using Template.Domain.QueryTemplate;

namespace Template.Persistence.Repositories
{

    public class UserRepository : BaseRepository<IDatabaseManager>, IUserRepository
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly IQueryTemplate _queryTemplate;

        public UserRepository(IDatabaseManager databaseManager, IQueryTemplate queryTemplate) : base(databaseManager, queryTemplate)
        {
        }

        public User? GetUserById(Guid id)
        {
            var sql = _queryTemplate.GetQuery("UserDAO.GetUserById");
            return base.SelectWithTemplate<User>(sql, new { Guid = id }).FirstOrDefault();
        }
        public User? GetUserByUserName(string username)
        {
            var sql = _queryTemplate.GetQuery("UserDAO.GetUserByUserName");
            return base.SelectWithTemplate<User>(sql, new { Username = username }).FirstOrDefault();
        }
        public User? GetActiveUserByUsername(string username)
        {
            var sql = _queryTemplate.GetQuery("UserDAO.GetActiveUserByUsername");
            return base.SelectWithTemplate<User>(sql, new { Username = username }).FirstOrDefault();
        }

        public int CreateUser(User user)
        {
            var sql = _queryTemplate.GetQuery("UserDAO.CreateUser");
            return base.InsertWithTemplate(sql, new
            {
                user.UserName,
                user.PasswordSalt,
                user.Email,
                user.IsActive,
                CreateDate = DateTime.UtcNow,
                CreateUser = user.UserName
            });
        }

        public int UpdateUserStatus(string username, bool isActive)
        {
            var sql = _queryTemplate.GetQuery("UserDAO.SetUserStatus");
            return base.InsertWithTemplate(sql, new { Username = username, IsActive = isActive });
        }

        public IEnumerable<User> GetAllUser()
        {
            var sql = _queryTemplate.GetQuery("UserDAO.GetAllUsers");
            return base.SelectWithTemplate<User>(sql);
        }
    }
}
