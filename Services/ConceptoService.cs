using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ConceptoService
    {
        private readonly DL_Concepto _conceptoDL;

        public ConceptoService(DL_Concepto conceptoDL)
        {
            _conceptoDL = conceptoDL;
        }
        public async Task<List<Concepto>> ListarAsync(int idNegocio)
        {
            return await _conceptoDL.ListarAsync();
        }

        public async Task<(int idGenerado, string mensaje)> RegistrarAsync(Concepto objConcepto)
        {
            return await _conceptoDL.RegistrarAsync(objConcepto);
        }

        public async Task<(bool resultado, string mensaje)> EditarAsync(Concepto objConcepto)
        {
            return await _conceptoDL.EditarAsync(objConcepto);
        }

        public async Task<(bool resultado, string mensaje)> EliminarAsync(Concepto objConcepto)
        {
            return await _conceptoDL.EliminarAsync(objConcepto);
        }
    }
}
