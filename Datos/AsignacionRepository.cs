using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using ControlAutobuses.Entidades;
using System.Collections.Generic;

namespace ControlAutobuses.Datos
{
    public class AsignacionRepository : RepositoryBase
    {
        public AsignacionRepository() { }

        public bool Crear(Asignacion asignacion)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spcrearasignacion(@p_choferid, @p_autobusid, @p_rutaid)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text; // ✅ AÑADIR ESTO
                    cmd.Parameters.AddWithValue("@p_choferid", NpgsqlDbType.Integer, asignacion.ChoferId);
                    cmd.Parameters.AddWithValue("@p_autobusid", NpgsqlDbType.Integer, asignacion.AutobusId);
                    cmd.Parameters.AddWithValue("@p_rutaid", NpgsqlDbType.Integer, asignacion.RutaId);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear asignación: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public List<Asignacion> ObtenerActivas()
        {
            List<Asignacion> lista = new List<Asignacion>();
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spobtenerasignacionesactivas()", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text; // ✅ AÑADIR ESTO
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Asignacion a = new Asignacion();
                            a.Id = reader.GetInt32(0);
                            a.ChoferId = reader.GetInt32(1);
                            a.AutobusId = reader.GetInt32(2);
                            a.RutaId = reader.GetInt32(3);
                            a.FechaAsignacion = reader.GetDateTime(4);
                            a.Activa = reader.GetBoolean(5);
                            a.NombreChofer = reader.GetString(6);
                            a.PlacaAutobus = reader.GetString(7);
                            a.NombreRuta = reader.GetString(8);
                            lista.Add(a);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener asignaciones: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
            return lista;
        }

        public bool Finalizar(int id)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spfinalizarasignacion(@p_id)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text; // ✅ AÑADIR ESTO
                    cmd.Parameters.AddWithValue("@p_id", NpgsqlDbType.Integer, id);
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al finalizar asignación: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }
    }
}