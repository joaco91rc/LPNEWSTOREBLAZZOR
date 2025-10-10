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
    public class DL_HistorialServicioTecnico
    {
        private readonly string _cadenaConexion;

        public DL_HistorialServicioTecnico(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<HistorialServicioTecnico>> ListarHistorialServicioAsync(int idServicio)
        {
            List<HistorialServicioTecnico> lista = new List<HistorialServicioTecnico>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_LISTAR_HISTORIAL_SERVICIO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idServicio", idServicio);

                    await oconexion.OpenAsync();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new HistorialServicioTecnico()
                            {
                                IdHistorial = Convert.ToInt32(dr["idHistorial"]),
                                IdServicio = Convert.ToInt32(dr["idServicio"]),
                                DescripcionAccion = dr["descripcionAccion"].ToString(),
                                FechaAccion = Convert.ToDateTime(dr["fechaAccion"]),
                                Responsable = dr["responsable"].ToString(),
                                CostoManoDeObra = dr["costoManoDeObra"] != DBNull.Value ? Convert.ToDecimal(dr["costoManoDeObra"]) : (decimal?)null,
                                CostoRepuestos = dr["costoRepuestos"] != DBNull.Value ? Convert.ToDecimal(dr["costoRepuestos"]) : (decimal?)null,
                                HorasTrabajo = dr["horasTrabajo"] != DBNull.Value ? Convert.ToDecimal(dr["horasTrabajo"]) : (decimal?)null,
                                EstadoReparacion = dr["estadoReparacion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<HistorialServicioTecnico>();
            }

            return lista;
        }

        public async Task<(int IdHistorialGenerado, string Mensaje)> RegistrarHistorialServicioAsync(HistorialServicioTecnico objHistorial)
        {
            int idHistorialGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_INSERTAR_HISTORIAL_SERVICIO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("idServicio", objHistorial.IdServicio);
                    cmd.Parameters.AddWithValue("descripcionAccion", objHistorial.DescripcionAccion);
                    cmd.Parameters.AddWithValue("fechaAccion", objHistorial.FechaAccion);
                    cmd.Parameters.AddWithValue("responsable", objHistorial.Responsable);
                    cmd.Parameters.AddWithValue("costoManoDeObra", objHistorial.CostoManoDeObra);
                    cmd.Parameters.AddWithValue("costoRepuestos", objHistorial.CostoRepuestos);
                    cmd.Parameters.AddWithValue("horasTrabajo", objHistorial.HorasTrabajo);
                    cmd.Parameters.AddWithValue("estadoReparacion", objHistorial.EstadoReparacion);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    idHistorialGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idHistorialGenerado = 0;
                mensaje = ex.Message;
            }

            return (idHistorialGenerado, mensaje);
        }

        public async Task<(bool Resultado, string Mensaje)> ModificarHistorialServicioAsync(HistorialServicioTecnico objHistorial)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_ACTUALIZAR_HISTORIAL_SERVICIO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("idHistorial", objHistorial.IdHistorial);
                    cmd.Parameters.AddWithValue("idServicio", objHistorial.IdServicio);
                    cmd.Parameters.AddWithValue("descripcionAccion", objHistorial.DescripcionAccion);
                    cmd.Parameters.AddWithValue("fechaAccion", objHistorial.FechaAccion);
                    cmd.Parameters.AddWithValue("responsable", objHistorial.Responsable);
                    cmd.Parameters.AddWithValue("costoManoDeObra", objHistorial.CostoManoDeObra);
                    cmd.Parameters.AddWithValue("costoRepuestos", objHistorial.CostoRepuestos);
                    cmd.Parameters.AddWithValue("horasTrabajo", objHistorial.HorasTrabajo);
                    cmd.Parameters.AddWithValue("estadoReparacion", objHistorial.EstadoReparacion);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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

        public async Task<(bool Resultado, string Mensaje)> EliminarHistorialServicioAsync(int idHistorial)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINAR_HISTORIAL_SERVICIO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("idHistorial", idHistorial);

                    cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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
