using System;
using System.Collections.Generic;
using ControlAutobuses.Datos;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Negocio
{
    public class AsignacionBL
    {
        private readonly AsignacionRepository repo;

        public AsignacionBL()
        {
            repo = new AsignacionRepository();
        }

        public bool CrearAsignacion(Asignacion a)
        {
            if (a.ChoferId <= 0 || a.AutobusId <= 0 || a.RutaId <= 0)
                throw new Exception("Todos los campos son obligatorios.");
            return repo.Crear(a);
        }

        public List<Asignacion> ObtenerAsignacionesActivas()
        {
            return repo.ObtenerActivas();
        }

        public bool FinalizarAsignacion(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido");
            return repo.Finalizar(id);
        }
    }
}