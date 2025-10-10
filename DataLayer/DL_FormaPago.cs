using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.FormaPago;

namespace DataLayer
{
    public class DL_FormaPago
    {
        private readonly string _cadenaConexion;

        public DL_FormaPago(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<FormaPago>> ListarFormasDePago()
        {
            var lista = new List<FormaPago>();

            try
            {
                await using var conexion = new SqlConnection(_cadenaConexion);
                var query = new StringBuilder();
                query.AppendLine("SELECT * FROM FORMAPAGO");
                query.AppendLine("ORDER BY descripcion");
                await using var cmd = new SqlCommand(query.ToString(), conexion)
                {
                    CommandType = CommandType.Text
                };

                await conexion.OpenAsync();

                await using var dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new FormaPago()
                    {
                        IdFormaPago = Convert.ToInt32(dr["idFormaPago"]),
                        Descripcion = dr["descripcion"].ToString()!,
                        PorcentajeRetencion = Convert.ToDecimal(dr["porcentajeRetencion"]),
                        PorcentajeRecargo = Convert.ToDecimal(dr["porcentajeRecargo"]),
                        PorcentajeDescuento = Convert.ToDecimal(dr["porcentajeDescuento"]),
                        Tipo = Enum.TryParse<TipoFormaPago>(dr["tipo"].ToString(), out var tipo) ? tipo : TipoFormaPago.EFECTIVO, // Valor por defecto si falla
                        CajaAsociada = Enum.TryParse<CajaAsociadaFP>(dr["cajaAsociada"].ToString(), out var caja) ? caja : CajaAsociadaFP.MERCADOPAGO, // Valor por defecto si falla


                        PorcentajeRecargoDolar = Convert.ToDecimal(dr["porcentajeRecargoDolar"]),
                        PorcentajeDescuentoDolar = Convert.ToDecimal(dr["porcentajeDescuentoDolar"]),
                    });
                }
            }
            catch (Exception)
            {
                lista = new List<FormaPago>();
                // Puedes loguear el error si quieres
            }

            return lista;
        }

        public async Task<(int idGenerado, string mensaje)> RegistrarFormaPago(FormaPago objFormaPago)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                await using var conexion = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_REGISTRARFORMAPAGO", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("descripcion", objFormaPago.Descripcion);
                cmd.Parameters.AddWithValue("porcentajeRetencion", objFormaPago.PorcentajeRetencion);
                cmd.Parameters.AddWithValue("porcentajeRecargo", objFormaPago.PorcentajeRecargo);
                cmd.Parameters.AddWithValue("porcentajeDescuento", objFormaPago.PorcentajeDescuento);
                cmd.Parameters.AddWithValue("porcentajeRecargoDolar", objFormaPago.PorcentajeRecargoDolar);
                cmd.Parameters.AddWithValue("porcentajeDescuentoDolar", objFormaPago.PorcentajeDescuentoDolar);
                cmd.Parameters.AddWithValue("cajaAsociada", objFormaPago.CajaAsociada.ToString());
                cmd.Parameters.AddWithValue("tipo", objFormaPago.Tipo.ToString());

                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }

            return (idGenerado, mensaje);
        }

        public async Task<(bool resultado, string mensaje)> EditarFormaPago(FormaPago objFormaPago)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                await using var conexion = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_EDITARFORMAPAGO", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idFormaPago", objFormaPago.IdFormaPago);
                cmd.Parameters.AddWithValue("descripcion", objFormaPago.Descripcion);
                cmd.Parameters.AddWithValue("cajaAsociada", objFormaPago.CajaAsociada.ToString());
                cmd.Parameters.AddWithValue("porcentajeRetencion", objFormaPago.PorcentajeRetencion);
                cmd.Parameters.AddWithValue("porcentajeDescuento", objFormaPago.PorcentajeDescuento);
                cmd.Parameters.AddWithValue("porcentajeRecargoDolar", objFormaPago.PorcentajeRecargoDolar);
                cmd.Parameters.AddWithValue("porcentajeDescuentoDolar", objFormaPago.PorcentajeDescuentoDolar);
                cmd.Parameters.AddWithValue("porcentajeRecargo", objFormaPago.PorcentajeRecargo);
                cmd.Parameters.AddWithValue("tipo", objFormaPago.Tipo.ToString());

                cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = "Error al editar la forma de pago: " + ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<FormaPago?> ObtenerFPPorDescripcion(string descripcion)
        {
            FormaPago? formaPago = null;

            try
            {
                await using var conexion = new SqlConnection(_cadenaConexion);
                var query = "SELECT * FROM FORMAPAGO WHERE descripcion = @descripcion";
                await using var cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);

                await conexion.OpenAsync();

                await using var dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    formaPago = new FormaPago()
                    {
                        IdFormaPago = Convert.ToInt32(dr["idFormaPago"]),
                        Descripcion = dr["descripcion"].ToString()!,
                        PorcentajeRetencion = Convert.ToDecimal(dr["porcentajeRetencion"]),
                        PorcentajeRecargo = Convert.ToDecimal(dr["porcentajeRecargo"]),
                        PorcentajeDescuento = Convert.ToDecimal(dr["porcentajeDescuento"]),
                        Tipo = Enum.TryParse<TipoFormaPago>(dr["tipo"].ToString(), out var tipo) ? tipo : TipoFormaPago.EFECTIVO, // Valor por defecto si falla
                        CajaAsociada = Enum.TryParse<CajaAsociadaFP>(dr["cajaAsociada"].ToString(), out var caja) ? caja : CajaAsociadaFP.MERCADOPAGO, // Valor por defecto si falla
                        PorcentajeRecargoDolar = Convert.ToDecimal(dr["porcentajeRecargoDolar"]),
                        PorcentajeDescuentoDolar = Convert.ToDecimal(dr["porcentajeDescuentoDolar"]),
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la forma de pago por descripción", ex);
            }

            return formaPago;
        }

        public async Task<(bool resultado, string mensaje)> Eliminar(int idFormaPago)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                await using var conexion = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_ELIMINARFORMAPAGO", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("idFormaPago", idFormaPago);
                cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
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
