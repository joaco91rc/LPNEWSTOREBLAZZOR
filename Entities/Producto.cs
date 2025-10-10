using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Categoria OCategoria { get; set; }
        public DateTime? FechaUltimaVenta { get; set; }
        public int? DiasSinVenta { get; set; }
        public decimal CostoPesos { get; set; }

        public decimal VentaPesos { get; set; }

        public decimal PrecioCompra { get; set; }
        public decimal PrecioLista { get; set; }
        public decimal PrecioVenta { get; set; }



        public bool Estado { get; set; }
        public bool ProdSerializable { get; set; }
        public bool ProductoDolar { get; set; }
        public string FechaRegistro { get; set; }

        public int Stock { get; set; }
        public string NombreLocal { get; set; }
        public int StockH1 { get; set; }
        public int StockH2 { get; set; }
        public int StockAS { get; set; }
        public int StockAC { get; set; }
        public int StockTotal
        {
            get
            {
                return StockH1 + StockH2 + StockAS + StockAC;
            }
        }


    }
}
