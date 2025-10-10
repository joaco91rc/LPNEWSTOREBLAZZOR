using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class OrdenTraspasoService
    {
        private readonly DL_OrdenTraspaso _ordenTraspasoDL;

        public OrdenTraspasoService(DL_OrdenTraspaso ordenTraspasoDL)
        {
            _ordenTraspasoDL = ordenTraspasoDL;
        }
        public Task<List<OrdenTraspaso>> ListarAsync()
        {
            return _ordenTraspasoDL.ListarAsync();
        }

        public Task<OrdenTraspaso?> ObtenerPorIdAsync(int id)
        {
            return _ordenTraspasoDL.ObtenerPorIdAsync(id);
        }

        public Task InsertarAsync(OrdenTraspaso orden)
        {
            // acá podés agregar validaciones de negocio si querés
            return _ordenTraspasoDL.InsertarAsync(orden);
        }

        public Task ConfirmarAsync(int id)
        {
            return _ordenTraspasoDL.ConfirmarAsync(id);
        }

        public Task RechazarAsync(int id)
        {
            return _ordenTraspasoDL.RechazarAsync(id);
        }
    }
}
