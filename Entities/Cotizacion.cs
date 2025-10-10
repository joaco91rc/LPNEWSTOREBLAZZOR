using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Cotizacion
    {

        public int IdCotizacion { get; set; }

        public decimal Importe { get; set; }

        public DateTime? Fecha { get; set; } = DateTime.Now;
        public bool Estado { get; set; }





    }
}
