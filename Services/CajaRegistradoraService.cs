using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CajaRegistradoraService
    {
        private readonly DL_CajaRegistradora _cajaRegistradoraDL;

        public CajaRegistradoraService(DL_CajaRegistradora cajaRegistradora)
        {
            _cajaRegistradoraDL = cajaRegistradora;
        }
        public async Task<(int idCajaGenerado, string mensaje)> AperturaCajaAsync(CajaRegistradora objCajaRegistradora, int idNegocio)
        {
            return await _cajaRegistradoraDL.AperturaCajaAsync(objCajaRegistradora, idNegocio);
        }

        public async Task<CajaRegistradora> ObtenerCajaPorFechaAsync(string fecha, int idNegocio)
        {
            return await _cajaRegistradoraDL.ObtenerCajaPorFechaAsync(fecha, idNegocio);
        }

        public async Task<List<CajaRegistradora>> ListarAsync(int idNegocio)
        {
            return await _cajaRegistradoraDL.ListarAsync(idNegocio);
        }

        public async Task<CajaRegistradora> ObtenerUltimaCajaCerradaAsync(int idNegocio)
        {
            return await _cajaRegistradoraDL.ObtenerUltimaCajaCerradaAsync(idNegocio);
        }

        public async Task<(bool resultado, string mensaje)> CerrarCajaAsync(CajaRegistradora caja, int idNegocio)
        {
            return await _cajaRegistradoraDL.CerrarCajaAsync(caja, idNegocio);
        }
    }
}
