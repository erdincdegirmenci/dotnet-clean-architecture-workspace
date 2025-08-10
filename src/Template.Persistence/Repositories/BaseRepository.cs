using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Interfaces;
using Template.Domain.QueryTemplate;

namespace Template.Persistence.Repositories
{

    public abstract class BaseRepository<T> : IDisposable where T : IDatabaseManager
    {
        T _databaseManager;
        T _externalDatabaseManager;
        readonly IQueryTemplate _queryTemplate;

        protected BaseRepository(T databaseManager)
        {
            _databaseManager = databaseManager;
        }

        protected BaseRepository(T databaseManager, IQueryTemplate queryTemplate)
        {
            _databaseManager = databaseManager;
            _queryTemplate = queryTemplate;

        }

        IDatabaseManager GetActiveDatabaseManager()
        {
            if (_externalDatabaseManager != null)
                return _externalDatabaseManager;

            return _databaseManager;
        }

        protected void BeginTransaction()
        {
            if (_externalDatabaseManager == null)
                _databaseManager.BeginTransaction();
        }


        protected void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_externalDatabaseManager == null)
                _databaseManager.BeginTransaction(isolationLevel);
        }

        protected void CommitTransaction()
        {
            if (_externalDatabaseManager == null)
                _databaseManager.CommitTransaction();
        }

        protected void RollbackTransaction()
        {
            if (_externalDatabaseManager == null)
                _databaseManager.RollbackTransaction();
        }

        //private void SetExternalDatabaseManager(T databaseManager)
        //{

        //}

        protected void InsertCustomWithTemplate<T>(string queryname, T param)
        {
            InsertCustom<T>(_queryTemplate.GetQuery(queryname), param);
        }

        protected void InsertCustomWithTemplate<T>(string queryname, string dynamicWhereClause, T param)
        {
            InsertCustom<T>(_queryTemplate.GetQuery(queryname, dynamicWhereClause), param);
        }

        protected void InsertCustom<T>(string sql, T param)
        {
            GetActiveDatabaseManager().InsertCustom<T>(sql, param);
        }

        protected int InsertWithTemplate(string queryname, object param)
        {
            return Insert(_queryTemplate.GetQuery(queryname), param);
        }

        protected int InsertWithTemplate(string queryname, string dynamicWhereClause, object param)
        {
            return Insert(_queryTemplate.GetQuery(queryname, dynamicWhereClause), param);
        }

        protected int Insert(string sql, object param)
        {
            return GetActiveDatabaseManager().Insert(sql, param);
        }

        protected void BulkInsert(string sql, List<object> param)
        {
            GetActiveDatabaseManager().BulkInsert(sql, param);
        }

        protected void BulkInsertWithTemplate(string queryname, List<object> param)
        {
            BulkInsert(_queryTemplate.GetQuery(queryname), param);
        }


        protected int UpdateWithTemplate(string queryname, object param)
        {
            return Update(_queryTemplate.GetQuery(queryname), param);
        }

        protected int UpdateWithTemplate(string queryname, string dynamicWhereClause, object param)
        {
            return Update(_queryTemplate.GetQuery(queryname, dynamicWhereClause), param);
        }

        protected int Update(string sql, object param)
        {
            return GetActiveDatabaseManager().Update(sql, param);
        }
        protected void BulkUpdate(string sql, List<object> param)
        {
            GetActiveDatabaseManager().BulkUpdate(sql, param);
        }

        protected void BulkUpdateWithTemplate(string queryname, List<object> param)
        {
            BulkUpdate(_queryTemplate.GetQuery(queryname), param);
        }

        protected int DeleteWithTemplate<T>(string queryname, object param)
        {
            return Delete(_queryTemplate.GetQuery(queryname), param);
        }

        protected int DeleteWithTemplate<T>(string queryname, string dynamicWhereClause, object param)
        {
            return Delete(_queryTemplate.GetQuery(queryname, dynamicWhereClause), param);
        }
        protected int DeleteWithTemplate(string queryname, object param)
        {
            return Delete(_queryTemplate.GetQuery(queryname), param);
        }

        protected int DeleteWithTemplate(string queryname, string dynamicWhereClause, object param)
        {
            return Delete(_queryTemplate.GetQuery(queryname, dynamicWhereClause), param);
        }

        protected int Delete(string sql, object param)
        {
            return GetActiveDatabaseManager().Delete(sql, param);
        }
        protected void BulkDelete(string sql, List<object> param)
        {
            GetActiveDatabaseManager().BulkDelete(sql, param);
        }

        protected void BulkDeleteWithTemplate(string queryname, List<object> param)
        {
            BulkDelete(_queryTemplate.GetQuery(queryname), param);
        }

        protected IEnumerable<T> SelectWithTemplate<T>(string queryname, object param)
        {
            return Select<T>(_queryTemplate.GetQuery(queryname), param);
        }

        protected IEnumerable<T> SelectWithTemplate<T>(string queryname, string dynamicWhereClause, object param)
        {
            return Select<T>(_queryTemplate.GetQuery(queryname, dynamicWhereClause), param);
        }

        protected IEnumerable<T> Select<T>(string sql, object param)
        {
            return GetActiveDatabaseManager().Select<T>(sql, param);
        }

        protected IEnumerable<T> SelectWithTemplate<T>(string queryname)
        {
            return Select<T>(_queryTemplate.GetQuery(queryname));
        }

        protected IEnumerable<T> SelectWithTemplate<T>(string queryname, string dynamicWhereClause)
        {
            return Select<T>(_queryTemplate.GetQuery(queryname, dynamicWhereClause));
        }

        protected IEnumerable<T> Select<T>(string sql)
        {
            return Select<T>(sql, null);
        }

        protected IEnumerable<T> SelectProcedureWithTemplate<T>(string queryname, IDbCommand command)
        {
            return SelectProcedure<T>(_queryTemplate.GetQuery(queryname), command);
        }

        protected IEnumerable<T> SelectProcedure<T>(string sql, IDbCommand command)
        {
            return GetActiveDatabaseManager().SelectProcedure<T>(sql, command);
        }

        public void Dispose()
        {
            _databaseManager?.Dispose();
        }

        internal void AddToExternalTransaction(IDatabaseManager databaseManager)
        {
            if (_externalDatabaseManager == null)
            {
                databaseManager.SetConnectionName(_databaseManager.GetConnectionName());
                _externalDatabaseManager = (T)databaseManager;
            }
            else
            {
                databaseManager.SetAsSubTransaction();
            }
        }

        internal void RemoveFromExternalTransaction()
        {
            if (_externalDatabaseManager != null)
            {
                _externalDatabaseManager = default(T);
            }
        }
    }
}
