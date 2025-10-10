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
    public class DL_Vendedor
    {
        private readonly string _cadenaConexion;

        public DL_Vendedor(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<Vendedor>> ListarVendedores()
        {
            List<Vendedor> lista = new List<Vendedor>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT * FROM VENDEDOR");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Vendedor()
                            {
                                IdVendedor = Convert.ToInt32(dr["idVendedor"]),
                                DNI = dr["dni"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Apellido = dr["apellido"].ToString(),
                                SueldoBase = Convert.ToDecimal(dr["sueldoBase"]),
                                SueldoComision = Convert.ToDecimal(dr["sueldoComision"]),
                                Estado = Convert.ToBoolean(dr["estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Vendedor>();
                }
            }
            return lista;
        }

        public async Task<(int idVendedorGenerado, string mensaje)> RegistrarVendedor(Vendedor objVendedor)
        {
            int idVendedorGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARVENDEDOR", oconexion);
                    cmd.Parameters.AddWithValue("dni", objVendedor.DNI);
                    cmd.Parameters.AddWithValue("nombre", objVendedor.Nombre);
                    cmd.Parameters.AddWithValue("apellido", objVendedor.Apellido);
                    cmd.Parameters.AddWithValue("sueldoBase", objVendedor.SueldoBase);
                    cmd.Parameters.AddWithValue("sueldoComision", objVendedor.SueldoComision);
                    cmd.Parameters.AddWithValue("estado", objVendedor.Estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    idVendedorGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idVendedorGenerado = 0;
                mensaje = ex.Message;
            }

            return (idVendedorGenerado, mensaje);
        }

        public async Task<(bool respuesta, string mensaje)> EditarVendedor(Vendedor objVendedor)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_MODIFICARVENDEDOR", oconexion);
                    cmd.Parameters.AddWithValue("idVendedor", objVendedor.IdVendedor);
                    cmd.Parameters.AddWithValue("dni", objVendedor.DNI);
                    cmd.Parameters.AddWithValue("nombre", objVendedor.Nombre);
                    cmd.Parameters.AddWithValue("apellido", objVendedor.Apellido);
                    cmd.Parameters.AddWithValue("sueldoBase", objVendedor.SueldoBase);
                    cmd.Parameters.AddWithValue("sueldoComision", objVendedor.SueldoComision);
                    cmd.Parameters.AddWithValue("estado", objVendedor.Estado);

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

        public async Task<(bool respuesta, string mensaje)> EliminarVendedor(Vendedor objVendedor)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARVENDEDOR", oconexion);
                    cmd.Parameters.AddWithValue("idVendedor", objVendedor.IdVendedor);
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
