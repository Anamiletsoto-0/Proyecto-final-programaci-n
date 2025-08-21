using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Datos
{
    public class AutobusRepository : RepositoryBase
    {
        public bool Crear(Autobus autobus)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spcrearautobus(@p_marca, @p_modelo, @p_placa, @p_color, @p_anio)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_marca", NpgsqlDbType.Varchar, autobus.Marca);
                    cmd.Parameters.AddWithValue("p_modelo", NpgsqlDbType.Varchar, autobus.Modelo);
                    cmd.Parameters.AddWithValue("p_placa", NpgsqlDbType.Varchar, autobus.Placa);
                    cmd.Parameters.AddWithValue("p_color", NpgsqlDbType.Varchar, autobus.Color);
                    cmd.Parameters.AddWithValue("p_anio", NpgsqlDbType.Integer, autobus.Anio);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear autobús: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public List<Autobus> ObtenerTodos()
        {
            var autobuses = new List<Autobus>();

            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerautobuses()", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            autobuses.Add(new Autobus
                            {
                                Id = reader.GetInt32(0),
                                Marca = reader.GetString(1),
                                Modelo = reader.GetString(2),
                                Placa = reader.GetString(3),
                                Color = reader.GetString(4),
                                Anio = reader.GetInt32(5),
                                Disponible = reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener autobuses: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }

            return autobuses;
        }

        public List<Autobus> ObtenerDisponibles()
        {
            var autobuses = new List<Autobus>();

            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerautobusesdisponibles()", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            autobuses.Add(new Autobus
                            {
                                Id = reader.GetInt32(0),
                                Marca = reader.GetString(1),
                                Modelo = reader.GetString(2),
                                Placa = reader.GetString(3),
                                Color = reader.GetString(4),
                                Anio = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener autobuses disponibles: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }

            return autobuses;
        }

        public bool Actualizar(Autobus autobus)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spactualizarautobus(@p_id, @p_marca, @p_modelo, @p_placa, @p_color, @p_anio)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, autobus.Id);
                    cmd.Parameters.AddWithValue("p_marca", NpgsqlDbType.Varchar, autobus.Marca);
                    cmd.Parameters.AddWithValue("p_modelo", NpgsqlDbType.Varchar, autobus.Modelo);
                    cmd.Parameters.AddWithValue("p_placa", NpgsqlDbType.Varchar, autobus.Placa);
                    cmd.Parameters.AddWithValue("p_color", NpgsqlDbType.Varchar, autobus.Color);
                    cmd.Parameters.AddWithValue("p_anio", NpgsqlDbType.Integer, autobus.Anio);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar autobús: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT speliminarautobus(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar autobús: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public Autobus ObtenerPorId(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerautobusporid(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Autobus
                            {
                                Id = reader.GetInt32(0),
                                Marca = reader.GetString(1),
                                Modelo = reader.GetString(2),
                                Placa = reader.GetString(3),
                                Color = reader.GetString(4),
                                Anio = reader.GetInt32(5),
                                Disponible = reader.GetBoolean(6)
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener autobús por ID: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT spautobusdisponible(@p_autobusid)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_autobusid", NpgsqlDbType.Integer, id);

                    return (bool)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar disponibilidad de autobús: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT spautobustieneasignacionesactivas(@p_autobusid)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_autobusid", NpgsqlDbType.Integer, id);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar asignaciones activas de autobús: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public void MarcarComoDisponible(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spmarcarautobusdisponible(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al marcar autobús como disponible: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public void MarcarComoNoDisponible(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spmarcarautobusnodisponible(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al marcar autobús como no disponible: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }
    }
}
