using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Datos
{
    public class RutaRepository : RepositoryBase
    {
        public bool Crear(Ruta ruta)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spcrearruta(@p_nombre)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_nombre", NpgsqlDbType.Varchar, ruta.Nombre);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear ruta: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public List<Ruta> ObtenerTodos()
        {
            var rutas = new List<Ruta>();

            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerrutas()", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rutas.Add(new Ruta
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Disponible = reader.GetBoolean(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rutas: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }

            return rutas;
        }

        public List<Ruta> ObtenerDisponibles()
        {
            var rutas = new List<Ruta>();

            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerrutasdisponibles()", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rutas.Add(new Ruta
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rutas disponibles: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }

            return rutas;
        }

        public bool Actualizar(Ruta ruta)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spactualizarruta(@p_id, @p_nombre)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, ruta.Id);
                    cmd.Parameters.AddWithValue("p_nombre", NpgsqlDbType.Varchar, ruta.Nombre);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar ruta: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT speliminarruta(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar ruta: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public Ruta ObtenerPorId(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerrutaporid(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Ruta
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Disponible = reader.GetBoolean(2)
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener ruta por ID: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public Ruta ObtenerPorNombre(string nombre)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerrutapornombre(@p_nombre)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_nombre", NpgsqlDbType.Varchar, nombre);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Ruta
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Disponible = reader.GetBoolean(2)
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener ruta por nombre: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public bool ExisteRuta(string nombre)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spexisteruta(@p_nombre)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_nombre", NpgsqlDbType.Varchar, nombre);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar existencia de ruta: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT sprutadisponible(@p_rutaid)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_rutaid", NpgsqlDbType.Integer, id);

                    return (bool)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar disponibilidad de ruta: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT sprutatieneasignacionesactivas(@p_rutaid)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_rutaid", NpgsqlDbType.Integer, id);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar asignaciones activas de ruta: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT spmarcarrutadisponible(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al marcar ruta como disponible: {ex.Message}");
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
                using (var cmd = new NpgsqlCommand("SELECT spmarcarrutanodisponible(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("p_id", NpgsqlDbType.Integer, id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al marcar ruta como no disponible: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }
    }
}

