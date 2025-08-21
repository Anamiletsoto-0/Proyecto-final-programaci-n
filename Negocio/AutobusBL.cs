using System;
using System.Collections.Generic;
using ControlAutobuses.Datos;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Negocio
{
    public class AutobusBL
    {
        private readonly AutobusRepository autobusRepository;

        public AutobusBL()
        {
            autobusRepository = new AutobusRepository();
        }

        public bool CrearAutobus(Autobus autobus)
        {
            // Validaciones de negocio
            if (string.IsNullOrEmpty(autobus.Marca))
                throw new Exception("La marca es requerida");

            if (string.IsNullOrEmpty(autobus.Modelo))
                throw new Exception("El modelo es requerido");

            if (string.IsNullOrEmpty(autobus.Placa))
                throw new Exception("La placa es requerida");

            if (string.IsNullOrEmpty(autobus.Color))
                throw new Exception("El color es requerido");

            // Validar año (no puede ser mayor al actual + 1)
            if (autobus.Anio < 1990 || autobus.Anio > DateTime.Now.Year + 1)
                throw new Exception("El año del autobús no es válido");

            // Validar formato de placa (ejemplo básico)
            if (autobus.Placa.Length < 6)
                throw new Exception("La placa debe tener un formato válido");

            return autobusRepository.Crear(autobus);
        }

        public List<Autobus> ObtenerAutobuses()
        {
            return autobusRepository.ObtenerTodos();
        }

        public List<Autobus> ObtenerAutobusesDisponibles()
        {
            return autobusRepository.ObtenerDisponibles();
        }

        public bool ActualizarAutobus(Autobus autobus)
        {
            // Validaciones similares a CrearAutobus
            return autobusRepository.Actualizar(autobus);
        }

        public bool EliminarAutobus(int id)
        {
            // Verificar si el autobús tiene asignaciones activas
            if (autobusRepository.TieneAsignacionesActivas(id))
                throw new Exception("No se puede eliminar el autobús porque tiene asignaciones activas");

            return autobusRepository.Eliminar(id);
        }

        public Autobus ObtenerAutobusPorId(int id)
        {
            return autobusRepository.ObtenerPorId(id);
        }
    }
}
