using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MonedaService
    {
        private readonly DL_Moneda _monedaDL;

        public MonedaService(DL_Moneda monedaDL)
        {
            _monedaDL = monedaDL;
        }

        public Task<List<Moneda>> ListarMonedas()
        => _monedaDL.ListarMonedas();

        public Task<Moneda?> ObtenerMonedaPorId(int idMoneda)
            => _monedaDL.ObtenerMonedaPorId(idMoneda);

        public Task<(int idGenerado, string mensaje)> RegistrarMoneda(Moneda moneda)
            => _monedaDL.RegistrarMoneda(moneda);

        public Task<(bool resultado, string mensaje)> EditarMoneda(Moneda moneda)
            => _monedaDL.EditarMoneda(moneda);

        public Task<(bool resultado, string mensaje)> EliminarMoneda(int idMoneda)
            => _monedaDL.EliminarMoneda(idMoneda);
    }

}
