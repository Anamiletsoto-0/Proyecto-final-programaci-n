using System;
using System.Configuration; // Para ConfigurationManager
using Npgsql; // Para NpgsqlConnection
using NpgsqlTypes; // Para tipos de datos de PostgreSQL

namespace ControlAutobuses.Datos
{
    public sealed class ConexionBD
    {
        private static ConexionBD instance = null;
        private static readonly object padlock = new object();
        private NpgsqlConnection connection;

        // Cadena de conexión directa para PostgreSQL
        private readonly string connectionString = "Server=localhost;Port=5432;Database=ControlAutobuses;User Id=postgres;Password=20942094;";

        private ConexionBD()
        {
            connection = new NpgsqlConnection(connectionString);
        }

        public static ConexionBD Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ConexionBD();
                    }
                    return instance;
                }
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return connection;
        }

        public void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public bool TestConnection()
        {
            try
            {
                OpenConnection();
                CloseConnection();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}