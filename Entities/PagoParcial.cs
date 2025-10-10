using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PagoParcial
    {
        public int IdPagoParcial { get; set; }
        public int IdCliente { get; set; }
        public int IdNegocio { get; set; }
        public string NombreCliente { get; set; } // Nuevo campo para el nombre del cliente
        public decimal Monto { get; set; }
        public int? IdVenta { get; set; } // Es opcional porque no siempre puede estar asociado a una venta
        public string NumeroVenta { get; set; } // Nuevo campo para el número de venta (opcional)
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }
        public string Vendedor { get; set; }
        public string NombreLocal { get; set; }
        public string ProductoReservado { get; set; }
        public string FormaPago { get; set; }
        public string Moneda { get; set; }
    }
}
