using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DetalleCajaService
    {
        private readonly DL_DetalleCaja _detalleCajaDL;

        public DetalleCajaService(DL_DetalleCaja detalleCajaDL)
        {
            _detalleCajaDL = detalleCajaDL;
        }

        public async Task<List<DetalleCaja>> DetalleCajaAsync(string fechaConsulta)
        {
            return await _detalleCajaDL.DetalleCajaAsync(fechaConsulta);
        }

        public async Task<List<DetalleCaja>> ListarAsync(string fecha, int idNegocio)
        {
            return await _detalleCajaDL.ListarAsync(fecha, idNegocio);
        }
    }
}
