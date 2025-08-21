using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace ControlAutobuses.Datos
{
    public abstract class RepositoryBase
    {
        protected readonly ConexionBD conexion;

        protected RepositoryBase()
        {
            conexion = ConexionBD.Instance;
        }

        protected NpgsqlParameter CreateParameter(string parameterName, object value)
        {
            return new NpgsqlParameter(parameterName, value ?? DBNull.Value);
        }

        protected NpgsqlParameter CreateParameter(string parameterName, NpgsqlDbType dbType, object value)
        {
            var parameter = new NpgsqlParameter(parameterName, dbType)
            {
                Value = value ?? DBNull.Value
            };
            return parameter;
        }
    }
}
