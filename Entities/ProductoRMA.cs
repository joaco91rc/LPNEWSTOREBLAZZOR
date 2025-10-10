using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ProductoRMA
    {
        public int IdProductoRMA { get; set; }
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public string DescripcionProductoRMA { get; set; }
        public int IdProducto { get; set; }

        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaEgreso { get; set; }



    }
}
