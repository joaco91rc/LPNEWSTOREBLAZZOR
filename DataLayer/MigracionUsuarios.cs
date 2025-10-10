using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MigracionUsuarios
    {
        private readonly string _cadenaConexion;
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public MigracionUsuarios(
            IConfiguration configuration,
            ApplicationDbContext context,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _cadenaConexion = configuration.GetConnectionString("cadena_conexion")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'cadena_conexion'.");

            _context = context;
            _passwordHasher = passwordHasher;
        }

        public List<UsuarioViejo> Listar()
        {
            List<UsuarioViejo> lista = new();
            using SqlConnection oconexion = new(_cadenaConexion);
            try
            {
                StringBuilder query = new();
                query.AppendLine("select u.idUsuario,u.documento,u.nombreCompleto,u.correo,u.clave,u.estado,r.idRol, r.descripcion from usuario u");
                query.AppendLine("inner join rol r on r.idRol = u.idRol");
                SqlCommand cmd = new(query.ToString(), oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new UsuarioViejo()
                    {
                        idUsuario = Convert.ToInt32(dr["idUsuario"]),
                        documento = dr["documento"].ToString(),
                        nombreCompleto = dr["nombreCompleto"].ToString(),
                        correo = dr["correo"].ToString(),
                        clave = dr["clave"].ToString(),
                        estado = Convert.ToBoolean(dr["estado"]),
                        oRol = new RolViejo()
                        {
                            idRol = Convert.ToInt32(dr["idRol"]),
                            descripcion = dr["descripcion"].ToString()
                        }
                    });
                }
            }
            catch (Exception)
            {
                lista = new();
            }

            return lista;
        }

        public async Task<int> MigrarUsuariosAsync()
        {
            var usuariosAntiguos = Listar();

            int migrados = 0;
            foreach (var oldUser in usuariosAntiguos)
            {
                var nuevoUsuario = new Usuario
                {
                    UserName = oldUser.documento,
                    NormalizedUserName = oldUser.documento.ToUpper(),
                    Email = oldUser.correo,
                    NormalizedEmail = oldUser.correo?.ToUpper(),
                    EmailConfirmed = true,
                    NombreCompleto = oldUser.nombreCompleto.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                nuevoUsuario.PasswordHash = _passwordHasher.HashPassword(nuevoUsuario, oldUser.clave);

                _context.Users.Add(nuevoUsuario);
                migrados++;
            }

            await _context.SaveChangesAsync();
            return migrados;
        }
    }

}
