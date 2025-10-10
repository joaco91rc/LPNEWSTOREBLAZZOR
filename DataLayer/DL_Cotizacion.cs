using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Cotizacion
    {
        private readonly string _cadenaConexion;

        public DL_Cotizacion(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }
        public async Task<Cotizacion> CotizacionActiva()
        {
            Cotizacion cotizacionDolar = new Cotizacion();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT idCotizacion, importe, fecha, estado");
                    query.AppendLine("FROM COTIZACION");
                    query.AppendLine("WHERE estado = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion)
                    {
                        CommandType = CommandType.Text
                    };

                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            cotizacionDolar.IdCotizacion = Convert.ToInt32(dr["idCotizacion"]);
                            cotizacionDolar.Importe = Convert.ToDecimal(dr["importe"]);
                            cotizacionDolar.Fecha = Convert.ToDateTime(dr["fecha"]);
                            cotizacionDolar.Estado = Convert.ToBoolean(dr["estado"]);
                        }
                    }
                }
                catch (Exception)
                {
                    cotizacionDolar = new Cotizacion();
                }
            }

            return cotizacionDolar;
        }




        public async Task<List<Cotizacion>> HistoricoCotizaciones()
        {
            List<Cotizacion> lista = new List<Cotizacion>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT idCotizacion, importe, fecha, estado FROM COTIZACION");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion)
                    {
                        CommandType = CommandType.Text
                    };

                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Cotizacion()
                            {
                                IdCotizacion = Convert.ToInt32(dr["idCotizacion"]),
                                Fecha = Convert.ToDateTime(dr["fecha"]),
                                Importe = Convert.ToDecimal(dr["importe"]),
                                Estado = Convert.ToBoolean(dr["estado"])
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    lista = new List<Cotizacion>();
                }
            }

            return lista;
        }



        public async Task<int> Registrar(Cotizacion objCotizacion, Action<string> setMensaje)
        {
            int idCotizaciongenerado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCOTIZACION", oconexion);

                    cmd.Parameters.AddWithValue("fecha", objCotizacion.Fecha);
                    cmd.Parameters.AddWithValue("importe", objCotizacion.Importe);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    idCotizaciongenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    setMensaje(cmd.Parameters["mensaje"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                idCotizaciongenerado = 0;
                setMensaje(ex.Message);
            }

            return idCotizaciongenerado;
        }



        public async Task<bool> Editar(Cotizacion objCotizacion, Action<string> setMensaje)
        {
            bool respuesta = false;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARCOTIZACION", oconexion);

                    cmd.Parameters.AddWithValue("idCotizacion", objCotizacion.IdCotizacion);
                    cmd.Parameters.AddWithValue("fecha", objCotizacion.Fecha);
                    cmd.Parameters.AddWithValue("importe", objCotizacion.Importe);
                    cmd.Parameters.AddWithValue("estado", objCotizacion.Estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    setMensaje(cmd.Parameters["mensaje"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                setMensaje(ex.Message);
            }

            return respuesta;
        }

    }
}
