using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductoService
    {
        private readonly DL_Producto _productoDL;
        public ProductoService(DL_Producto productoDL)
        {
            _productoDL = productoDL;
        }

        public Task<decimal> ObtenerCostoProducto(int idProducto)
           => _productoDL.ObtenerCostoProducto(idProducto);

        public Task<(bool, string)> Eliminar(Producto producto)
            => _productoDL.Eliminar(producto);

        public Task<(bool, string)> DarBajaLogica(int idProducto)
            => _productoDL.DarBajaLogica(idProducto);

        public Task<bool> ActualizarProductoDolar(int idProducto, bool productoDolar)
            => _productoDL.ActualizarProductoDolar(idProducto, productoDolar);

        public Task<(bool, string)> Editar(Producto producto)
            => _productoDL.Editar(producto);

        public Task<Producto> ObtenerProductoPorId(int idProducto)
            => _productoDL.ObtenerProductoPorId(idProducto);

        public Task<(int, string)> Registrar(Producto producto)
            => _productoDL.Registrar(producto);

        public Task<(bool Exito, string Mensaje)> SumarStockPorRMAAsync(int idProducto, int cantidad, int idNegocio)
            => _productoDL.SumarStockPorRMAAsync(idProducto, cantidad, idNegocio);

        public Task<(bool Exito, string Mensaje)> RestarStockPorRMAAsync(int idProducto, int cantidad, int idNegocio)
            => _productoDL.RestarStockPorRMAAsync(idProducto, cantidad, idNegocio);

        public Task<List<Producto>> ListarPorNegocioAsync(int idNegocio)
            => _productoDL.ListarPorNegocioAsync(idNegocio);

        public Task<List<Producto>> ListarAsync()
            => _productoDL.ListarAsync();

        public Task<List<Producto>> ListarSerializablesPorNegocioAsync(int idNegocio)
            => _productoDL.ListarSerializablesPorNegocioAsync(idNegocio);

        public Task<List<Producto>> ListarSerializablesAsync(int idLocal)
            => _productoDL.ListarSerializablesAsync(idLocal);

        public Task<List<Producto>> ListarAsync(int idNegocio)
            => _productoDL.ListarAsync(idNegocio);

        public Task<List<Producto>> ListarProductosEnStockAsync(int idNegocio)
            => _productoDL.ListarProductosEnStockAsync(idNegocio);

        public Task<string> ObtenerNombreProductoAsync(int idProducto)
            => _productoDL.ObtenerNombreProductoAsync(idProducto);
    }
}
