using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class HistorialServicioTecnico
    {
        public int IdHistorial { get; set; }
        public int IdServicio { get; set; }
        public string DescripcionAccion { get; set; }
        public DateTime FechaAccion { get; set; }
        public string Responsable { get; set; }
        public decimal? CostoManoDeObra { get; set; } // Puede ser nulo
        public decimal? CostoRepuestos { get; set; } // Puede ser nulo
        public decimal? HorasTrabajo { get; set; } // Puede ser nulo
        public string EstadoReparacion { get; set; }

    }
}
