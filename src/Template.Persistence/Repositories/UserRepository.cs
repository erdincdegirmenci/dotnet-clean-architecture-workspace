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

        public IEnumerable<User> GetAllUser()
        {
            var sql = _queryTemplate.GetQuery("UserDAO.GetAllUsers");
            return base.SelectWithTemplate<User>(sql);
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
        public int CreateUser(User user)
        {
            var sql = _queryTemplate.GetQuery("UserDAO.CreateUser");
            return base.InsertWithTemplate(sql, user);
        }
        public IEnumerable<User> GetUsersByDynamicCondition(string? userName = null, string? role = null)
        {
            string whereClause = "";

            if (!String.IsNullOrEmpty(userName))
                whereClause = $"UserName = '{userName}'";
            else if (!String.IsNullOrEmpty(role))
                whereClause = $"Role = '{role}'";
            else
                whereClause = "1=1"; // Filtre yoksa tüm kayıtları al

            var sql = _queryTemplate.GetQuery("UserDAO.GetUsersByDynamicCondition", whereClause);
            return base.SelectWithTemplate<User>(sql);
        }

    }
}
