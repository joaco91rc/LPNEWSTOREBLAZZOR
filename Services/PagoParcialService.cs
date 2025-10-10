using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class PagoParcialService
    {
        private readonly DL_PagoParcial _pagoParcialDL;

        public PagoParcialService(DL_PagoParcial pagoParcialDL)
        {
            _pagoParcialDL = pagoParcialDL;
        }

        public Task<List<PagoParcial>> Listar()
            => _pagoParcialDL.ListarAsync();

        public Task<List<PagoParcial>> ListarPorLocal(int idNegocio)
            => _pagoParcialDL.ListarPagosParcialesPorLocalAsync(idNegocio);

        public Task<List<PagoParcial>> ListarActivos(int idNegocio)
            => _pagoParcialDL.ListarPagosParcialesActivosAsync(idNegocio);

        public Task<List<PagoParcial>> ListarInactivos(int idNegocio)
            => _pagoParcialDL.ListarPagosParcialesInactivosAsync(idNegocio);

        public Task<bool> DarDeBaja(int idPagoParcial, int idVenta)
            => _pagoParcialDL.DarDeBajaPagoParcialAsync(idPagoParcial, idVenta);

        public Task<(int idGenerado, string mensaje)> Registrar(PagoParcial obj)
            => _pagoParcialDL.RegistrarPagoParcialAsync(obj);

        public Task<(bool ok, string mensaje)> Modificar(PagoParcial obj)
            => _pagoParcialDL.ModificarPagoParcialAsync(obj);

        public Task<(bool ok, string mensaje)> Eliminar(int idPagoParcial)
            => _pagoParcialDL.EliminarPagoParcialAsync(idPagoParcial);

        public Task<List<PagoParcial>> ConsultarPorCliente(int idCliente)
            => _pagoParcialDL.ConsultarPagosParcialesPorClienteAsync(idCliente);

    }
}
