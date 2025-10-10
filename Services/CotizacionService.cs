using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CotizacionService
    {
        private readonly DL_Cotizacion _cotizacionDL;

        public CotizacionService(DL_Cotizacion cotizacionDL)
        {
            _cotizacionDL = cotizacionDL;
        }

        public Task<Cotizacion> CotizacionActiva()
            => _cotizacionDL.CotizacionActiva();

        public Task<List<Cotizacion>> HistoricoCotizaciones()
            => _cotizacionDL.HistoricoCotizaciones();

        public async Task<(int, string)> Registrar(Cotizacion cotizacion)
        {
            string mensaje = string.Empty;
            int resultado = await _cotizacionDL.Registrar(cotizacion, msg => mensaje = msg);
            return (resultado, mensaje);
        }

        public async Task<(bool, string)> Editar(Cotizacion cotizacion)
        {
            string mensaje = string.Empty;
            bool resultado = await _cotizacionDL.Editar(cotizacion, msg => mensaje = msg);
            return (resultado, mensaje);
        }
    }

}
