using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ReporteVentasPorProducto
    {
        public string NombreLocal { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadVendida { get; set; }
        public decimal TotalFacturadoPesos { get; set; }
        public decimal TotalFacturadoDolares { get; set; }
        public string Observaciones { get; set; }

    }
}
