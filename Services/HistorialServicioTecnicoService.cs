using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class HistorialServicioTecnicoService
    {
        private readonly DL_HistorialServicioTecnico _historialServicioTecnicoDL;

        public HistorialServicioTecnicoService(DL_HistorialServicioTecnico historialServicioTecnicoDL)
        {
            _historialServicioTecnicoDL = historialServicioTecnicoDL;
        }

        public async Task<List<HistorialServicioTecnico>> ListarHistorialServicioAsync(int idServicio)
        {
            return await _historialServicioTecnicoDL.ListarHistorialServicioAsync(idServicio);
        }

        public async Task<(int IdHistorialGenerado, string Mensaje)> RegistrarHistorialServicioAsync(HistorialServicioTecnico objHistorial)
        {
            return await _historialServicioTecnicoDL.RegistrarHistorialServicioAsync(objHistorial);
        }

        public async Task<(bool Resultado, string Mensaje)> ModificarHistorialServicioAsync(HistorialServicioTecnico objHistorial)
        {
            return await _historialServicioTecnicoDL.ModificarHistorialServicioAsync(objHistorial);
        }

        public async Task<(bool Resultado, string Mensaje)> EliminarHistorialServicioAsync(int idHistorial)
        {
            return await _historialServicioTecnicoDL.EliminarHistorialServicioAsync(idHistorial);
        }
    }
}
