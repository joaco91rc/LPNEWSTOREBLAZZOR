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
    public class DL_CajaRegistradora
    {

        private readonly string _cadenaConexion;

        public DL_CajaRegistradora(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }


        public async Task<(int idCajaGenerado, string mensaje)> AperturaCajaAsync(CajaRegistradora objCajaRegistradora, int idNegocio)
        {
            int idCajaGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_APERTURACAJA", oconexion))
                {
                    cmd.Parameters.AddWithValue("usuarioAperturaCaja", objCajaRegistradora.UsuarioAperturaCaja);
                    cmd.Parameters.AddWithValue("estado", true);
                    cmd.Parameters.AddWithValue("saldoInicial", objCajaRegistradora.SaldoInicio);
                    cmd.Parameters.AddWithValue("saldoInicialMP", objCajaRegistradora.SaldoInicioMP);
                    cmd.Parameters.AddWithValue("saldoInicialUSS", objCajaRegistradora.SaldoInicioUSS);
                    cmd.Parameters.AddWithValue("saldoInicialGalicia", objCajaRegistradora.SaldoInicioGalicia);
                    cmd.Parameters.AddWithValue("idNegocio", idNegocio);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    idCajaGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idCajaGenerado = 0;
                mensaje = ex.Message;
            }

            return (idCajaGenerado, mensaje);
        }


        public async Task<CajaRegistradora> ObtenerCajaPorFechaAsync(string fecha, int idNegocio)
        {
            CajaRegistradora caja = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand())
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT * FROM CAJA_REGISTRADORA");
                    query.AppendLine("WHERE CONVERT(DATE, fechaApertura) = @fecha AND idNegocio = @idNegocio");

                    cmd.Connection = oconexion;
                    cmd.CommandText = query.ToString();
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            caja = new CajaRegistradora()
                            {
                                IdCajaRegistradora = Convert.ToInt32(dr["idCajaRegistradora"]),
                                FechaApertura = dr["fechaApertura"].ToString(),
                                FechaCierre = dr["fechaCierre"].ToString(),
                                UsuarioAperturaCaja = dr["usuarioAperturaCaja"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"]),
                                Saldo = dr["saldo"] != DBNull.Value ? Convert.ToDecimal(dr["saldo"]) : 0,
                                SaldoMP = dr["saldoMP"] != DBNull.Value ? Convert.ToDecimal(dr["saldoMP"]) : 0,
                                SaldoUSS = dr["saldoUSS"] != DBNull.Value ? Convert.ToDecimal(dr["saldoUSS"]) : 0,
                                SaldoGalicia = dr["saldoGalicia"] != DBNull.Value ? Convert.ToDecimal(dr["saldoGalicia"]) : 0
                            };
                        }
                    }
                }
            }
            catch
            {
                caja = null;
            }

            return caja;
        }


        public async Task<List<CajaRegistradora>> ListarAsync(int idNegocio)
        {
            List<CajaRegistradora> lista = new List<CajaRegistradora>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT * FROM CAJA_REGISTRADORA WHERE idNegocio = @idNegocio");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), oconexion))
                    {
                        cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                        cmd.CommandType = CommandType.Text;

                        await oconexion.OpenAsync();

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                CajaRegistradora caja = new CajaRegistradora()
                                {
                                    IdCajaRegistradora = Convert.ToInt32(dr["idCajaRegistradora"]),
                                    FechaApertura = dr["fechaApertura"].ToString(),
                                    FechaCierre = dr["fechaCierre"].ToString(),
                                    UsuarioAperturaCaja = dr["usuarioAperturaCaja"].ToString(),
                                    Estado = Convert.ToBoolean(dr["estado"]),
                                    IdNegocio = idNegocio,
                                    Saldo = dr["saldo"] != DBNull.Value ? Convert.ToDecimal(dr["saldo"]) : 0,
                                    SaldoMP = dr["saldoMP"] != DBNull.Value ? Convert.ToDecimal(dr["saldoMP"]) : 0,
                                    SaldoUSS = dr["saldoUSS"] != DBNull.Value ? Convert.ToDecimal(dr["saldoUSS"]) : 0,
                                    SaldoGalicia = dr["saldoGalicia"] != DBNull.Value ? Convert.ToDecimal(dr["saldoGalicia"]) : 0,
                                    SaldoInicio = dr["saldoInicio"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicio"]) : 0,
                                    SaldoInicioMP = dr["saldoInicioMP"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicioMP"]) : 0,
                                    SaldoInicioUSS = dr["saldoInicioUSS"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicioUSS"]) : 0,
                                    SaldoInicioGalicia = dr["saldoInicioGalicia"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicioGalicia"]) : 0
                                };

                                lista.Add(caja);
                            }
                        }
                    }
                }
                catch
                {
                    lista = new List<CajaRegistradora>();
                }
            }
            return lista;
        }

        public async Task<CajaRegistradora> ObtenerUltimaCajaCerradaAsync(int idNegocio)
        {
            CajaRegistradora ultimaCaja = null;
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT TOP 1 *");
                    query.AppendLine("FROM CAJA_REGISTRADORA");
                    query.AppendLine("WHERE idNegocio = @idNegocio AND estado = 0");
                    query.AppendLine("ORDER BY fechaCierre DESC");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), oconexion))
                    {
                        cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                        cmd.CommandType = CommandType.Text;

                        await oconexion.OpenAsync();

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            if (await dr.ReadAsync())
                            {
                                ultimaCaja = new CajaRegistradora()
                                {
                                    IdCajaRegistradora = Convert.ToInt32(dr["idCajaRegistradora"]),
                                    FechaApertura = dr["fechaApertura"].ToString(),
                                    FechaCierre = dr["fechaCierre"].ToString(),
                                    UsuarioAperturaCaja = dr["usuarioAperturaCaja"].ToString(),
                                    Estado = Convert.ToBoolean(dr["estado"]),
                                    IdNegocio = idNegocio,
                                    Saldo = dr["saldo"] != DBNull.Value ? Convert.ToDecimal(dr["saldo"]) : 0,
                                    SaldoMP = dr["saldoMP"] != DBNull.Value ? Convert.ToDecimal(dr["saldoMP"]) : 0,
                                    SaldoUSS = dr["saldoUSS"] != DBNull.Value ? Convert.ToDecimal(dr["saldoUSS"]) : 0,
                                    SaldoGalicia = dr["saldoGalicia"] != DBNull.Value ? Convert.ToDecimal(dr["saldoGalicia"]) : 0,
                                    SaldoInicio = dr["saldoInicio"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicio"]) : 0,
                                    SaldoInicioMP = dr["saldoInicioMP"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicioMP"]) : 0,
                                    SaldoInicioUSS = dr["saldoInicioUSS"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicioUSS"]) : 0,
                                    SaldoInicioGalicia = dr["saldoInicioGalicia"] != DBNull.Value ? Convert.ToDecimal(dr["saldoInicioGalicia"]) : 0
                                };
                            }
                        }
                    }
                }
                catch
                {
                    ultimaCaja = null;
                }
            }
            return ultimaCaja;
        }

        public async Task<(bool resultado, string mensaje)> CerrarCajaAsync(CajaRegistradora objCajaRegistradora, int idNegocio)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_CERRARCAJA", oconexion))
                {
                    cmd.Parameters.AddWithValue("idCajaRegistradora", objCajaRegistradora.IdCajaRegistradora);
                    cmd.Parameters.AddWithValue("fechaCierre", Convert.ToDateTime(objCajaRegistradora.FechaCierre));
                    cmd.Parameters.AddWithValue("saldo", objCajaRegistradora.Saldo);
                    cmd.Parameters.AddWithValue("saldoMP", objCajaRegistradora.SaldoMP);
                    cmd.Parameters.AddWithValue("saldoUSS", objCajaRegistradora.SaldoUSS);
                    cmd.Parameters.AddWithValue("saldoGalicia", objCajaRegistradora.SaldoGalicia);
                    cmd.Parameters.AddWithValue("idNegocio", idNegocio);

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
