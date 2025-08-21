using System;

namespace ControlAutobuses.Entidades
{
    public class Asignacion
    {
        public int Id { get; set; }
        public int ChoferId { get; set; }
        public int AutobusId { get; set; }
        public int RutaId { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Activa { get; set; }

        // Propiedades de navegación (opcionales, para mostrar datos relacionados)
        public string NombreChofer { get; set; }
        public string PlacaAutobus { get; set; }
        public string NombreRuta { get; set; }
    }
}
