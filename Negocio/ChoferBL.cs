using System;
using System.Collections.Generic;
using ControlAutobuses.Datos;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Negocio
{
    public class ChoferBL
    {
        private readonly ChoferRepository _choferRepository;

        public ChoferBL()
        {
            _choferRepository = new ChoferRepository();
        }

        public bool CrearChofer(Chofer chofer)
        {
            try
            {
                // Validaciones de negocio
                if (string.IsNullOrEmpty(chofer.Nombre) || string.IsNullOrEmpty(chofer.Apellido))
                    throw new Exception("Nombre y apellido son requeridos");

                if (chofer.FechaNacimiento == DateTime.MinValue)
                    throw new Exception("Fecha de nacimiento es requerida");

                if (string.IsNullOrEmpty(chofer.Cedula))
                    throw new Exception("Cédula es requerida");

                // Validar edad mínima (21 años)
                int edad = DateTime.Now.Year - chofer.FechaNacimiento.Year;
                if (DateTime.Now.DayOfYear < chofer.FechaNacimiento.DayOfYear)
                    edad--;

                if (edad < 21)
                    throw new Exception("El chofer debe tener al menos 21 años");

                // Validar formato de cédula (ejemplo básico)
                if (chofer.Cedula.Length < 11)
                    throw new Exception("La cédula debe tener un formato válido");

                return _choferRepository.Crear(chofer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear chofer: {ex.Message}");
            }
        }

        public List<Chofer> ObtenerChoferes()
        {
            try
            {
                return _choferRepository.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener choferes: {ex.Message}");
            }
        }

        public List<Chofer> ObtenerChoferesDisponibles()
        {
            try
            {
                return _choferRepository.ObtenerDisponibles();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener choferes disponibles: {ex.Message}");
            }
        }

        public bool ActualizarChofer(Chofer chofer)
        {
            try
            {
                // Validaciones similares a CrearChofer
                if (string.IsNullOrEmpty(chofer.Nombre) || string.IsNullOrEmpty(chofer.Apellido))
                    throw new Exception("Nombre y apellido son requeridos");

                if (chofer.FechaNacimiento == DateTime.MinValue)
                    throw new Exception("Fecha de nacimiento es requerida");

                if (string.IsNullOrEmpty(chofer.Cedula))
                    throw new Exception("Cédula es requerida");

                int edad = DateTime.Now.Year - chofer.FechaNacimiento.Year;
                if (DateTime.Now.DayOfYear < chofer.FechaNacimiento.DayOfYear)
                    edad--;

                if (edad < 21)
                    throw new Exception("El chofer debe tener al menos 21 años");

                if (chofer.Cedula.Length < 11)
                    throw new Exception("La cédula debe tener un formato válido");

                return _choferRepository.Actualizar(chofer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar chofer: {ex.Message}");
            }
        }

        public bool EliminarChofer(int id)
        {
            try
            {
                // Verificar si el chofer tiene asignaciones activas
                if (_choferRepository.TieneAsignacionesActivas(id))
                    throw new Exception("No se puede eliminar el chofer porque tiene asignaciones activas");

                return _choferRepository.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar chofer: {ex.Message}");
            }
        }

        public Chofer ObtenerChoferPorId(int id)
        {
            try
            {
                return _choferRepository.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener chofer por ID: {ex.Message}");
            }
        }

        public bool ChoferDisponible(int id)
        {
            try
            {
                return _choferRepository.EstaDisponible(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar disponibilidad del chofer: {ex.Message}");
            }
        }
    }
}