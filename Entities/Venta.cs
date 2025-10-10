using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdNegocio { get; set; }

        public Usuario OUsuario { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoPagoFP2 { get; set; }
        public decimal MontoPagoFP3 { get; set; }
        public decimal MontoPagoFP4 { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoTotal { get; set; }
        public List<DetalleVenta> ODetalleVenta { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string FormaPago { get; set; }
        public string FormaPago2 { get; set; }
        public string FormaPago3 { get; set; }
        public string FormaPago4 { get; set; }
        public decimal MontoFP1 { get; set; }
        public decimal MontoFP2 { get; set; }
        public decimal MontoFP3 { get; set; }
        public decimal MontoFP4 { get; set; }
        public decimal Descuento { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal CotizacionDolar { get; set; }
        public int IdVendedor { get; set; }
        public string NombreVendedor { get; set; }  // Nueva propiedad
        public string Observaciones { get; set; }
    }
}
