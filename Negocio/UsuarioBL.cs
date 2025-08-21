using System;
using ControlAutobuses.Datos;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Negocio
{
    public class UsuarioBL
    {
        private readonly UsuarioRepository _repo;

        public UsuarioBL()
        {
            _repo = new UsuarioRepository();
        }

        public Usuario Autenticar(string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
                throw new Exception("El nombre de usuario es requerido");
            if (string.IsNullOrEmpty(contrasena))
                throw new Exception("La contraseña es requerida");
            return _repo.Autenticar(nombreUsuario, contrasena);
        }

        public bool CambiarContrasena(int usuarioId, string nuevaContrasena)
        {
            if (usuarioId <= 0)
                throw new Exception("ID de usuario inválido");
            if (string.IsNullOrEmpty(nuevaContrasena) || nuevaContrasena.Length < 6)
                throw new Exception("La nueva contraseña debe tener al menos 6 caracteres");
            return _repo.CambiarContrasena(usuarioId, nuevaContrasena);
        }

        public bool CrearUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.NombreUsuario) || string.IsNullOrEmpty(usuario.Contrasena))
                throw new Exception("Usuario y contraseña son requeridos");
            if (usuario.TipoUsuario != "Administrador" && usuario.TipoUsuario != "Usuario")
                throw new Exception("Tipo de usuario inválido");
            return _repo.CrearUsuario(usuario);
        }

        public bool ExisteUsuario(string nombreUsuario)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
                throw new Exception("Nombre de usuario requerido");
            return _repo.ExisteUsuario(nombreUsuario);
        }
    }
}
