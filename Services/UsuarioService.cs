using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsuarioService
    {
        private readonly MigracionUsuarios _migracionUsuarios;
        private readonly DL_Usuario _usuarioDL;

        public UsuarioService(MigracionUsuarios migracionUsuarios, DL_Usuario usuarioDL)
        {
            _migracionUsuarios = migracionUsuarios;
            _usuarioDL = usuarioDL;
        }



        public Task<int> MigrarUsuariosAsync()
        {
            return _migracionUsuarios.MigrarUsuariosAsync();
        }

        public async Task<List<Usuario>> Listar()
        {
            return await _usuarioDL.Listar();
        }

        public async Task<int> Registrar(Usuario usuario, int idRol)
        {
            return await _usuarioDL.Registrar(usuario, idRol);
        }

        public async Task<bool> Modificar(Usuario usuario, int idRol)
        {
            return await _usuarioDL.Modificar(usuario, idRol);
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            return await _usuarioDL.Eliminar(idUsuario);
        }

    }

}
