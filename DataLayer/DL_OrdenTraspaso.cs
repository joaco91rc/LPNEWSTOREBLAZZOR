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
    public class DL_OrdenTraspaso
    {
        private readonly string _cadenaConexion;

        public DL_OrdenTraspaso(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }
        public async Task<List<OrdenTraspaso>> ListarAsync()
        {
            var lista = new List<OrdenTraspaso>();

            using (var conexion = new SqlConnection(_cadenaConexion))
            using (var comando = new SqlCommand("SP_LISTAR_ORDENTRASPASO", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                await conexion.OpenAsync();
                using var reader = await comando.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lista.Add(new OrdenTraspaso
                    {
                        IdOrdenTraspaso = reader.GetInt32(0),
                        IdSucursalOrigen = reader.GetInt32(1),
                        IdSucursalDestino = reader.GetInt32(2),
                        IdProducto = reader.GetInt32(3),
                        Cantidad = reader.GetInt32(4),
                        Confirmada = reader.GetString(5),
                        FechaCreacion = reader.GetDateTime(6),
                        FechaConfirmacion = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                        CostoProducto = reader.GetDecimal(8),
                        SerialNumber = reader.IsDBNull(9) ? null : reader.GetString(9)
                    });
                }
            }

            return lista;
        }

        public async Task<OrdenTraspaso?> ObtenerPorIdAsync(int id)
        {
            OrdenTraspaso? orden = null;

            using var conexion = new SqlConnection(_cadenaConexion);
            using var comando = new SqlCommand("SP_OBTENER_ORDENTRASPASO_POR_ID", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@Id", id);

            await conexion.OpenAsync();
            using var reader = await comando.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                orden = new OrdenTraspaso
                {
                    IdOrdenTraspaso = reader.GetInt32(0),
                    IdSucursalOrigen = reader.GetInt32(1),
                    IdSucursalDestino = reader.GetInt32(2),
                    IdProducto = reader.GetInt32(3),
                    Cantidad = reader.GetInt32(4),
                    Confirmada = reader.GetString(5),
                    FechaCreacion = reader.GetDateTime(6),
                    FechaConfirmacion = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                    CostoProducto = reader.GetDecimal(8),
                    SerialNumber = reader.GetString(9)
                };
            }

            return orden;
        }

        public async Task<bool> InsertarAsync(OrdenTraspaso o)
        {
            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var comando = new SqlCommand("SP_INSERTAR_ORDENTRASPASO", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                comando.Parameters.AddWithValue("@IdSucursalOrigen", o.IdSucursalOrigen);
                comando.Parameters.AddWithValue("@IdSucursalDestino", o.IdSucursalDestino);
                comando.Parameters.AddWithValue("@IdProducto", o.IdProducto);
                comando.Parameters.AddWithValue("@Cantidad", o.Cantidad);
                comando.Parameters.AddWithValue("@Confirmada", o.Confirmada);
                comando.Parameters.AddWithValue("@FechaCreacion", o.FechaCreacion);
                comando.Parameters.AddWithValue("@FechaConfirmacion",
                    o.FechaConfirmacion.HasValue ? o.FechaConfirmacion.Value : (object)DBNull.Value);
                comando.Parameters.AddWithValue("@CostoProducto", o.CostoProducto);
                comando.Parameters.AddWithValue("@SerialNumber",
                    string.IsNullOrEmpty(o.SerialNumber) ? (object)DBNull.Value : o.SerialNumber);

                await conexion.OpenAsync();

                int filasAfectadas = await comando.ExecuteNonQueryAsync();

                return filasAfectadas > 0; // true si insertó al menos una fila
            }
            catch (Exception ex)
            {
                // Opcional: loguear el error (Console, logger, etc.)
                Console.WriteLine($"Error al insertar orden de traspaso: {ex.Message}");
                return false;
            }
        }

        public async Task ConfirmarAsync(int id)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            using var comando = new SqlCommand("SP_CONFIRMAR_ORDENTRASPASO", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@Id", id);
            await conexion.OpenAsync();
            await comando.ExecuteNonQueryAsync();
        }

        public async Task RechazarAsync(int id)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            using var comando = new SqlCommand("SP_RECHAZAR_ORDENTRASPASO", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@Id", id);
            await conexion.OpenAsync();
            await comando.ExecuteNonQueryAsync();
        }
    }
}

