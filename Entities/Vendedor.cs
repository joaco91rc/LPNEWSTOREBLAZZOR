using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Vendedor
    {
        public int IdVendedor { get; set; }        // idVendedor
        public string Nombre { get; set; }          // nombre
        public string Apellido { get; set; }        // apellido
        public string DNI { get; set; }
        public decimal SueldoBase { get; set; }     // sueldoBase
        public decimal SueldoComision { get; set; } // sueldoComision
        public bool Estado { get; set; }
    }
}
