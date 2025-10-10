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
    public class DL_Compra
    {
        private readonly string _cadenaConexion;


        public DL_Compra(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;

        }


        public async Task<int> ObtenerCorrelativo()
        {
            int idCorrelativo = 0;
            using SqlConnection oconexion = new(_cadenaConexion);
            try
            {
                const string query = "SELECT COUNT(*) + 1 FROM COMPRA";
                using SqlCommand cmd = new(query, oconexion) { CommandType = CommandType.Text };
                await oconexion.OpenAsync();
                object? scalar = await cmd.ExecuteScalarAsync();
                idCorrelativo = Convert.ToInt32(scalar);
            }
            catch
            {
                idCorrelativo = 0;
            }
            return idCorrelativo;
        }

        // (exito, mensaje, idCompraSalida)
        public async Task<(bool exito, string mensaje, int idCompraSalida)> Registrar(Compra objCompra, DataTable detalleCompra)
        {
            bool exito = false;
            string mensaje = string.Empty;
            int idCompraSalida = 0;

            try
            {
                using SqlConnection oconexion = new(_cadenaConexion);
                using SqlCommand cmd = new("SP_REGISTRARCOMPRA", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idUsuario", objCompra.oUsuario.Id);
                cmd.Parameters.AddWithValue("idNegocio", objCompra.IdNegocio);
                cmd.Parameters.AddWithValue("idProveedor", objCompra.oProveedor.IdProveedor);
                cmd.Parameters.AddWithValue("tipoDocumento", (object?)objCompra.TipoDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("nroDocumento", (object?)objCompra.NroDocumento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("montoTotal", objCompra.montoTotal);

                // Detalle (si tu SP espera TVP, marcamos Structured)
                var pDetalle = cmd.Parameters.AddWithValue("detalleCompra", (object?)detalleCompra ?? DBNull.Value);
                if (detalleCompra != null) pDetalle.SqlDbType = SqlDbType.Structured;

                cmd.Parameters.AddWithValue("observaciones", (object?)objCompra.Observaciones ?? DBNull.Value);

                // Nuevos campos
                cmd.Parameters.AddWithValue("formaPago", (object?)objCompra.FormaPago ?? DBNull.Value);
                cmd.Parameters.AddWithValue("formaPago2", (object?)objCompra.FormaPago2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("formaPago3", (object?)objCompra.FormaPago3 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("formaPago4", (object?)objCompra.FormaPago4 ?? DBNull.Value);

                cmd.Parameters.AddWithValue("montoFP1", objCompra.MontoFP1);
                cmd.Parameters.AddWithValue("montoFP2", objCompra.MontoFP2);
                cmd.Parameters.AddWithValue("montoFP3", objCompra.MontoFP3);
                cmd.Parameters.AddWithValue("montoFP4", objCompra.MontoFP4);

                cmd.Parameters.AddWithValue("montoPago", objCompra.MontoPago);
                cmd.Parameters.AddWithValue("montoPagoFP2", objCompra.MontoPagoFP2);
                cmd.Parameters.AddWithValue("montoPagoFP3", objCompra.MontoPagoFP3);
                cmd.Parameters.AddWithValue("montoPagoFP4", objCompra.MontoPagoFP4);

                // Outputs
                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("idCompraSalida", SqlDbType.Int).Direction = ParameterDirection.Output;

                await oconexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                exito = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value?.ToString() ?? string.Empty;
                idCompraSalida = Convert.ToInt32(cmd.Parameters["idCompraSalida"].Value);
            }
            catch (Exception ex)
            {
                exito = false;
                mensaje = ex.Message;
                idCompraSalida = 0;
            }

            return (exito, mensaje, idCompraSalida);
        }

        public async Task<List<Compra>> ObtenerComprasConDetalleEntreFechas(int idNegocio, DateTime fechaInicio, DateTime fechaFin)
        {
            List<Compra> listaCompras = new();

            using SqlConnection conexion = new(_cadenaConexion);
            try
            {
                await conexion.OpenAsync();

                StringBuilder query = new();
                query.AppendLine("SELECT C.idCompra,")
                     .AppendLine("U.nombreCompleto,")
                     .AppendLine("PR.documento, PR.razonSocial,")
                     .AppendLine("C.tipoDocumento, C.idNegocio, C.nroDocumento, C.montoTotal, CONVERT(CHAR(10), C.fechaRegistro, 103) [fechaRegistro],")
                     .AppendLine("C.formaPago, C.formaPago2, C.formaPago3, C.formaPago4,")
                     .AppendLine("C.montoFP1, C.montoFP2, C.montoFP3, C.montoFP4,")
                     .AppendLine("C.montoPago, C.montoPagoFP2, C.montoPagoFP3, C.montoPagoFP4")
                     .AppendLine("FROM COMPRA C")
                     .AppendLine("INNER JOIN USUARIO U ON U.idUsuario = C.idUsuario")
                     .AppendLine("INNER JOIN PROVEEDOR PR ON PR.idProveedor = C.idProveedor")
                     .AppendLine("WHERE C.idNegocio = @idNegocio")
                     .AppendLine("AND C.fechaRegistro BETWEEN @fechaInicio AND @fechaFin");

                using SqlCommand cmd = new(query.ToString(), conexion) { CommandType = CommandType.Text };
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    Compra compra = new()
                    {
                        IdCompra = Convert.ToInt32(dr["idCompra"]),
                        IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                        oUsuario = new Usuario { NombreCompleto = dr["nombreCompleto"].ToString() },
                        oProveedor = new Proveedor
                        {
                            Documento = dr["documento"].ToString(),
                            RazonSocial = dr["razonSocial"].ToString()
                        },
                        TipoDocumento = dr["tipoDocumento"].ToString(),
                        NroDocumento = dr["nroDocumento"].ToString(),
                        montoTotal = Convert.ToDecimal(dr["montoTotal"]),
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"].ToString()),
                        FormaPago = dr["formaPago"].ToString(),
                        FormaPago2 = dr["formaPago2"].ToString(),
                        FormaPago3 = dr["formaPago3"].ToString(),
                        FormaPago4 = dr["formaPago4"].ToString(),
                        MontoFP1 = Convert.ToDecimal(dr["montoFP1"]),
                        MontoFP2 = Convert.ToDecimal(dr["montoFP2"]),
                        MontoFP3 = Convert.ToDecimal(dr["montoFP3"]),
                        MontoFP4 = Convert.ToDecimal(dr["montoFP4"]),
                        MontoPago = Convert.ToDecimal(dr["montoPago"]),
                        MontoPagoFP2 = Convert.ToDecimal(dr["montoPagoFP2"]),
                        MontoPagoFP3 = Convert.ToDecimal(dr["montoPagoFP3"]),
                        MontoPagoFP4 = Convert.ToDecimal(dr["montoPagoFP4"])
                    };

                    compra.ODetalleCompra = await ObtenerDetalleCompra(compra.IdCompra);
                    listaCompras.Add(compra);
                }
            }
            catch
            {
                listaCompras = new();
            }

            return listaCompras;
        }

        public async Task<List<Compra>> ObtenerComprasConDetalle(int idNegocio)
        {
            List<Compra> listaCompras = new();

            using SqlConnection conexion = new(_cadenaConexion);
            try
            {
                await conexion.OpenAsync();

                StringBuilder query = new();
                query.AppendLine("SELECT C.idCompra,")
                     .AppendLine("U.nombreCompleto,")
                     .AppendLine("PR.documento, PR.razonSocial,")
                     .AppendLine("C.tipoDocumento, C.idNegocio, C.nroDocumento, C.montoTotal, CONVERT(CHAR(10), C.fechaRegistro, 103) [fechaRegistro],")
                     .AppendLine("C.formaPago, C.formaPago2, C.formaPago3, C.formaPago4,")
                     .AppendLine("C.montoFP1, C.montoFP2, C.montoFP3, C.montoFP4,")
                     .AppendLine("C.montoPago, C.montoPagoFP2, C.montoPagoFP3, C.montoPagoFP4")
                     .AppendLine("FROM COMPRA C")
                     .AppendLine("INNER JOIN USUARIO U ON U.idUsuario = C.idUsuario")
                     .AppendLine("INNER JOIN PROVEEDOR PR ON PR.idProveedor = C.idProveedor")
                     .AppendLine("WHERE C.idNegocio = @idNegocio");

                using SqlCommand cmd = new(query.ToString(), conexion) { CommandType = CommandType.Text };
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    Compra compra = new()
                    {
                        IdCompra = Convert.ToInt32(dr["idCompra"]),
                        IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                        oUsuario = new Usuario { NombreCompleto = dr["nombreCompleto"].ToString() },
                        oProveedor = new Proveedor
                        {
                            Documento = dr["documento"].ToString(),
                            RazonSocial = dr["razonSocial"].ToString()
                        },
                        TipoDocumento = dr["tipoDocumento"].ToString(),
                        NroDocumento = dr["nroDocumento"].ToString(),
                        montoTotal = Convert.ToDecimal(dr["montoTotal"]),
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        FormaPago = dr["formaPago"].ToString(),
                        FormaPago2 = dr["formaPago2"].ToString(),
                        FormaPago3 = dr["formaPago3"].ToString(),
                        FormaPago4 = dr["formaPago4"].ToString(),
                        MontoFP1 = Convert.ToDecimal(dr["montoFP1"]),
                        MontoFP2 = Convert.ToDecimal(dr["montoFP2"]),
                        MontoFP3 = Convert.ToDecimal(dr["montoFP3"]),
                        MontoFP4 = Convert.ToDecimal(dr["montoFP4"]),
                        MontoPago = Convert.ToDecimal(dr["montoPago"]),
                        MontoPagoFP2 = Convert.ToDecimal(dr["montoPagoFP2"]),
                        MontoPagoFP3 = Convert.ToDecimal(dr["montoPagoFP3"]),
                        MontoPagoFP4 = Convert.ToDecimal(dr["montoPagoFP4"])
                    };

                    compra.ODetalleCompra = await ObtenerDetalleCompra(compra.IdCompra);
                    listaCompras.Add(compra);
                }
            }
            catch
            {
                listaCompras = new();
            }

            return listaCompras;
        }

        public async Task<Compra> ObtenerCompra(string numero, int idNegocio)
        {
            Compra objCompra = new();

            using SqlConnection oconexion = new(_cadenaConexion);
            try
            {
                StringBuilder query = new();
                query.AppendLine("SELECT C.idCompra,")
                     .AppendLine("U.nombreCompleto,")
                     .AppendLine("PR.documento, PR.razonSocial,")
                     .AppendLine("C.tipoDocumento, C.idNegocio, C.nroDocumento, C.montoTotal, CONVERT(CHAR(10), C.fechaRegistro, 103) [fechaRegistro],")
                     .AppendLine("C.formaPago, C.formaPago2, C.formaPago3, C.formaPago4,")
                     .AppendLine("C.montoFP1, C.montoFP2, C.montoFP3, C.montoFP4,")
                     .AppendLine("C.montoPago, C.montoPagoFP2, C.montoPagoFP3, C.montoPagoFP4,")
                     .AppendLine("C.cotizacionDolar ")
                     .AppendLine("FROM COMPRA C")
                     .AppendLine("INNER JOIN USUARIO U ON U.idUsuario = C.idUsuario")
                     .AppendLine("INNER JOIN PROVEEDOR PR ON PR.idProveedor = C.idProveedor")
                     .AppendLine("WHERE C.nroDocumento = @numero AND C.idNegocio = @idNegocio");

                using SqlCommand cmd = new(query.ToString(), oconexion) { CommandType = CommandType.Text };
                cmd.Parameters.AddWithValue("@numero", numero);
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                await oconexion.OpenAsync();
                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    objCompra = new Compra
                    {
                        IdCompra = Convert.ToInt32(dr["idCompra"]),
                        IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                        oUsuario = new Usuario { NombreCompleto = dr["nombreCompleto"].ToString() },
                        oProveedor = new Proveedor
                        {
                            Documento = dr["documento"].ToString(),
                            RazonSocial = dr["razonSocial"].ToString()
                        },
                        TipoDocumento = dr["tipoDocumento"].ToString(),
                        NroDocumento = dr["nroDocumento"].ToString(),
                        montoTotal = Convert.ToDecimal(dr["montoTotal"]),
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"].ToString()),
                        FormaPago = dr["formaPago"].ToString(),
                        FormaPago2 = dr["formaPago2"].ToString(),
                        FormaPago3 = dr["formaPago3"].ToString(),
                        FormaPago4 = dr["formaPago4"].ToString(),
                        MontoFP1 = Convert.ToDecimal(dr["montoFP1"]),
                        MontoFP2 = Convert.ToDecimal(dr["montoFP2"]),
                        MontoFP3 = Convert.ToDecimal(dr["montoFP3"]),
                        MontoFP4 = Convert.ToDecimal(dr["montoFP4"]),
                        MontoPago = Convert.ToDecimal(dr["montoPago"]),
                        MontoPagoFP2 = Convert.ToDecimal(dr["montoPagoFP2"]),
                        MontoPagoFP3 = Convert.ToDecimal(dr["montoPagoFP3"]),
                        MontoPagoFP4 = Convert.ToDecimal(dr["montoPagoFP4"]),
                        cotizacionDolar = dr["cotizacionDolar"] != DBNull.Value ? Convert.ToDecimal(dr["cotizacionDolar"]) : 0
                    };
                }
            }
            catch
            {
                objCompra = new();
            }

            return objCompra;
        }

        public async Task<List<DetalleCompra>> ObtenerDetalleCompra(int idCompra)
        {
            List<DetalleCompra> lista = new();

            try
            {
                using SqlConnection conexion = new(_cadenaConexion);
                await conexion.OpenAsync();

                StringBuilder query = new();
                query.AppendLine("SELECT P.idProducto, P.nombre, DC.precioCompra, DC.cantidad, DC.montoTotal, P.productoDolar")
                     .AppendLine("FROM DETALLE_COMPRA DC")
                     .AppendLine("INNER JOIN PRODUCTO P ON P.idProducto = DC.idProducto")
                     .AppendLine("WHERE DC.idCompra = @idCompra");

                using SqlCommand cmd = new(query.ToString(), conexion) { CommandType = CommandType.Text };
                cmd.Parameters.AddWithValue("@idCompra", idCompra);

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new DetalleCompra
                    {
                        OProducto = new Producto
                        {
                            IdProducto = Convert.ToInt32(dr["idProducto"]),
                            Nombre = dr["nombre"].ToString(),
                            ProductoDolar = Convert.ToBoolean(dr["productoDolar"])
                        },
                        PrecioCompra = Convert.ToDecimal(dr["precioCompra"]),
                        Cantidad = Convert.ToInt32(dr["cantidad"]),
                        MontoTotal = Convert.ToDecimal(dr["montoTotal"])
                    });
                }
            }
            catch
            {
                lista = new();
            }

            return lista;
        }

        // (exito, mensaje)
        public async Task<(bool exito, string mensaje)> EliminarCompraConDetalle(int idCompra)
        {
            bool exito = false;
            string mensaje = string.Empty;

            using SqlConnection conexion = new(_cadenaConexion);
            await conexion.OpenAsync();
            using SqlTransaction tx = conexion.BeginTransaction();

            try
            {
                using (SqlCommand deleteDetalleCmd = new(
                    "DELETE FROM DETALLE_COMPRA WHERE idCompra = @idCompra", conexion, tx))
                {
                    deleteDetalleCmd.Parameters.AddWithValue("@idCompra", idCompra);
                    await deleteDetalleCmd.ExecuteNonQueryAsync();
                }

                using (SqlCommand deleteCompraCmd = new(
                    "DELETE FROM COMPRA WHERE idCompra = @idCompra", conexion, tx))
                {
                    deleteCompraCmd.Parameters.AddWithValue("@idCompra", idCompra);
                    await deleteCompraCmd.ExecuteNonQueryAsync();
                }

                tx.Commit();
                exito = true;
            }
            catch (Exception ex)
            {
                tx.Rollback();
                exito = false;
                mensaje = ex.Message;
            }

            return (exito, mensaje);
        }
    }

}

