using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Reflection;

namespace Template.Persistence.Helper
{
    public class OracleParameterMapper
    {
        public static Dapper.DynamicParameters MapToDynamicParameters(object model, OracleConnection connection)
        {
            Dapper.DynamicParameters parameters = new Dapper.DynamicParameters();
            if (model == null)
                return parameters;

            foreach (PropertyInfo sourceProp in model.GetType().GetProperties())
            {
                object value = sourceProp.GetValue(model, null);
                if (sourceProp.GetCustomAttributes<Attribute>(true).Any())
                {

                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        byte[] newvalue = System.Text.Encoding.Unicode.GetBytes(value.ToString());
                        var clob = new OracleClob(connection);
                        clob.Write(newvalue, 0, newvalue.Length);
                    }
                }
                parameters.Add(name: sourceProp.Name, value: value, direction: System.Data.ParameterDirection.Input);
            }

            return parameters;
        }

        public static OracleDynamicParameters MapToOracleDynamicParameters(object model, OracleConnection connection)
        {
            OracleDynamicParameters parameters = new OracleDynamicParameters();
            if (model == null)
                return parameters;

            foreach (PropertyInfo sourceProp in model.GetType().GetProperties())
            {
                object value = sourceProp.GetValue(model, null);
                if (sourceProp.GetCustomAttributes<Attribute>(true).Any())
                {

                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        byte[] newvalue = System.Text.Encoding.Unicode.GetBytes(value.ToString());
                        var clob = new OracleClob(connection);
                        clob.Write(newvalue, 0, newvalue.Length);
                    }
                    parameters.Add(name: sourceProp.Name, value: value, OracleMappingType.Clob, direction: System.Data.ParameterDirection.Input);

                }
                else
                {
                    parameters.Add(name: sourceProp.Name, value: value, direction: System.Data.ParameterDirection.Input);
                }
            }

            return parameters;
        }
    }
}
