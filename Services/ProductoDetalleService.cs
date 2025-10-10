using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    
        public class ProductoDetalleService
        {
            private readonly DL_ProductoDetalle _productoDetalleDL;

            public ProductoDetalleService(DL_ProductoDetalle productoDetalleDL)
            {
                _productoDetalleDL = productoDetalleDL;
            }

            public Task<List<ProductoDetalle>> ListarProductosConSerialNumberByIDNegocioAsync(int idProducto, int idNegocio)
                => _productoDetalleDL.ListarProductosConSerialNumberByIDNegocioAsync(idProducto, idNegocio);

            public Task<List<ProductoDetalle>> ListarProductosConSerialNumberByIDAsync(int idProducto)
                => _productoDetalleDL.ListarProductosConSerialNumberByIDAsync(idProducto);

            public Task<List<ProductoDetalle>> ListarProductosConSerialNumberEnStockTodosLocalesAsync()
                => _productoDetalleDL.ListarProductosConSerialNumberEnStockTodosLocalesAsync();

            public Task<List<ProductoDetalle>> ListarProductosConSerialNumberAsync()
                => _productoDetalleDL.ListarProductosConSerialNumberAsync();

            public Task<List<ProductoDetalle>> ListarProductosConSerialNumberPorLocalDisponiblesAsync(int idNegocio)
                => _productoDetalleDL.ListarProductosConSerialNumberPorLocalDisponiblesAsync(idNegocio);

            public Task<List<ProductoDetalle>> ListarProductosVendidosAsync(int idNegocio)
                => _productoDetalleDL.ListarProductosVendidosAsync(idNegocio);

            public Task<List<ProductoDetalle>> ListarProductosVendidosPorFechaAsync(int idNegocio, DateTime fechaInicio, DateTime fechaFin)
                => _productoDetalleDL.ListarProductosVendidosPorFechaAsync(idNegocio, fechaInicio, fechaFin);

            public Task<List<ProductoDetalle>> ListarProductosVendidosTodosLocalesAsync()
                => _productoDetalleDL.ListarProductosVendidosTodosLocalesAsync();

            public Task<List<ProductoDetalle>> ListarProductosSerialesPorVentaAsync(int idVenta)
                => _productoDetalleDL.ListarProductosSerialesPorVentaAsync(idVenta);

            public Task<int> ContarProductosSerializadosAsync(int idProducto, int idNegocio)
                => _productoDetalleDL.ContarProductosSerializadosAsync(idProducto, idNegocio);

            public Task<(int resultado, string mensaje)> RegistrarSerialNumberAsync(ProductoDetalle productoDetalle)
                => _productoDetalleDL.RegistrarSerialNumberAsync(productoDetalle);

            public Task<(int resultado, string mensaje)> DesactivarProductoDetalleAsync(int idProductoDetalle, int idVenta)
                => _productoDetalleDL.DesactivarProductoDetalleAsync(idProductoDetalle, idVenta);

            public Task<(int resultado, string mensaje)> ActivarProductoDetalleAsync(int idProductoDetalle)
                => _productoDetalleDL.ActivarProductoDetalleAsync(idProductoDetalle);

            public Task<(bool respuesta, string mensaje)> EditarSerialNumberAsync(ProductoDetalle productoDetalle)
                => _productoDetalleDL.EditarSerialNumberAsync(productoDetalle);

            public Task<(bool respuesta, string mensaje)> TraspasarSerialNumberAsync(ProductoDetalle productoDetalle)
                => _productoDetalleDL.TraspasarSerialNumberAsync(productoDetalle);

            public Task<(bool respuesta, string mensaje)> ActualizarSerialNumberTrasasadoAsync(ProductoDetalle productoDetalle)
                => _productoDetalleDL.ActualizarSerialNumberTrasasadoAsync(productoDetalle);

            public Task<(bool respuesta, string mensaje)> EliminarSerialNumberAsync(ProductoDetalle productoDetalle)
                => _productoDetalleDL.EliminarSerialNumberAsync(productoDetalle);
        }
    }

