using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DetalleVenta
    {

        public int IdDetalleVenta { get; set; }
        public Producto OProducto { get; set; } = new Producto();
        public decimal PrecioVenta { get; set; }
        public decimal PrecioCompra { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public string FechaRegistro { get; set; }
    }
}
