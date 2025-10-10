using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class HistorialProductoSTService
    {
        private readonly DL_HistorialProductoST _historialProductoST;

        public HistorialProductoSTService(DL_HistorialProductoST historialProductoST)
        {
            _historialProductoST = historialProductoST;
        }


        public async Task<(int IdGenerado, string Mensaje)> InsertarHistorialProductoAsync(HistorialProductoServTec objHistorialProducto)
        {
            return await _historialProductoST.InsertarHistorialProductoAsync(objHistorialProducto);
        }

        public async Task<(bool Resultado, string Mensaje)> ModificarHistorialProductoAsync(HistorialProductoServTec objHistorialProducto)
        {
            return await _historialProductoST.ModificarHistorialProductoAsync(objHistorialProducto);
        }

        public async Task<(bool Resultado, string Mensaje)> EliminarHistorialProductoAsync(int idHistorialProducto)
        {
            return await _historialProductoST.EliminarHistorialProductoAsync(idHistorialProducto);
        }

        public async Task<List<HistorialProductoServTec>> ListarHistorialProductosAsync(int idHistorialServicio)
        {
            return await _historialProductoST.ListarHistorialProductosAsync(idHistorialServicio);
        }
    }
}
