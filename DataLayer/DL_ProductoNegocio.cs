using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_ProductoNegocio
    {

        private readonly string _cadenaConexion;

        public DL_ProductoNegocio(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<int> ObtenerStockProductoEnSucursalAsync(int idProducto, int idNegocio)
        {
            int stock = 0;

            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SP_OBTENERSTOCKPRODUCTOENSUCURSAL", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                        stock = Convert.ToInt32(result);
                }
            }

            return stock;
        }

        public async Task<string> CargarOActualizarStockProductoAsync(int idProducto, int idNegocio, int stock)
        {
            using var connection = new SqlConnection(_cadenaConexion);
            using var cmd = new SqlCommand("SP_CARGAROACTUALIZARSTOCKPRODUCTO", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto;
            cmd.Parameters.Add("@idNegocio", SqlDbType.Int).Value = idNegocio;
            cmd.Parameters.Add("@stock", SqlDbType.Int).Value = stock;

            // OUTPUTs del SP
            var pMensaje = cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200);
            pMensaje.Direction = ParameterDirection.Output;

           
            

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

           

            return pMensaje.Value?.ToString() ?? string.Empty;
        }

        public async Task SobrescribirStockAsync(int idProducto, int idNegocio, int stock)
        {
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SP_SOBRESCRIBIRSTOCK", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.Parameters.AddWithValue("@stock", stock);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EliminarProductoNegocioAsync(int idProducto, int idNegocio)
        {
            using (SqlConnection connection = new SqlConnection(_cadenaConexion))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SP_ELIMINARPRODUCTONEGOCIO", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

