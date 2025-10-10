using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PrecioProducto
    {
        public int IdPrecioProducto { get; set; }
        public int IdProducto { get; set; }
        public int IdMoneda { get; set; }
        public string NombreProducto { get; set; }
        public string NombreMoneda { get; set; }
        public string SimboloMoneda { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioLista { get; set; }
        public decimal PrecioEfectivo { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
