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
    public class DL_PrecioProducto
    {
        private readonly string _cadenaConexion;

        public DL_PrecioProducto(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        // LISTAR
        public async Task<List<PrecioProducto>> ListarPreciosProductoAsync()
        {
            var lista = new List<PrecioProducto>();

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_LISTARPRECIOPRODUCTOS", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await cn.OpenAsync();

                await using var dr = await cmd.ExecuteReaderAsync();
                if (!dr.HasRows) return lista;

                // Usar ordinales para performance
                int oIdPrecioProducto = dr.GetOrdinal("idPrecioProducto");
                int oNombreProducto = dr.GetOrdinal("NombreProducto");
                int oNombreMoneda = dr.GetOrdinal("NombreMoneda");
                int oSimbolo = dr.GetOrdinal("simbolo");
                int oPrecioCompra = dr.GetOrdinal("precioCompra");
                int oPrecioVenta = dr.GetOrdinal("precioVenta");
                int oPrecioLista = dr.GetOrdinal("precioLista");
                int oPrecioEfectivo = dr.GetOrdinal("precioEfectivo");
                int oFechaRegistro = dr.GetOrdinal("fechaRegistro");

                while (await dr.ReadAsync())
                {
                    lista.Add(new PrecioProducto
                    {
                        IdPrecioProducto = dr.GetInt32(oIdPrecioProducto),
                        NombreProducto = dr.GetString(oNombreProducto),
                        NombreMoneda = dr.GetString(oNombreMoneda),
                        SimboloMoneda = dr.GetString(oSimbolo),
                        PrecioCompra = dr.GetDecimal(oPrecioCompra),
                        PrecioVenta = dr.GetDecimal(oPrecioVenta),
                        PrecioLista = dr.GetDecimal(oPrecioLista),
                        PrecioEfectivo = dr.GetDecimal(oPrecioEfectivo),
                        FechaRegistro = dr.GetDateTime(oFechaRegistro)
                    });
                }
            }
            catch
            {
                // Podés loguear el error si querés
                lista = new List<PrecioProducto>();
            }

            return lista;
        }

        // OBTENER POR PRODUCTO + MONEDA
        public async Task<PrecioProducto?> ObtenerPreciosPorProductoYMonedaAsync(int idProducto, int idMoneda)
        {
            PrecioProducto? precioProducto = null;

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_OBTENERPRECIOPORPRODUCTOYMONEDA", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;
                cmd.Parameters.Add("@IdMoneda", SqlDbType.Int).Value = idMoneda;

                await cn.OpenAsync();

                await using var dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    int oIdPrecioProducto = dr.GetOrdinal("idPrecioProducto");
                    int oNombreProducto = dr.GetOrdinal("NombreProducto");
                    int oNombreMoneda = dr.GetOrdinal("NombreMoneda");
                    int oSimbolo = dr.GetOrdinal("simbolo");
                    int oPrecioCompra = dr.GetOrdinal("precioCompra");
                    int oPrecioVenta = dr.GetOrdinal("precioVenta");
                    int oPrecioLista = dr.GetOrdinal("precioLista");
                    int oPrecioEfectivo = dr.GetOrdinal("precioEfectivo");
                    int oFechaRegistro = dr.GetOrdinal("fechaRegistro");

                    precioProducto = new PrecioProducto
                    {
                        IdPrecioProducto = dr.GetInt32(oIdPrecioProducto),
                        NombreProducto = dr.GetString(oNombreProducto),
                        NombreMoneda = dr.GetString(oNombreMoneda),
                        SimboloMoneda = dr.GetString(oSimbolo),
                        PrecioCompra = dr.GetDecimal(oPrecioCompra),
                        PrecioVenta = dr.GetDecimal(oPrecioVenta),
                        PrecioLista = dr.GetDecimal(oPrecioLista),
                        PrecioEfectivo = dr.GetDecimal(oPrecioEfectivo),
                        FechaRegistro = dr.GetDateTime(oFechaRegistro)
                    };
                }
            }
            catch
            {
                precioProducto = null;
            }

            return precioProducto;
        }

        // REGISTRAR
        public async Task<(int idGenerado, string mensaje)> RegistrarPrecioProductoAsync(PrecioProducto obj)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_REGISTRARPRECIOPRODUCTO", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@idProducto", SqlDbType.Int).Value = obj.IdProducto;
                cmd.Parameters.Add("@idMoneda", SqlDbType.Int).Value = obj.IdMoneda;
                cmd.Parameters.Add("@precioCompra", SqlDbType.Decimal).Value = obj.PrecioCompra;
                cmd.Parameters.Add("@precioVenta", SqlDbType.Decimal).Value = obj.PrecioVenta;
                cmd.Parameters.Add("@precioLista", SqlDbType.Decimal).Value = obj.PrecioLista;
                cmd.Parameters.Add("@precioEfectivo", SqlDbType.Decimal).Value = obj.PrecioEfectivo;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = obj.FechaRegistro;

                var pResultado = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                pResultado.Direction = ParameterDirection.Output;

                var pMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500);
                pMensaje.Direction = ParameterDirection.Output;

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(pResultado.Value);
                mensaje = Convert.ToString(pMensaje.Value) ?? string.Empty;
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }

            return (idGenerado, mensaje);
        }

        // EDITAR
        public async Task<(bool exito, string mensaje)> EditarPrecioProductoAsync(PrecioProducto obj)
        {
            bool exito = false;
            string mensaje = string.Empty;

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_EDITARPRECIOPRODUCTO", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@idPrecioProducto", SqlDbType.Int).Value = obj.IdPrecioProducto;
                cmd.Parameters.Add("@idProducto", SqlDbType.Int).Value = obj.IdProducto;
                cmd.Parameters.Add("@idMoneda", SqlDbType.Int).Value = obj.IdMoneda;
                cmd.Parameters.Add("@precioCompra", SqlDbType.Decimal).Value = obj.PrecioCompra;
                cmd.Parameters.Add("@precioVenta", SqlDbType.Decimal).Value = obj.PrecioVenta;
                cmd.Parameters.Add("@precioLista", SqlDbType.Decimal).Value = obj.PrecioLista;
                cmd.Parameters.Add("@precioEfectivo", SqlDbType.Decimal).Value = obj.PrecioEfectivo;

                var pResultado = cmd.Parameters.Add("@resultado", SqlDbType.Bit);
                pResultado.Direction = ParameterDirection.Output;

                var pMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500);
                pMensaje.Direction = ParameterDirection.Output;

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                exito = Convert.ToBoolean(pResultado.Value);
                mensaje = Convert.ToString(pMensaje.Value) ?? string.Empty;
            }
            catch (Exception ex)
            {
                exito = false;
                mensaje = ex.Message;
            }

            return (exito, mensaje);
        }

        // ELIMINAR
        public async Task<(bool exito, string mensaje)> EliminarPrecioProductoAsync(int idPrecioProducto)
        {
            bool exito = false;
            string mensaje = string.Empty;

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_ELIMINARPRECIOPRODUCTO", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@idPrecioProducto", SqlDbType.Int).Value = idPrecioProducto;

                var pResultado = cmd.Parameters.Add("@resultado", SqlDbType.Bit);
                pResultado.Direction = ParameterDirection.Output;

                var pMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500);
                pMensaje.Direction = ParameterDirection.Output;

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                exito = Convert.ToBoolean(pResultado.Value);
                mensaje = Convert.ToString(pMensaje.Value) ?? string.Empty;
            }
            catch (Exception ex)
            {
                exito = false;
                mensaje = ex.Message;
            }

            return (exito, mensaje);
        }
    }

   
}

