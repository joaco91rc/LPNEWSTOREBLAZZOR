using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdNegocio { get; set; }
        public Usuario oUsuario { get; set; }
        public Proveedor oProveedor { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public decimal montoTotal { get; set; }
        public decimal cotizacionDolar { get; set; }
        public List<DetalleCompra> ODetalleCompra { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Observaciones { get; set; }
        // Nuevos campos
        public string FormaPago { get; set; }
        public string FormaPago2 { get; set; }
        public string FormaPago3 { get; set; }
        public string FormaPago4 { get; set; }
        public decimal MontoFP1 { get; set; }
        public decimal MontoFP2 { get; set; }
        public decimal MontoFP3 { get; set; }
        public decimal MontoFP4 { get; set; }
        public decimal MontoPago { get; set; }
        public decimal MontoPagoFP2 { get; set; }
        public decimal MontoPagoFP3 { get; set; }
        public decimal MontoPagoFP4 { get; set; }



    }
}
