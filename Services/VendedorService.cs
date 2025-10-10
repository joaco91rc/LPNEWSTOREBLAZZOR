using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class VendedorService
    {
        private readonly DL_Vendedor _vendedorDL;

        public VendedorService(DL_Vendedor vendedorDL)
        {
            _vendedorDL = vendedorDL;
        }

        public Task<List<Vendedor>> ListarVendedores()
            => _vendedorDL.ListarVendedores();

        public Task<(int idGenerado, string mensaje)> RegistrarVendedor(Vendedor vendedor)
            => _vendedorDL.RegistrarVendedor(vendedor);

        public Task<(bool resultado, string mensaje)> EditarVendedor(Vendedor vendedor)
            => _vendedorDL.EditarVendedor(vendedor);

        public Task<(bool resultado, string mensaje)> EliminarVendedor(Vendedor vendedor)
            => _vendedorDL.EliminarVendedor(vendedor);
    }
}
