using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Concepto
    {
        public int IdConcepto { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
