using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrdenTraspaso
    {
        public int IdOrdenTraspaso { get; set; }
        public int IdSucursalOrigen { get; set; }
        public int IdSucursalDestino { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string Confirmada { get; set; }
        public string SerialNumber { get; set; }
        public string LocalOrigen { get; set; }
        public string LocalDestino { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal CostoProducto { get; set; }

        public DateTime? FechaConfirmacion { get; set; }



    }
}
