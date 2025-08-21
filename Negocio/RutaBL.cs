using System;
using System.Collections.Generic;
using ControlAutobuses.Datos;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Negocio
{
    public class RutaBL
    {
        private readonly RutaRepository rutaRepository;

        public RutaBL()
        {
            rutaRepository = new RutaRepository();
        }

        public bool CrearRuta(Ruta ruta)
        {
            // Validaciones de negocio
            if (string.IsNullOrEmpty(ruta.Nombre))
                throw new Exception("El nombre de la ruta es requerido");

            if (ruta.Nombre.Length < 3)
                throw new Exception("El nombre de la ruta debe tener al menos 3 caracteres");

            // Verificar si ya existe una ruta con el mismo nombre
            if (rutaRepository.ExisteRuta(ruta.Nombre))
                throw new Exception("Ya existe una ruta con ese nombre");

            return rutaRepository.Crear(ruta);
        }

        public List<Ruta> ObtenerRutas()
        {
            return rutaRepository.ObtenerTodos();
        }

        public List<Ruta> ObtenerRutasDisponibles()
        {
            return rutaRepository.ObtenerDisponibles();
        }

        public bool ActualizarRuta(Ruta ruta)
        {
            // Validaciones similares a CrearRuta
            if (string.IsNullOrEmpty(ruta.Nombre))
                throw new Exception("El nombre de la ruta es requerido");

            if (ruta.Nombre.Length < 3)
                throw new Exception("El nombre de la ruta debe tener al menos 3 caracteres");

            return rutaRepository.Actualizar(ruta);
        }

        public bool EliminarRuta(int id)
        {
            // Verificar si la ruta tiene asignaciones activas
            if (rutaRepository.TieneAsignacionesActivas(id))
                throw new Exception("No se puede eliminar la ruta porque tiene asignaciones activas");

            return rutaRepository.Eliminar(id);
        }

        public Ruta ObtenerRutaPorId(int id)
        {
            return rutaRepository.ObtenerPorId(id);
        }

        public Ruta ObtenerRutaPorNombre(string nombre)
        {
            return rutaRepository.ObtenerPorNombre(nombre);
        }
    }
}
