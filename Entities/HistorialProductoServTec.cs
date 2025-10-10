using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class HistorialProductoServTec
    {

        public int IdHistorialProducto { get; set; }
        public int IdHistorialServicio { get; set; } // Referencia al historial de servicio
        public int? IdProducto { get; set; } // idProducto utilizado
        public string NombreProducto { get; set; } // idProducto utilizado
        public string CodigoProducto { get; set; } // idProducto utilizado
        public decimal Cantidad { get; set; } // Cantidad de productos utilizados
        public decimal PrecioVenta { get; set; } // Precio de venta del producto utilizado
    }
}
