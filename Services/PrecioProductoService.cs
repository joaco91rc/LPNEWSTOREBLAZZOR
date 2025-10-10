using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PrecioProductoService
    {
        private readonly DL_PrecioProducto _dl;

        public PrecioProductoService(DL_PrecioProducto dl)
        {
            _dl = dl;
        }

        public Task<List<PrecioProducto>> ListarPreciosProductoAsync()
            => _dl.ListarPreciosProductoAsync();

        public Task<PrecioProducto?> ObtenerPreciosPorProductoYMonedaAsync(int idProducto, int idMoneda)
            => _dl.ObtenerPreciosPorProductoYMonedaAsync(idProducto, idMoneda);

        public Task<(int idGenerado, string mensaje)> RegistrarPrecioProductoAsync(PrecioProducto obj )
        {
            if (obj.FechaRegistro == default)
                obj.FechaRegistro = DateTime.Now;
            return _dl.RegistrarPrecioProductoAsync(obj);
        }

        public Task<(bool exito, string mensaje)> EditarPrecioProductoAsync(PrecioProducto obj )
            => _dl.EditarPrecioProductoAsync(obj);

        public Task<(bool exito, string mensaje)> EliminarPrecioProductoAsync(int idPrecioProducto)
            => _dl.EliminarPrecioProductoAsync(idPrecioProducto);

        
        public async Task<(bool exito, string mensaje, int id)> GuardarPrecioProductoAsync(PrecioProducto obj)
        {
            if (obj.IdPrecioProducto > 0)
            {
                var (ok, msg) = await _dl.EditarPrecioProductoAsync(obj);
                return (ok, msg, obj.IdPrecioProducto);
            }
            else
            {
                if (obj.FechaRegistro == default)
                    obj.FechaRegistro = DateTime.Now;

                var (id, msg) = await _dl.RegistrarPrecioProductoAsync(obj);
                return (id > 0, msg, id);
            }
        }
    }
}
