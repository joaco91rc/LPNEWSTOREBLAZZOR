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
    public class DL_Concepto
    {
        private readonly string _cadenaConexion;

        public DL_Concepto(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<Concepto>> ListarAsync()
        {
            List<Concepto> lista = new List<Concepto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand())
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT idConcepto, descripcion, tipo, estado, fechaRegistro FROM CONCEPTO");

                    cmd.Connection = oconexion;
                    cmd.CommandText = query.ToString();
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Concepto()
                            {
                                IdConcepto = Convert.ToInt32(dr["idConcepto"]),
                                Descripcion = dr["descripcion"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"]),
                                FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                                Tipo = dr["tipo"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Concepto>();
            }

            return lista;
        }

        public async Task<(int idGenerado, string mensaje)> RegistrarAsync(Concepto objConcepto)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_REGISTRARCONCEPTO", oconexion))
                {
                    cmd.Parameters.AddWithValue("descripcion", objConcepto.Descripcion);
                    cmd.Parameters.AddWithValue("estado", objConcepto.Estado);
                    cmd.Parameters.AddWithValue("fechaRegistro", objConcepto.FechaRegistro);
                    cmd.Parameters.AddWithValue("tipo", objConcepto.Tipo);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    idGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }

            return (idGenerado, mensaje);
        }

        public async Task<(bool resultado, string mensaje)> EditarAsync(Concepto objConcepto)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_EDITARCONCEPTO", oconexion))
                {
                    cmd.Parameters.AddWithValue("idConcepto", objConcepto.IdConcepto);
                    cmd.Parameters.AddWithValue("descripcion", objConcepto.Descripcion);
                    cmd.Parameters.AddWithValue("estado", objConcepto.Estado);
                    cmd.Parameters.AddWithValue("fechaRegistro", objConcepto.FechaRegistro);
                    cmd.Parameters.AddWithValue("tipo", objConcepto.Tipo);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<(bool resultado, string mensaje)> EliminarAsync(Concepto objConcepto)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_ELIMINARCONCEPTO", oconexion))
                {
                    cmd.Parameters.AddWithValue("idConcepto", objConcepto.IdConcepto);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }

    }
}
