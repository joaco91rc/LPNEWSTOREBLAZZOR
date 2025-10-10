using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class TransaccionCaja
    {
        public int IdTransaccion { get; set; }
        public int? IdVenta { get; set; }
        public int? IdCompra { get; set; }
        public int? IdPagoParcial { get; set; }
        public int? IdNegocio { get; set; }
        public int IdCajaRegistradora { get; set; }
        public string Hora { get; set; }
        public string TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public string DocAsociado { get; set; }
        public string UsuarioTransaccion { get; set; }
        public string FormaPago { get; set; }

        public string CajaAsociada { get; set; }
        public string Concepto { get; set; }


    }
}
