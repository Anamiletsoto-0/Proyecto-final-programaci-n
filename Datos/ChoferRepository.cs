using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Datos
{
    public class ChoferRepository : RepositoryBase
    {
        public ChoferRepository()
        {
            // Constructor sin parámetros
        }

        public bool Crear(Chofer chofer)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spcrearchofer(@p_nombre, @p_apellido, @p_fechanacimiento, @p_cedula)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_nombre", NpgsqlDbType.Varchar, chofer.Nombre);
                    cmd.Parameters.AddWithValue("p_apellido", NpgsqlDbType.Varchar, chofer.Apellido);
                    cmd.Parameters.AddWithValue("p_fechanacimiento", NpgsqlDbType.Date, chofer.FechaNacimiento);
                    cmd.Parameters.AddWithValue("p_cedula", NpgsqlDbType.Varchar, chofer.Cedula);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear chofer: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public List<Chofer> ObtenerTodos()
        {
            var choferes = new List<Chofer>();

            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerchoferes()", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            choferes.Add(new Chofer
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                FechaNacimiento = reader.GetDateTime(3),
                                Cedula = reader.GetString(4),
                                Disponible = reader.GetBoolean(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener choferes: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }

            return choferes;
        }

        public List<Chofer> ObtenerDisponibles()
        {
            var choferes = new List<Chofer>();

            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerchoferesdisponibles()", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            choferes.Add(new Chofer
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Cedula = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener choferes disponibles: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }

            return choferes;
        }

        public bool Actualizar(Chofer chofer)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spactualizarchofer(@p_id, @p_nombre, @p_apellido, @p_fechanacimiento, @p_cedula)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, chofer.Id);
                    cmd.Parameters.AddWithValue("p_nombre", NpgsqlDbType.Varchar, chofer.Nombre);
                    cmd.Parameters.AddWithValue("p_apellido", NpgsqlDbType.Varchar, chofer.Apellido);
                    cmd.Parameters.AddWithValue("p_fechanacimiento", NpgsqlDbType.Date, chofer.FechaNacimiento);
                    cmd.Parameters.AddWithValue("p_cedula", NpgsqlDbType.Varchar, chofer.Cedula);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar chofer: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT speliminarchofer(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar chofer: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public Chofer ObtenerPorId(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerchoferporid(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Chofer
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                FechaNacimiento = reader.GetDateTime(3),
                                Cedula = reader.GetString(4),
                                Disponible = reader.GetBoolean(5)
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener chofer por ID: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public bool EstaDisponible(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spchoferdisponible(@p_choferid)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_choferid", NpgsqlDbType.Integer, id);

                    return (bool)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar disponibilidad de chofer: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public bool TieneAsignacionesActivas(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spchofertieneasignacionesactivas(@p_choferid)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_choferid", NpgsqlDbType.Integer, id);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar asignaciones activas de chofer: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }
    }
}