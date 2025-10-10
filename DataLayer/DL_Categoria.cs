using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Categoria
    {
        private readonly string _cadenaConexion;

        public DL_Categoria(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<Categoria>> Listar()
        {
            List<Categoria> lista = new List<Categoria>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select idCategoria,descripcion,estado from CATEGORIA");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Descripcion = dr["descripcion"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Categoria>();
                }
            }
            return lista;
        }

        public async Task<(int idCategoriagenerado, string mensaje)> Registrar(Categoria objCategoria)
        {
            int idCategoriagenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCATEGORIA", oconexion);
                    cmd.Parameters.AddWithValue("descripcion", objCategoria.Descripcion);
                    cmd.Parameters.AddWithValue("estado", objCategoria.Estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    idCategoriagenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idCategoriagenerado = 0;
                mensaje = ex.Message;
            }

            return (idCategoriagenerado, mensaje);
        }

        public async Task<(bool respuesta, string mensaje)> Editar(Categoria objCategoria)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARCATEGORIA", oconexion);
                    cmd.Parameters.AddWithValue("idCategoria", objCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("descripcion", objCategoria.Descripcion);
                    cmd.Parameters.AddWithValue("estado", objCategoria.Estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }

            return (respuesta, mensaje);
        }

        public async Task<(bool respuesta, string mensaje)> Eliminar(Categoria objCategoria)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARCATEGORIA", oconexion);
                    cmd.Parameters.AddWithValue("idCategoria", objCategoria.IdCategoria);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }

            return (respuesta, mensaje);
        }
    }
}
