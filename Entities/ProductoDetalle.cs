using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ProductoDetalle
    {
        public DateTime Fecha { get; set; }
        public DateTime? FechaEgreso { get; set; }
        public int IdProductoDetalle { get; set; }
        public int IdProducto { get; set; }
        public string NumeroSerie { get; set; }
        public string Color { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int IdNegocio { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public int? IdVenta { get; set; }
        public string NumeroVenta { get; set; }
        public string EstadoVendido { get; set; }
        public string NombreLocal { get; set; }
    }
}
