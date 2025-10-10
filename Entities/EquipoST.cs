using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EquipoST
    {
        public int IdEquipoST { get; set; }
        public int IdNegocio { get; set; }
        public string Nombre { get; set; }
        public string TipoEquipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string SerialNumber { get; set; }
        public int IdCliente { get; set; }
    }
}
