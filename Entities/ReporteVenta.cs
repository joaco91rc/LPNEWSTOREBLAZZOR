using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ReporteVenta
    {

        public string FechaRegistro { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string MontoTotal { get; set; }
        public string CotizacionDolar { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }

        public string NombreProducto { get; set; }
        public string CostoTotalProductos { get; set; }

        public string Observaciones { get; set; }
        public string MargenGananciaEnDolares { get; set; }
        public string PorcentajeMargenGanancia { get; set; }
        public string Vendedor { get; set; }
        public string NombreLocal { get; set; }

    }
}
