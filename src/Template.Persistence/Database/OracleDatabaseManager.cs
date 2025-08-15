using Dapper;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Template.Application.Managers;
using Template.Domain.Interfaces;
using Template.Persistence.Helper;
using Template.Persistence.Repositories;

namespace Template.Persistence.Database
{
    public class OracleDatabaseManager : IDatabaseManager
    {
        string _connectionName;
        string _connectionString;

        OracleConnection _connection;
        OracleTransaction _transaction;
        IConfigManager _configManager;

        bool _isSubTransaction = false;

        List<BaseRepository<IDatabaseManager>> _attachedDAOs = new List<BaseRepository<IDatabaseManager>>();

        public OracleDatabaseManager(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public OracleDatabaseManager(string connectionName, IConfigManager configManager)
        {
            _connectionName = connectionName;
            _configManager = configManager;
        }

        public IDbConnection GetConnection()
        {
            if (_connection == null)
            {
                _connectionString = _configManager.GetConfig(_connectionName); ;
                _connection = new OracleConnection(_connectionString);
            }

            return _connection;
        }


        public IDbTransaction BeginTransaction()
        {
            // Oracle database default isolation level !!!!
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public IDbTransaction GetTransaction()
        {
            return _transaction;
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_isSubTransaction)
                return null;

            IDbConnection connection = GetConnection();
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            _transaction = connection.BeginTransaction(isolationLevel) as OracleTransaction;
            return _transaction;
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                CloseTransaction();
            }

            CloseConnection();
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                CloseTransaction();
            }

            CloseConnection();
        }

        public void Dispose()
        {
            CloseTransaction();
            CloseConnection();

            if (!_isSubTransaction)
                RemoveDAOsFromTranscations();
        }

        void RemoveDAOsFromTranscations()
        {
            foreach (var dao in _attachedDAOs)
            {
                if (dao != null)
                    dao.RemoveFromExternalTransaction();
            }
        }

        void CloseTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        void CloseConnection()
        {
            if (_connection != null)
            {

                if (_connection.State != System.Data.ConnectionState.Closed)
                {
                    _connection.Close();
                }

                _connection.Dispose();
                _connection = null;
            }
        }

        public IEnumerable<T> Select<T>(string sql, object param)
        {
            IEnumerable<T> returnData;
            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                returnData = connection.Query<T>(sql, param, transaction);
            }
            else
            {
                try
                {
                    connection.Open();
                    returnData = connection.Query<T>(sql, param);
                }
                finally
                {

                    CloseConnection();
                }
            }

            return returnData;
        }

        public IEnumerable<T> SelectProcedure<T>(string sql, IDbCommand command)
        {
            IEnumerable<T> returnData;
            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();

            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                command.Connection = connection;
                command.Transaction = transaction;
                IDataReader reader = command.ExecuteReader();

                List<T> list = new List<T>();
                T obj = default(T);

                while (reader.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        ColumnAttribute attribute = prop.GetCustomAttribute<ColumnAttribute>();

                        if (!object.Equals(attribute.Name, DBNull.Value) && Enumerable.Range(0, reader.FieldCount).Any(i => reader.GetName(i) == attribute.Name))
                        {
                            object convertedValue = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(reader[attribute.Name].ToString());
                            prop.SetValue(obj, convertedValue, null);
                        }
                    }
                    list.Add(obj);
                }

                returnData = list;
            }
            else
            {
                try
                {
                    connection.Open();
                    command.Connection = connection;
                    IDataReader reader = command.ExecuteReader();

                    List<T> list = new List<T>();
                    T obj = default(T);

                    while (reader.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            ColumnAttribute attribute = prop.GetCustomAttribute<ColumnAttribute>();

                            if (!object.Equals(attribute.Name, DBNull.Value) && Enumerable.Range(0, reader.FieldCount).Any(i => reader.GetName(i) == attribute.Name))
                            {
                                object convertedValue = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(reader[attribute.Name].ToString());
                                prop.SetValue(obj, convertedValue, null);
                            }
                        }
                        list.Add(obj);
                    }

                    returnData = list;
                }
                finally
                {
                    CloseConnection();
                }
            }

            return returnData;
        }

        public int Delete(string sql, object param)
        {
            int updatedRowCount;
            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                updatedRowCount = connection.Execute(sql, param, transaction);
            }
            else
            {
                try
                {
                    connection.Open();
                    updatedRowCount = connection.Execute(sql, param);
                }
                finally
                {
                    CloseConnection();
                }
            }

            return updatedRowCount;
        }

        public int Update(string sql, object param)
        {
            int updatedRowCount;
            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();
            OracleDynamicParameters model;

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                model = param is OracleDynamicParameters ? param as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(param, (OracleConnection)connection);

                updatedRowCount = connection.Execute(sql, model, transaction);
            }
            else
            {
                try
                {
                    connection.Open();

                    model = param is OracleDynamicParameters ? param as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(param, (OracleConnection)connection);

                    updatedRowCount = connection.Execute(sql, model);
                }
                finally
                {
                    CloseConnection();
                }
            }

            return updatedRowCount;
        }

        public int Insert(string sql, object param)
        {

            int identity = 0;
            OracleDynamicParameters model;


            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                model = param is OracleDynamicParameters ? param as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(param, (OracleConnection)connection);
                //Id bilgisinin donusu sorguda LASTCID parametresi ile yapilmali
                model.Add(name: "LASTCID", dbType: OracleMappingType.Int32, direction: System.Data.ParameterDirection.Output);

                connection.Execute(sql, model, transaction);
            }
            else
            {
                try
                {
                    connection.Open();

                    model = param is OracleDynamicParameters ? param as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(param, (OracleConnection)connection);
                    //Id bilgisinin donusu sorguda LASTCID parametresi ile yapilmali
                    model.Add(name: "LASTCID", dbType: OracleMappingType.Int32, direction: System.Data.ParameterDirection.Output);

                    connection.Execute(sql, model);
                }
                finally
                {

                    CloseConnection();
                }
            }


            var lastcid = model.Get<object>("LASTCID");
            if (lastcid != null)
            {
                identity = int.Parse(lastcid.ToString());
            }

            return identity;

        }


        public void InsertCustom<T>(string sql, T param)
        {
            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();
            OracleDynamicParameters model;

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                model = param is OracleDynamicParameters ? param as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(param, (OracleConnection)connection);

                connection.Execute(sql, model, transaction);
            }
            else
            {
                try
                {
                    connection.Open();

                    model = param is OracleDynamicParameters ? param as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(param, (OracleConnection)connection);

                    connection.Execute(sql, model);
                }
                finally
                {
                    CloseConnection();
                }
            }

        }

        public void SetConnectionName(string connectionName)
        {
            _connectionName = connectionName;
        }
        public string GetConnectionName()
        {
            return _connectionName;
        }

        public void AddToTransaction(params IDAO[] dao)
        {
            foreach (var transactionDao in dao)
            {
                BaseRepository<IDatabaseManager> baseDAO = (BaseRepository<IDatabaseManager>)transactionDao;
                baseDAO.AddToExternalTransaction(this);

                _attachedDAOs.Add(baseDAO);
            }

        }
        public void AddToTransaction(params IService[] service)
        {
            foreach (var transactionService in service)
            {
                transactionService.AddToExternalTransaction(this);
            }

        }
        public void SetAsSubTransaction()
        {
            _isSubTransaction = true;
        }

        public void BulkInsert(string sql, List<object> param)
        {

            OracleDynamicParameters model;


            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                foreach (var item in param)
                {
                    model = item is OracleDynamicParameters ? item as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(item, (OracleConnection)connection);
                    //Id bilgisinin donusu sorguda LASTCID parametresi ile yapilmali
                    model.Add(name: "LASTCID", dbType: OracleMappingType.Int32, direction: System.Data.ParameterDirection.Output);

                    connection.Execute(sql, model, transaction);
                }

            }
            else
            {
                try
                {
                    connection.Open();

                    foreach (var item in param)
                    {
                        model = item is OracleDynamicParameters ? item as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(item, (OracleConnection)connection);
                        //Id bilgisinin donusu sorguda LASTCID parametresi ile yapilmali
                        model.Add(name: "LASTCID", dbType: OracleMappingType.Int32, direction: System.Data.ParameterDirection.Output);

                        connection.Execute(sql, model);
                    }
                }
                finally
                {

                    CloseConnection();
                }
            }

        }

        public void BulkUpdate(string sql, List<object> param)
        {

            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();
            OracleDynamicParameters model;

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                foreach (var item in param)
                {
                    model = item is OracleDynamicParameters ? item as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(item, (OracleConnection)connection);

                    connection.Execute(sql, model, transaction);
                }

            }
            else
            {
                try
                {
                    connection.Open();

                    foreach (var item in param)
                    {
                        model = item is OracleDynamicParameters ? item as OracleDynamicParameters : OracleParameterMapper.MapToOracleDynamicParameters(item, (OracleConnection)connection);

                        connection.Execute(sql, model);
                    }

                }
                finally
                {

                    CloseConnection();
                }
            }
        }

        public void BulkDelete(string sql, List<object> param)
        {

            IDbTransaction transaction = GetTransaction();
            IDbConnection connection = GetConnection();

            if (transaction != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                foreach (var item in param)
                {
                    connection.Execute(sql, item, transaction);
                }

            }
            else
            {
                try
                {
                    connection.Open();

                    foreach (var item in param)
                    {
                        connection.Execute(sql, item);
                    }
                }
                finally
                {

                    CloseConnection();
                }
            }

        }
    }
}
