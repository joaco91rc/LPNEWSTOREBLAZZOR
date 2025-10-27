using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductoNegocioService
    {
        private readonly DL_ProductoNegocio _productoNegocioDL;

        public ProductoNegocioService(DL_ProductoNegocio productoNegocioDL)
        {
            _productoNegocioDL = productoNegocioDL;
        }

        public Task<int> ObtenerStockProductoEnSucursalAsync(int idProducto, int idNegocio)
            => _productoNegocioDL.ObtenerStockProductoEnSucursalAsync(idProducto, idNegocio);

        public Task<string> CargarOActualizarStockProductoAsync(int idProducto, int idNegocio, int stock)
            => _productoNegocioDL.CargarOActualizarStockProductoAsync(idProducto, idNegocio, stock);

        public Task SobrescribirStockAsync(int idProducto, int idNegocio, int stock)
            => _productoNegocioDL.SobrescribirStockAsync(idProducto, idNegocio, stock);

        public Task EliminarProductoNegocioAsync(int idProducto, int idNegocio)
            => _productoNegocioDL.EliminarProductoNegocioAsync(idProducto, idNegocio);


    }
}
