using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlAutobuses.Entidades
{
    public class Ruta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Disponible { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
    }
}