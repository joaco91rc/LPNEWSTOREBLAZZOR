using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrdenTrabajo
    {
        public int IdOrdenTrabajo { get; set; }
        public int IdServicio { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Detalle { get; set; }
        public string Estado { get; set; }

        public ServicioTecnico ServicioTecnico { get; set; }
    }
}
