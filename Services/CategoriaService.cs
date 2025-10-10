using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoriaService
    {

        private readonly DL_Categoria _categoriaDL;

        public CategoriaService(DL_Categoria categoriaDL)
        {
            _categoriaDL = categoriaDL;
        }

        public Task<List<Categoria>> ListarCategorias()
            => _categoriaDL.Listar();

        public Task<(int idGenerado, string mensaje)> RegistrarCategoria(Categoria categoria)
            => _categoriaDL.Registrar(categoria);

        public Task<(bool resultado, string mensaje)> EditarCategoria(Categoria categoria)
            => _categoriaDL.Editar(categoria);

        public Task<(bool resultado, string mensaje)> EliminarCategoria(Categoria categoria)
            => _categoriaDL.Eliminar(categoria);

    }
}
