using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Interfaces
{
    public interface IDatabaseManager : IDisposable
    {
        IDbConnection GetConnection();
        IDbTransaction GetTransaction();
        IDbTransaction BeginTransaction();
        IDbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollbackTransaction();
        void SetConnectionName(string connectionName);
        string GetConnectionName();
        IEnumerable<T> Select<T>(string sql, object param);
        IEnumerable<T> SelectProcedure<T>(string sql, IDbCommand command);
        int Delete(string sql, object param);
        int Update(string sql, object param);
        int Insert(string sql, object param);
        void InsertCustom<T>(string sql, T param);
        void AddToTransaction(params IDAO[] dao);
        void AddToTransaction(params IService[] service);
        void SetAsSubTransaction();
        void BulkInsert(string sql, List<object> param);
        void BulkUpdate(string sql, List<object> param);
        void BulkDelete(string sql, List<object> param);
    }
}
