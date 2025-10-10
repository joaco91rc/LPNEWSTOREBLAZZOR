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
    public  class DL_PagoParcial
    {
        private readonly string _cadenaConexion;

        public DL_PagoParcial(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        private static string NombreLocalFrom(int idNegocio) => idNegocio switch
        {
            1 => "HITECH 1",
            2 => "HITECH 2",
            3 => "APPLE 49",
            4 => "APPLE CAFE",
            _ => "Desconocido"
        };

        private static PagoParcial MapearPagoParcial(SqlDataReader dr, int idNegocio,
            // ordinals cache
            int oIdPagoParcial, int oIdCliente, int oNombreCliente, int oMonto, int oIdVenta,
            int oNumeroVenta, int oFechaRegistro, int oEstado, int oFormaPago, int oProductoReservado,
            int oVendedor, int oMoneda)
        {
            string nombreLocal = NombreLocalFrom(idNegocio);

            int? idVenta = dr.IsDBNull(oIdVenta) ? (int?)null : dr.GetInt32(oIdVenta);

            return new PagoParcial
            {
                IdPagoParcial = dr.GetInt32(oIdPagoParcial),
                IdCliente = dr.GetInt32(oIdCliente),
                NombreCliente = dr.IsDBNull(oNombreCliente) ? "" : dr.GetString(oNombreCliente),
                Monto = dr.GetDecimal(oMonto),
                IdVenta = idVenta,
                NumeroVenta = dr.IsDBNull(oNumeroVenta) ? "" : dr.GetString(oNumeroVenta),
                FechaRegistro = dr.GetDateTime(oFechaRegistro),
                Estado = dr.GetBoolean(oEstado),
                FormaPago = dr.IsDBNull(oFormaPago) ? "" : dr.GetString(oFormaPago),
                ProductoReservado = dr.IsDBNull(oProductoReservado) ? "" : dr.GetString(oProductoReservado),
                Vendedor = dr.IsDBNull(oVendedor) ? "" : dr.GetString(oVendedor),
                IdNegocio = idNegocio,
                NombreLocal = nombreLocal,
                Moneda = dr.IsDBNull(oMoneda) ? "" : dr.GetString(oMoneda)
            };
        }

        // ===== LISTAR (todos activos) =====
        public async Task<List<PagoParcial>> ListarAsync(CancellationToken ct = default)
        {
            var lista = new List<PagoParcial>();
            const string query = @"
SELECT p.idPagoParcial, p.idCliente, p.moneda, c.nombreCompleto AS nombreCliente, 
       p.monto, p.idVenta, p.vendedor, p.idNegocio,
       ISNULL(v.nroDocumento, '') AS numeroVenta,
       p.fechaRegistro, p.estado, p.formaPago, p.productoReservado
FROM PAGOPARCIAL p
INNER JOIN CLIENTE c ON p.idCliente = c.idCliente
LEFT JOIN VENTA v ON p.idVenta = v.idVenta
WHERE p.estado = 1;";

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand(query, cn) { CommandType = CommandType.Text };
                await cn.OpenAsync(ct).ConfigureAwait(false);

                await using var dr = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

                // cache ordinals
                int oIdPagoParcial = dr.GetOrdinal("idPagoParcial");
                int oIdCliente = dr.GetOrdinal("idCliente");
                int oNombreCliente = dr.GetOrdinal("nombreCliente");
                int oMonto = dr.GetOrdinal("monto");
                int oIdVenta = dr.GetOrdinal("idVenta");
                int oNumeroVenta = dr.GetOrdinal("numeroVenta");
                int oFechaRegistro = dr.GetOrdinal("fechaRegistro");
                int oEstado = dr.GetOrdinal("estado");
                int oFormaPago = dr.GetOrdinal("formaPago");
                int oProductoReservado = dr.GetOrdinal("productoReservado");
                int oVendedor = dr.GetOrdinal("vendedor");
                int oIdNegocio = dr.GetOrdinal("idNegocio");
                int oMoneda = dr.GetOrdinal("moneda");

                while (await dr.ReadAsync(ct).ConfigureAwait(false))
                {
                    int idNegocio = dr.GetInt32(oIdNegocio);

                    lista.Add(MapearPagoParcial(
                        dr, idNegocio,
                        oIdPagoParcial, oIdCliente, oNombreCliente, oMonto, oIdVenta,
                        oNumeroVenta, oFechaRegistro, oEstado, oFormaPago, oProductoReservado,
                        oVendedor, oMoneda));
                }
            }
            catch
            {
                // si querés, log
                return new List<PagoParcial>();
            }

            return lista;
        }

        // ===== LISTAR por local (activos) =====
        public async Task<List<PagoParcial>> ListarPagosParcialesPorLocalAsync(int idNegocio, CancellationToken ct = default)
        {
            var lista = new List<PagoParcial>();
            const string query = @"
SELECT p.idPagoParcial, p.moneda, p.idCliente, c.nombreCompleto AS nombreCliente, 
       p.monto, p.idVenta, p.vendedor, p.idNegocio,
       ISNULL(v.nroDocumento, '') AS numeroVenta, 
       p.fechaRegistro, p.estado, p.formaPago, p.productoReservado
FROM PAGOPARCIAL p
INNER JOIN CLIENTE c ON p.idCliente = c.idCliente
LEFT JOIN VENTA v ON p.idVenta = v.idVenta
WHERE p.idNegocio = @idNegocio AND p.estado = 1;";

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand(query, cn) { CommandType = CommandType.Text };
                cmd.Parameters.Add("@idNegocio", SqlDbType.Int).Value = idNegocio;

                await cn.OpenAsync(ct).ConfigureAwait(false);
                await using var dr = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

                int oIdPagoParcial = dr.GetOrdinal("idPagoParcial");
                int oIdCliente = dr.GetOrdinal("idCliente");
                int oNombreCliente = dr.GetOrdinal("nombreCliente");
                int oMonto = dr.GetOrdinal("monto");
                int oIdVenta = dr.GetOrdinal("idVenta");
                int oNumeroVenta = dr.GetOrdinal("numeroVenta");
                int oFechaRegistro = dr.GetOrdinal("fechaRegistro");
                int oEstado = dr.GetOrdinal("estado");
                int oFormaPago = dr.GetOrdinal("formaPago");
                int oProductoReservado = dr.GetOrdinal("productoReservado");
                int oVendedor = dr.GetOrdinal("vendedor");
                int oMoneda = dr.GetOrdinal("moneda");

                while (await dr.ReadAsync(ct).ConfigureAwait(false))
                {
                    lista.Add(MapearPagoParcial(
                        dr, idNegocio,
                        oIdPagoParcial, oIdCliente, oNombreCliente, oMonto, oIdVenta,
                        oNumeroVenta, oFechaRegistro, oEstado, oFormaPago, oProductoReservado,
                        oVendedor, oMoneda));
                }
            }
            catch
            {
                return new List<PagoParcial>();
            }

            return lista;
        }

        public async Task<List<PagoParcial>> ListarPagosParcialesActivosAsync(int idNegocio, CancellationToken ct = default)
        {
            var lista = new List<PagoParcial>();
            const string query = @"
SELECT p.idPagoParcial, p.moneda, p.idCliente, c.nombreCompleto AS nombreCliente, 
       p.monto, p.idVenta, p.vendedor, p.idNegocio,
       ISNULL(v.nroDocumento, '') AS numeroVenta, 
       p.fechaRegistro, p.estado, p.formaPago, p.productoReservado
FROM PAGOPARCIAL p
INNER JOIN CLIENTE c ON p.idCliente = c.idCliente
LEFT JOIN VENTA v ON p.idVenta = v.idVenta
WHERE p.idNegocio = @idNegocio AND p.estado = 1;";

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand(query, cn) { CommandType = CommandType.Text };
                cmd.Parameters.Add("@idNegocio", SqlDbType.Int).Value = idNegocio;

                await cn.OpenAsync(ct).ConfigureAwait(false);
                await using var dr = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

                int oIdPagoParcial = dr.GetOrdinal("idPagoParcial");
                int oIdCliente = dr.GetOrdinal("idCliente");
                int oNombreCliente = dr.GetOrdinal("nombreCliente");
                int oMonto = dr.GetOrdinal("monto");
                int oIdVenta = dr.GetOrdinal("idVenta");
                int oNumeroVenta = dr.GetOrdinal("numeroVenta");
                int oFechaRegistro = dr.GetOrdinal("fechaRegistro");
                int oEstado = dr.GetOrdinal("estado");
                int oFormaPago = dr.GetOrdinal("formaPago");
                int oProductoReservado = dr.GetOrdinal("productoReservado");
                int oVendedor = dr.GetOrdinal("vendedor");
                int oMoneda = dr.GetOrdinal("moneda");

                while (await dr.ReadAsync(ct).ConfigureAwait(false))
                {
                    lista.Add(MapearPagoParcial(
                        dr, idNegocio,
                        oIdPagoParcial, oIdCliente, oNombreCliente, oMonto, oIdVenta,
                        oNumeroVenta, oFechaRegistro, oEstado, oFormaPago, oProductoReservado,
                        oVendedor, oMoneda));
                }
            }
            catch
            {
                return new List<PagoParcial>();
            }

            return lista;
        }

        public async Task<List<PagoParcial>> ListarPagosParcialesInactivosAsync(int idNegocio, CancellationToken ct = default)
        {
            var lista = new List<PagoParcial>();
            const string query = @"
SELECT p.idPagoParcial, p.moneda, p.idCliente, c.nombreCompleto AS nombreCliente, 
       p.monto, p.idVenta, p.vendedor, p.idNegocio,
       ISNULL(v.nroDocumento, '') AS numeroVenta, 
       p.fechaRegistro, p.estado, p.formaPago, p.productoReservado
FROM PAGOPARCIAL p
INNER JOIN CLIENTE c ON p.idCliente = c.idCliente
LEFT JOIN VENTA v ON p.idVenta = v.idVenta
WHERE p.idNegocio = @idNegocio AND p.estado = 0;";

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand(query, cn) { CommandType = CommandType.Text };
                cmd.Parameters.Add("@idNegocio", SqlDbType.Int).Value = idNegocio;

                await cn.OpenAsync(ct).ConfigureAwait(false);
                await using var dr = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

                int oIdPagoParcial = dr.GetOrdinal("idPagoParcial");
                int oIdCliente = dr.GetOrdinal("idCliente");
                int oNombreCliente = dr.GetOrdinal("nombreCliente");
                int oMonto = dr.GetOrdinal("monto");
                int oIdVenta = dr.GetOrdinal("idVenta");
                int oNumeroVenta = dr.GetOrdinal("numeroVenta");
                int oFechaRegistro = dr.GetOrdinal("fechaRegistro");
                int oEstado = dr.GetOrdinal("estado");
                int oFormaPago = dr.GetOrdinal("formaPago");
                int oProductoReservado = dr.GetOrdinal("productoReservado");
                int oVendedor = dr.GetOrdinal("vendedor");
                int oMoneda = dr.GetOrdinal("moneda");

                while (await dr.ReadAsync(ct).ConfigureAwait(false))
                {
                    lista.Add(MapearPagoParcial(
                        dr, idNegocio,
                        oIdPagoParcial, oIdCliente, oNombreCliente, oMonto, oIdVenta,
                        oNumeroVenta, oFechaRegistro, oEstado, oFormaPago, oProductoReservado,
                        oVendedor, oMoneda));
                }
            }
            catch
            {
                return new List<PagoParcial>();
            }

            return lista;
        }

        public async Task<bool> DarDeBajaPagoParcialAsync(int idPagoParcial, int idVenta, CancellationToken ct = default)
        {
            const string query = @"
UPDATE PAGOPARCIAL
SET estado = 0, idVenta = @idVenta
WHERE idPagoParcial = @idPagoParcial;";

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@idPagoParcial", SqlDbType.Int).Value = idPagoParcial;
                cmd.Parameters.Add("@idVenta", SqlDbType.Int).Value = idVenta;

                await cn.OpenAsync(ct).ConfigureAwait(false);
                var rows = await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
                return rows > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(int idGenerado, string mensaje)> RegistrarPagoParcialAsync(PagoParcial obj)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_REGISTRARPAGOPARCIAL", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = obj.IdCliente;
                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = obj.Monto;
                cmd.Parameters.Add("@moneda", SqlDbType.VarChar, 10).Value = obj.Moneda ?? "";
                cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = obj.Estado;
                cmd.Parameters.Add("@formaPago", SqlDbType.VarChar, 50).Value = obj.FormaPago ?? "";
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = obj.FechaRegistro;
                cmd.Parameters.Add("@idNegocio", SqlDbType.Int).Value = obj.IdNegocio;
                cmd.Parameters.Add("@productoReservado", SqlDbType.VarChar, 200).Value = obj.ProductoReservado ?? "";
                cmd.Parameters.Add("@vendedor", SqlDbType.VarChar, 100).Value = obj.Vendedor ?? "";

                var pResultado = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                pResultado.Direction = ParameterDirection.Output;

                var pMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500);
                pMensaje.Direction = ParameterDirection.Output;

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(pResultado.Value ?? 0);
                mensaje = pMensaje.Value?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }

            return (idGenerado, mensaje);
        }



        public async Task<(bool ok, string mensaje)> ModificarPagoParcialAsync(PagoParcial obj)
        {
            bool ok = false;
            string mensaje = string.Empty;

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_MODIFICARPAGOPARCIAL", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@idPagoParcial", SqlDbType.Int).Value = obj.IdPagoParcial;
                cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = obj.Monto;
                cmd.Parameters.Add("@moneda", SqlDbType.VarChar, 10).Value = obj.Moneda ?? "";
                cmd.Parameters.Add("@estado", SqlDbType.Bit).Value = obj.Estado;
                cmd.Parameters.Add("@formaPago", SqlDbType.VarChar, 50).Value = obj.FormaPago ?? "";
                cmd.Parameters.Add("@idNegocio", SqlDbType.Int).Value = obj.IdNegocio;
                cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = obj.FechaRegistro;
                cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = obj.IdCliente;
                cmd.Parameters.Add("@productoReservado", SqlDbType.VarChar, 200).Value = obj.ProductoReservado ?? "";
                cmd.Parameters.Add("@vendedor", SqlDbType.VarChar, 100).Value = obj.Vendedor ?? "";

                var pResultado = cmd.Parameters.Add("@resultado", SqlDbType.Int);
                pResultado.Direction = ParameterDirection.Output;

                var pMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500);
                pMensaje.Direction = ParameterDirection.Output;

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                ok = Convert.ToBoolean(pResultado.Value ?? 0);
                mensaje = pMensaje.Value?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                ok = false;
                mensaje = ex.Message;
            }

            return (ok, mensaje);
        }



        public async Task<(bool ok, string mensaje)> EliminarPagoParcialAsync(int idPagoParcial)
        {
            bool ok = false;
            string mensaje = string.Empty;

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_ELIMINARPAGOPARCIAL", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@idPagoParcial", SqlDbType.Int).Value = idPagoParcial;

                var pRespuesta = cmd.Parameters.Add("@respuesta", SqlDbType.Bit);
                pRespuesta.Direction = ParameterDirection.Output;

                var pMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500);
                pMensaje.Direction = ParameterDirection.Output;

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                ok = Convert.ToBoolean(pRespuesta.Value ?? false);
                mensaje = pMensaje.Value?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                ok = false;
                mensaje = ex.Message;
            }

            return (ok, mensaje);
        }



        public async Task<List<PagoParcial>> ConsultarPagosParcialesPorClienteAsync(int idCliente, CancellationToken ct = default)
        {
            var lista = new List<PagoParcial>();

            try
            {
                await using var cn = new SqlConnection(_cadenaConexion);
                await using var cmd = new SqlCommand("SP_CONSULTARPAGOPARCIAL", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente;

                await cn.OpenAsync(ct).ConfigureAwait(false);
                await using var dr = await cmd.ExecuteReaderAsync(ct).ConfigureAwait(false);

                int oIdPagoParcial = dr.GetOrdinal("idPagoParcial");
                int oIdCliente = dr.GetOrdinal("idCliente");
                int oMonto = dr.GetOrdinal("monto");
                int oFechaRegistro = dr.GetOrdinal("fechaRegistro");
                int oEstado = dr.GetOrdinal("estado");
                int oNombreCliente = dr.GetOrdinal("NombreCliente");
                int oNumeroVenta = dr.GetOrdinal("NumeroVenta");
                int oProductoReservado = dr.GetOrdinal("productoReservado");
                int oVendedor = dr.GetOrdinal("vendedor");
                int oMoneda = dr.GetOrdinal("moneda");
                int oFormaPago = dr.GetOrdinal("formaPago");

                while (await dr.ReadAsync(ct).ConfigureAwait(false))
                {
                    bool estado = dr.GetBoolean(oEstado);
                    if (!estado) continue; // sólo pendientes (true)

                    lista.Add(new PagoParcial
                    {
                        IdPagoParcial = dr.GetInt32(oIdPagoParcial),
                        IdCliente = dr.GetInt32(oIdCliente),
                        Monto = dr.GetDecimal(oMonto),
                        FechaRegistro = dr.GetDateTime(oFechaRegistro),
                        Estado = estado,
                        NombreCliente = dr.IsDBNull(oNombreCliente) ? "" : dr.GetString(oNombreCliente),
                        NumeroVenta = dr.IsDBNull(oNumeroVenta) ? "" : dr.GetString(oNumeroVenta),
                        ProductoReservado = dr.IsDBNull(oProductoReservado) ? "" : dr.GetString(oProductoReservado),
                        Vendedor = dr.IsDBNull(oVendedor) ? "" : dr.GetString(oVendedor),
                        Moneda = dr.IsDBNull(oMoneda) ? "" : dr.GetString(oMoneda),
                        FormaPago = dr.IsDBNull(oFormaPago) ? "" : dr.GetString(oFormaPago),
                    });
                }
            }
            catch
            {
                return new List<PagoParcial>();
            }

            return lista;
        }

    }
}
