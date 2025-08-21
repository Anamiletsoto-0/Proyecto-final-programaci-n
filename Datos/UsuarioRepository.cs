using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Datos
{
    public class UsuarioRepository : RepositoryBase
    {
        public UsuarioRepository()
        {
            // Constructor sin parámetros
        }

        public Usuario Autenticar(string nombreUsuario, string contrasena)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT * FROM spautenticarusuario(@p_nombreusuario, @p_contrasena)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text; // ✅ CORRECTO
                    cmd.Parameters.AddWithValue("p_nombreusuario", NpgsqlDbType.Varchar, nombreUsuario);
                    cmd.Parameters.AddWithValue("p_contrasena", NpgsqlDbType.Varchar, contrasena);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = reader.GetInt32(0),
                                NombreUsuario = reader.GetString(1),
                                TipoUsuario = reader.GetString(2)
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en autenticación: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public bool CambiarContrasena(int usuarioId, string nuevaContrasena)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spcambiarcontrasena(@p_usuarioid, @p_nuevacontrasena)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text; // ✅ CORRECTO
                    cmd.Parameters.AddWithValue("p_usuarioid", NpgsqlDbType.Integer, usuarioId);
                    cmd.Parameters.AddWithValue("p_nuevacontrasena", NpgsqlDbType.Varchar, nuevaContrasena);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cambiar contraseña: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public bool CrearUsuario(Usuario usuario)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spcrearusuario(@p_nombreusuario, @p_contrasena, @p_tipousuario)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text; // ✅ CORRECTO
                    cmd.Parameters.AddWithValue("p_nombreusuario", NpgsqlDbType.Varchar, usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("p_contrasena", NpgsqlDbType.Varchar, usuario.Contrasena);
                    cmd.Parameters.AddWithValue("p_tipousuario", NpgsqlDbType.Varchar, usuario.TipoUsuario);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear usuario: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }

        public bool ExisteUsuario(string nombreUsuario)
        {
            try
            {
                conexion.OpenConnection();
                using (var cmd = new NpgsqlCommand("SELECT spexisteusuario(@p_nombreusuario)", conexion.GetConnection()))
                {
                    cmd.CommandType = CommandType.Text; // ✅ CORRECTO
                    cmd.Parameters.AddWithValue("p_nombreusuario", NpgsqlDbType.Varchar, nombreUsuario);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar usuario: {ex.Message}");
            }
            finally
            {
                conexion.CloseConnection();
            }
        }
    }
}

