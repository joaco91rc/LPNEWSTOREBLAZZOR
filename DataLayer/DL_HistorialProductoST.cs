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
    public class DL_HistorialProductoST
    {
        private readonly string _cadenaConexion;

        public DL_HistorialProductoST(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<(int IdGenerado, string Mensaje)> InsertarHistorialProductoAsync(HistorialProductoServTec objHistorialProducto)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using SqlConnection oconexion = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("SP_INSERTAR_HISTORIAL_PRODUCTO", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idHistorialServicio", objHistorialProducto.IdHistorialServicio);
                cmd.Parameters.AddWithValue("idProducto", objHistorialProducto.IdProducto);
                cmd.Parameters.AddWithValue("cantidad", objHistorialProducto.Cantidad);
                cmd.Parameters.AddWithValue("precioVenta", objHistorialProducto.PrecioVenta);

                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                await oconexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }

            return (idGenerado, mensaje);
        }

        public async Task<(bool Resultado, string Mensaje)> ModificarHistorialProductoAsync(HistorialProductoServTec objHistorialProducto)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using SqlConnection oconexion = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("SP_MODIFICAR_HISTORIAL_PRODUCTO", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idHistorialProducto", objHistorialProducto.IdHistorialProducto);
                cmd.Parameters.AddWithValue("idHistorialServicio", objHistorialProducto.IdHistorialServicio);
                cmd.Parameters.AddWithValue("idProducto", objHistorialProducto.IdProducto);
                cmd.Parameters.AddWithValue("cantidad", objHistorialProducto.Cantidad);
                cmd.Parameters.AddWithValue("precioVenta", objHistorialProducto.PrecioVenta);

                cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                await oconexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<(bool Resultado, string Mensaje)> EliminarHistorialProductoAsync(int idHistorialProducto)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using SqlConnection oconexion = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("SP_ELIMINAR_HISTORIAL_PRODUCTO", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idHistorialProducto", idHistorialProducto);

                cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                await oconexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<List<HistorialProductoServTec>> ListarHistorialProductosAsync(int idHistorialServicio)
        {
            List<HistorialProductoServTec> lista = new();

            try
            {
                using SqlConnection oconexion = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("SP_LISTAR_HISTORIAL_PRODUCTOS", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idHistorialServicio", idHistorialServicio);

                await oconexion.OpenAsync();

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new HistorialProductoServTec()
                    {
                        IdHistorialProducto = Convert.ToInt32(dr["idHistorialProducto"]),
                        IdHistorialServicio = Convert.ToInt32(dr["idHistorialServicio"]),
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        NombreProducto = dr["nombreProducto"].ToString(),
                        CodigoProducto = dr["codigoProducto"].ToString(),
                        Cantidad = Convert.ToDecimal(dr["cantidad"]),
                        PrecioVenta = Convert.ToDecimal(dr["precioVenta"])
                    });
                }
            }
            catch (Exception)
            {
                lista = new List<HistorialProductoServTec>();
            }

            return lista;
        }

    }
}
