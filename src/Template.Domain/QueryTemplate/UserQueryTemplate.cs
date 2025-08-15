using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.QueryTemplate
{
    public class UserQueryTemplate : IQueryTemplate
    {
        readonly Dictionary<string, string> _queries = new Dictionary<string, string>();

        public UserQueryTemplate()
        {
            _queries.Add("UserDAO.GetAllUsers", @"SELECT Id, UserName, Email, Role, PasswordHash, PasswordSalt FROM [USER_TABLE]");

            _queries.Add("UserDAO.GetUserById", @"SELECT Id, UserName, Email, Role, PasswordHash, PasswordSalt 
                                                  FROM [USER_TABLE] 
                                                  WHERE Id = @Guid");

            _queries.Add("UserDAO.GetUserByUserName", @"SELECT Id, UserName, Email, Role, PasswordHash, PasswordSalt 
                                                    FROM [USER_TABLE] 
                                                    WHERE UserName = @Username");

            _queries.Add("UserDAO.CreateUser", @"INSERT INTO [USER_TABLE] (Id, UserName, Email, Role, PasswordHash, PasswordSalt) 
                                                    VALUES (@Id, @UserName, @Email, @Role, @PasswordHash, @PasswordSalt)");

            _queries.Add("UserDAO.GetUsersByDynamicCondition", @"SELECT Id, UserName, Email, Role, PasswordHash, PasswordSalt 
                                                  FROM [USER_TABLE] 
                                                  [DynamicWhereClause]");

        }
        public string GetQuery(string key)
        {
            if (!_queries.TryGetValue(key, out string value))
                throw new Exception("Query is not found. Query Key : " + key);
            return value;
        }

        public string GetQuery(string key, string dynamicWhereClause)
        {
            return GetQuery(key).Replace("[DynamicWhereClause]", $"where {dynamicWhereClause}");
        }
    }
}
