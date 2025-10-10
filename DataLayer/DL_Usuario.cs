using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public  class DL_Usuario
    {
        private readonly string _cadenaConexion;

        public DL_Usuario(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<Usuario>> Listar()
        {
            var lista = new List<Usuario>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_USUARIOS", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Usuario()
                            {
                                Id = Convert.ToInt32(dr["idUsuario"]),
                                UserName = dr["UserName"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Email = dr["Email"].ToString(),
                                PhoneNumber = dr["PhoneNumber"].ToString(),
                                LockoutEnabled = Convert.ToBoolean(dr["estado"]),
                                oRol = new Rol()
                                {
                                    Id = Convert.ToInt32(dr["idRol"]),        // aquí el alias correcto
                                    Name = dr["descripcion"].ToString()       // y aquí también
                                }
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Usuario>();
                }
            }

            return lista;
        }



        public async Task<int> Registrar(Usuario usuario, int idRol)
        {
            int idGenerado = 0;

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
                cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object)usuario.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmailConfirmed", usuario.EmailConfirmed);
                cmd.Parameters.AddWithValue("@LockoutEnabled", usuario.LockoutEnabled);
                cmd.Parameters.AddWithValue("@AccessFailedCount", usuario.AccessFailedCount);
                cmd.Parameters.AddWithValue("@IdRol", idRol);

                SqlParameter outputId = new SqlParameter("@Id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(outputId.Value);
            }

            return idGenerado;
        }

        public async Task<bool> Modificar(Usuario usuario, int idRol)
        {
            bool resultado = false;

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_MODIFICAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", usuario.Id);
                cmd.Parameters.AddWithValue("@NombreCompleto", usuario.NombreCompleto);
                cmd.Parameters.AddWithValue("@UserName", usuario.UserName);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object)usuario.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmailConfirmed", usuario.EmailConfirmed);
                cmd.Parameters.AddWithValue("@LockoutEnabled", usuario.LockoutEnabled);
                cmd.Parameters.AddWithValue("@AccessFailedCount", usuario.AccessFailedCount);
                cmd.Parameters.AddWithValue("@IdRol", idRol);

                await conn.OpenAsync();
                resultado = await cmd.ExecuteNonQueryAsync() > 0;
            }

            return resultado;
        }

        public async Task<bool> Eliminar(int idUsuario)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", idUsuario);

                SqlParameter returnValue = new SqlParameter("@ReturnVal", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                int filasAfectadas = (int)returnValue.Value;

                return filasAfectadas > 0;
            }
        }




    }
}
