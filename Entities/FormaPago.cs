using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class FormaPago
    {
        public enum TipoFormaPago
        {
            CREDITO,
            DEBITO,
            DOLAR,
            EFECTIVO,
            TRANSFERENCIA
        }

        public enum CajaAsociadaFP
        {
            GALICIA,
            EFECTIVO,
            MERCADOPAGO,
            DOLARES
        }
        public int IdFormaPago { get; set; }
        public string Descripcion { get; set; }
        public decimal PorcentajeRetencion { get; set; }
        public decimal PorcentajeRecargo { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal PorcentajeRecargoDolar { get; set; }
        public decimal PorcentajeDescuentoDolar { get; set; }
        public CajaAsociadaFP CajaAsociada { get; set; }
        public TipoFormaPago Tipo { get; set; }

    }
}
