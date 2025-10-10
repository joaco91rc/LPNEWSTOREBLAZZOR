using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FormaPagoService
    {
        private readonly DL_FormaPago _formaPagoDL;

        public FormaPagoService(DL_FormaPago formaPagoDL)
        {
            _formaPagoDL = formaPagoDL;
        }

        public Task<List<FormaPago>> ListarFormasDePago()
            => _formaPagoDL.ListarFormasDePago();

        public Task<FormaPago?> ObtenerFPPorDescripcion(string descripcion)
            => _formaPagoDL.ObtenerFPPorDescripcion(descripcion);

        public Task<(int idGenerado, string mensaje)> RegistrarFormaPago(FormaPago formaPago)
            => _formaPagoDL.RegistrarFormaPago(formaPago);

        public Task<(bool resultado, string mensaje)> EditarFormaPago(FormaPago formaPago)
            => _formaPagoDL.EditarFormaPago(formaPago);

        public Task<(bool resultado, string mensaje)> Eliminar(int idFormaPago)
            => _formaPagoDL.Eliminar(idFormaPago);
    }

}
