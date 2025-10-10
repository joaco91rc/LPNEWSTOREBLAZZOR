using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Entities;

namespace Services
{
    public class ProveedorService
    {

        private readonly DL_Proveedor _ProveedorDL;
        

        public ProveedorService(DL_Proveedor ProveedorDL)
        {
            _ProveedorDL = ProveedorDL;
            
        }

        public Task<List<Proveedor>> Listar() => _ProveedorDL.Listar();

        public Task<(int, string)> Registrar(Proveedor proveedor) => _ProveedorDL.Registrar(proveedor);

        public Task<(bool, string)> Editar(Proveedor proveedor) => _ProveedorDL.Editar(proveedor);

        public Task<(bool, string)> Eliminar(int idProveedor) => _ProveedorDL.Eliminar(idProveedor);

    }
}
