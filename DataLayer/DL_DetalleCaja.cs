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
    public class DL_DetalleCaja
    {
        private readonly string _cadenaConexion;

        public DL_DetalleCaja(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<DetalleCaja>> DetalleCajaAsync(string fechaConsulta)
        {
            List<DetalleCaja> lista = new List<DetalleCaja>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_DETALLECAJA", oconexion))
                {
                    cmd.Parameters.AddWithValue("fechaConsulta", fechaConsulta);
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new DetalleCaja()
                            {
                                FechaApertura = dr["fechaApertura"].ToString(),
                                Hora = dr["hora"].ToString(),
                                TipoTransaccion = dr["tipoTransaccion"].ToString(),
                                Monto = Convert.ToDecimal(dr["monto"].ToString()),
                                DocAsociado = dr["docAsociado"].ToString(),
                                UsuarioTransaccion = dr["usuarioTransaccion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<DetalleCaja>();
            }

            return lista;
        }

        public async Task<List<DetalleCaja>> ListarAsync(string fecha, int idNegocio)
        {
            List<DetalleCaja> lista = new List<DetalleCaja>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select cr.fechaApertura, cr.fechaCierre, tc.hora, tc.tipoTransaccion, tc.monto, tc.formaPago, tc.docAsociado,");
                    query.AppendLine("tc.usuarioTransaccion, tc.idCompra, tc.idVenta, tc.idTransaccion");
                    query.AppendLine("from CAJA_REGISTRADORA cr");
                    query.AppendLine("inner join TRANSACCION_CAJA tc on tc.idCajaRegistradora = cr.idCajaRegistradora");
                    query.AppendLine("where CONVERT(DATE, cr.fechaApertura) = @fecha AND cr.fechaCierre IS NOT NULL AND tc.idNegocio = @idNegocio");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), oconexion))
                    {
                        cmd.Parameters.AddWithValue("@fecha", fecha);
                        cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                        cmd.CommandType = CommandType.Text;

                        await oconexion.OpenAsync();

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                lista.Add(new DetalleCaja()
                                {
                                    FechaApertura = dr["fechaApertura"].ToString(),
                                    Hora = dr["hora"].ToString(),
                                    TipoTransaccion = dr["tipoTransaccion"].ToString(),
                                    Monto = Convert.ToDecimal(dr["monto"].ToString()),
                                    FormaPago = dr["formaPago"].ToString(),
                                    DocAsociado = dr["docAsociado"].ToString(),
                                    UsuarioTransaccion = dr["usuarioTransaccion"].ToString(),
                                    IdCompra = dr["idCompra"] != DBNull.Value ? Convert.ToInt32(dr["idCompra"]) : 0,
                                    IdVenta = dr["idVenta"] != DBNull.Value ? Convert.ToInt32(dr["idVenta"]) : 0,
                                    IdTransaccion = Convert.ToInt32(dr["idTransaccion"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<DetalleCaja>();
            }

            return lista;
        }

    }
}
