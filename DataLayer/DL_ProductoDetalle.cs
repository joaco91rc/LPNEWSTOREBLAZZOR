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
    public class DL_ProductoDetalle
    {
        private readonly string _cadenaConexion;

        public DL_ProductoDetalle(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }
        public async Task<List<ProductoDetalle>> ListarProductosConSerialNumberByIDNegocioAsync(int idProducto, int idNegocio)
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("WHERE PD.idProducto = @idProducto AND PD.estado = 1 AND PD.idNegocio = @idNegocio");
                    query.AppendLine("AND P.prodSerializable = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"]),
                                Fecha = Convert.ToDateTime(dr["fecha"])
                            };

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }

        public async Task<List<ProductoDetalle>> ListarProductosConSerialNumberByIDAsync(int idProducto)
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("WHERE PD.idProducto = @idProducto AND PD.estado = 1");
                    query.AppendLine("AND P.prodSerializable = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);

                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"]),
                                Fecha = Convert.ToDateTime(dr["fecha"])
                            };

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }

        public async Task<List<ProductoDetalle>> ListarProductosConSerialNumberEnStockTodosLocalesAsync()
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre, PR.idProveedor, PR.razonSocial AS nombreProveedor");
                    query.AppendLine("FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("INNER JOIN PROVEEDOR PR ON PD.idProveedor = PR.idProveedor");
                    query.AppendLine("WHERE PD.estado = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"]),
                                EstadoVendido = Convert.ToBoolean(dr["estado"]) == true ? "EN STOCK" : "VENDIDO",
                                Fecha = Convert.ToDateTime(dr["fecha"])
                            };

                            // Validar si el idProveedor o nombreProveedor son NULL
                            detalle.IdProveedor = dr["idProveedor"] != DBNull.Value ? Convert.ToInt32(dr["idProveedor"]) : 0;
                            detalle.NombreProveedor = dr["nombreProveedor"] != DBNull.Value ? dr["nombreProveedor"].ToString() : string.Empty;

                            // Asignar nombreLocal basado en el idNegocio
                            switch (detalle.IdNegocio)
                            {
                                case 1:
                                    detalle.NombreLocal = "HITECH 1";
                                    break;
                                case 2:
                                    detalle.NombreLocal = "HITECH 2";
                                    break;
                                case 3:
                                    detalle.NombreLocal = "APPLE 49";
                                    break;
                                case 4:
                                    detalle.NombreLocal = "APPLE CAFÉ";
                                    break;
                                default:
                                    detalle.NombreLocal = "";
                                    break;
                            }

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }



        public async Task<List<ProductoDetalle>> ListarProductosConSerialNumberAsync()
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"]),
                                EstadoVendido = Convert.ToBoolean(dr["estado"]) == true ? "EN STOCK" : "VENDIDO",
                                Fecha = Convert.ToDateTime(dr["fecha"])
                            };

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }

        public async Task<List<ProductoDetalle>> ListarProductosConSerialNumberPorLocalDisponiblesAsync(int idNegocio)
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre, PR.razonSocial AS nombreProveedor");
                    query.AppendLine("FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("INNER JOIN PROVEEDOR PR ON PD.idProveedor = PR.idProveedor");
                    query.AppendLine("WHERE PD.idNegocio = @idNegocio AND PD.estado = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Estado = Convert.ToBoolean(dr["estado"]),
                                EstadoVendido = Convert.ToBoolean(dr["estado"]) == true ? "EN STOCK" : "VENDIDO",
                                Fecha = Convert.ToDateTime(dr["fecha"])
                            };

                            // Validar si el idProveedor o nombreProveedor son NULL
                            detalle.IdProveedor = dr["idProveedor"] != DBNull.Value ? Convert.ToInt32(dr["idProveedor"]) : 0;
                            detalle.NombreProveedor = dr["nombreProveedor"] != DBNull.Value ? dr["nombreProveedor"].ToString() : string.Empty;

                            // Asignar nombreLocal basado en el idNegocio
                            switch (detalle.IdNegocio)
                            {
                                case 1:
                                    detalle.NombreLocal = "HITECH 1";
                                    break;
                                case 2:
                                    detalle.NombreLocal = "HITECH 2";
                                    break;
                                case 3:
                                    detalle.NombreLocal = "APPLE 49";
                                    break;
                                case 4:
                                    detalle.NombreLocal = "APPLE CAFÉ";
                                    break;
                                default:
                                    detalle.NombreLocal = "";
                                    break;
                            }

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }

        public async Task<List<ProductoDetalle>> ListarProductosVendidosAsync(int idNegocio)
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre, V.nroDocumento, PR.idProveedor, PR.razonSocial AS nombreProveedor FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("INNER JOIN VENTA V ON PD.idVenta = V.idVenta");
                    query.AppendLine("INNER JOIN PROVEEDOR PR ON PD.idProveedor = PR.idProveedor");
                    query.AppendLine("WHERE PD.estado = 0 AND PD.idVenta <> 0 AND PD.idNegocio = @idNegocio");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                NumeroVenta = dr["nroDocumento"].ToString(),
                                Fecha = Convert.ToDateTime(dr["fecha"]),

                                // Manejo de DBNull para fechaEgreso
                                FechaEgreso = dr.IsDBNull(dr.GetOrdinal("fechaEgreso"))
                                              ? (DateTime?)null
                                              : Convert.ToDateTime(dr["fechaEgreso"])
                            };

                            // Manejo de DBNull para idProveedor y nombreProveedor
                            detalle.IdProveedor = dr["idProveedor"] != DBNull.Value ? Convert.ToInt32(dr["idProveedor"]) : 0;
                            detalle.NombreProveedor = dr["nombreProveedor"] != DBNull.Value ? dr["nombreProveedor"].ToString() : string.Empty;

                            // Asignar nombreLocal basado en el idNegocio
                            switch (detalle.IdNegocio)
                            {
                                case 1:
                                    detalle.NombreLocal = "HITECH 1";
                                    break;
                                case 2:
                                    detalle.NombreLocal = "HITECH 2";
                                    break;
                                case 3:
                                    detalle.NombreLocal = "APPLE 49";
                                    break;
                                case 4:
                                    detalle.NombreLocal = "APPLE CAFÉ";
                                    break;
                                default:
                                    detalle.NombreLocal = "";
                                    break;
                            }

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }



        public async Task<List<ProductoDetalle>> ListarProductosVendidosPorFechaAsync(int idNegocio, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre, V.nroDocumento FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("INNER JOIN VENTA V ON PD.idVenta = V.idVenta");
                    query.AppendLine("WHERE PD.estado = 0 AND PD.idVenta <> 0 AND PD.idNegocio = @idNegocio");
                    query.AppendLine("AND PD.fechaEgreso BETWEEN @fechaInicio AND @fechaFin");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                NumeroVenta = dr["nroDocumento"].ToString(),
                                Fecha = Convert.ToDateTime(dr["fecha"]),

                                // Manejo de DBNull para fechaEgreso
                                FechaEgreso = dr.IsDBNull(dr.GetOrdinal("fechaEgreso"))
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(dr["fechaEgreso"])
                            };

                            // Asignar nombreLocal basado en el idNegocio
                            switch (detalle.IdNegocio)
                            {
                                case 1:
                                    detalle.NombreLocal = "HITECH 1";
                                    break;
                                case 2:
                                    detalle.NombreLocal = "HITECH 2";
                                    break;
                                case 3:
                                    detalle.NombreLocal = "APPLE 49";
                                    break;
                                case 4:
                                    detalle.NombreLocal = "APPLE CAFÉ";
                                    break;
                                default:
                                    detalle.NombreLocal = "";
                                    break;
                            }

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }

            return lista;
        }

        public async Task<List<ProductoDetalle>> ListarProductosVendidosTodosLocalesAsync()
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre, V.nroDocumento, PR.idProveedor, PR.razonSocial AS nombreProveedor FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("INNER JOIN VENTA V ON PD.idVenta = V.idVenta");
                    query.AppendLine("INNER JOIN PROVEEDOR PR ON PD.idProveedor = PR.idProveedor");
                    query.AppendLine("WHERE PD.estado = 0 AND PD.idVenta <> 0");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                NumeroVenta = dr["nroDocumento"].ToString(),
                                Fecha = Convert.ToDateTime(dr["fecha"]),

                                // Manejo de DBNull para fechaEgreso
                                FechaEgreso = dr.IsDBNull(dr.GetOrdinal("fechaEgreso"))
                                              ? (DateTime?)null
                                              : Convert.ToDateTime(dr["fechaEgreso"])
                            };

                            // Manejo de DBNull para idProveedor y nombreProveedor
                            detalle.IdProveedor = dr["idProveedor"] != DBNull.Value ? Convert.ToInt32(dr["idProveedor"]) : 0;
                            detalle.NombreProveedor = dr["nombreProveedor"] != DBNull.Value ? dr["nombreProveedor"].ToString() : string.Empty;

                            // Asignar nombreLocal basado en el idNegocio
                            switch (detalle.IdNegocio)
                            {
                                case 1:
                                    detalle.NombreLocal = "HITECH 1";
                                    break;
                                case 2:
                                    detalle.NombreLocal = "HITECH 2";
                                    break;
                                case 3:
                                    detalle.NombreLocal = "APPLE 49";
                                    break;
                                case 4:
                                    detalle.NombreLocal = "APPLE CAFÉ";
                                    break;
                                default:
                                    detalle.NombreLocal = "";
                                    break;
                            }

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }

        public async Task<List<ProductoDetalle>> ListarProductosSerialesPorVentaAsync(int idVenta)
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT PD.*, P.codigo, P.nombre FROM PRODUCTO_DETALLE PD");
                    query.AppendLine("INNER JOIN PRODUCTO P ON PD.idProducto = P.idProducto");
                    query.AppendLine("WHERE PD.idVenta = @idVenta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);

                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ProductoDetalle detalle = new ProductoDetalle()
                            {
                                IdProductoDetalle = Convert.ToInt32(dr["idProductoDetalle"]),
                                IdProducto = Convert.ToInt32(dr["idProducto"]),
                                NumeroSerie = dr["numeroSerie"].ToString(),
                                Color = dr["color"].ToString(),
                                Modelo = dr["modelo"].ToString(),
                                Marca = dr["marca"].ToString(),
                                IdNegocio = Convert.ToInt32(dr["idNegocio"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Fecha = Convert.ToDateTime(dr["fecha"])
                            };

                            lista.Add(detalle);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ProductoDetalle>();
                }
            }
            return lista;
        }


        public async Task<int> ContarProductosSerializadosAsync(int idProducto, int idNegocio)
        {
            int cantidadSerializados = 0;
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("SELECT COUNT(*) FROM PRODUCTO_DETALLE");
                query.AppendLine("WHERE idProducto = @idProducto AND estado = 1 AND idNegocio = @idNegocio");

                SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                await oconexion.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                cantidadSerializados = Convert.ToInt32(result);
            }
            return cantidadSerializados;
        }

        public async Task<(int resultado, string mensaje)> RegistrarSerialNumberAsync(ProductoDetalle productoDetalle)
        {
            int resultado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARSERIALNUMBER ", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("idProducto", productoDetalle.IdProducto);
                    cmd.Parameters.AddWithValue("numeroSerie", productoDetalle.NumeroSerie);
                    cmd.Parameters.AddWithValue("color", productoDetalle.Color);
                    cmd.Parameters.AddWithValue("modelo", productoDetalle.Modelo);
                    cmd.Parameters.AddWithValue("marca", productoDetalle.Marca);
                    cmd.Parameters.AddWithValue("idNegocio", productoDetalle.IdNegocio);
                    cmd.Parameters.AddWithValue("idVenta", productoDetalle.IdVenta ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("fecha", productoDetalle.Fecha);
                    cmd.Parameters.AddWithValue("fechaEgreso", productoDetalle.FechaEgreso ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("idProveedor", productoDetalle.IdProveedor);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    resultado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<(int resultado, string mensaje)> DesactivarProductoDetalleAsync(int idProductoDetalle, int idVenta)
        {
            int resultado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    string query = "UPDATE PRODUCTO_DETALLE SET estado = 0, idVenta = @idVenta, fechaEgreso = @fechaEgreso WHERE idProductoDetalle = @idProductoDetalle";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idProductoDetalle", idProductoDetalle);
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);
                    cmd.Parameters.AddWithValue("@fechaEgreso", DateTime.Now);

                    await oconexion.OpenAsync();
                    resultado = await cmd.ExecuteNonQueryAsync();

                    if (resultado > 0)
                    {
                        mensaje = "Producto desactivado correctamente.";
                    }
                    else
                    {
                        mensaje = "No se pudo desactivar el producto. Verifique si el ID es correcto.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                mensaje = "Error: " + ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<(int resultado, string mensaje)> ActivarProductoDetalleAsync(int idProductoDetalle)
        {
            int resultado = 0;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    string query = "UPDATE PRODUCTO_DETALLE SET estado = 1, idVenta = 0 WHERE idProductoDetalle = @idProductoDetalle";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idProductoDetalle", idProductoDetalle);

                    await oconexion.OpenAsync();
                    resultado = await cmd.ExecuteNonQueryAsync();

                    if (resultado > 0)
                    {
                        mensaje = "Producto activado correctamente.";
                    }
                    else
                    {
                        mensaje = "No se pudo activar el producto. Verifique si el ID es correcto.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                mensaje = "Error: " + ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<(bool respuesta, string mensaje)> EditarSerialNumberAsync(ProductoDetalle productoDetalle)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITARSERIALNUMBER", oconexion);
                    cmd.Parameters.AddWithValue("idProductoDetalle", productoDetalle.IdProductoDetalle);
                    cmd.Parameters.AddWithValue("idProducto", productoDetalle.IdProducto);
                    cmd.Parameters.AddWithValue("numeroSerie", productoDetalle.NumeroSerie);
                    cmd.Parameters.AddWithValue("color", productoDetalle.Color);
                    cmd.Parameters.AddWithValue("modelo", productoDetalle.Modelo);
                    cmd.Parameters.AddWithValue("marca", productoDetalle.Marca);
                    cmd.Parameters.AddWithValue("idNegocio", productoDetalle.IdNegocio);
                    cmd.Parameters.AddWithValue("fecha", productoDetalle.Fecha);
                    cmd.Parameters.AddWithValue("idProveedor", productoDetalle.IdProveedor);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }

            return (respuesta, mensaje);
        }

        public async Task<(bool respuesta, string mensaje)> TraspasarSerialNumberAsync(ProductoDetalle productoDetalle)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    string query = "UPDATE PRODUCTO_DETALLE SET idNegocio = @idNegocio, estado = 1 WHERE numeroSerie = @numeroSerie";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", productoDetalle.IdNegocio);
                    cmd.Parameters.AddWithValue("@numeroSerie", productoDetalle.NumeroSerie);

                    await oconexion.OpenAsync();

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        respuesta = true;
                        mensaje = "El idNegocio se actualizó correctamente.";
                    }
                    else
                    {
                        respuesta = false;
                        mensaje = "No se encontró un registro con el número de serie especificado.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }

            return (respuesta, mensaje);
        }


        public async Task<(bool respuesta, string mensaje)> ActualizarSerialNumberTrasasadoAsync(ProductoDetalle productoDetalle)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    string query = "UPDATE PRODUCTO_DETALLE SET estado = @estado WHERE numeroSerie = @numeroSerie";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@estado", productoDetalle.Estado);
                    cmd.Parameters.AddWithValue("@numeroSerie", productoDetalle.NumeroSerie);

                    await oconexion.OpenAsync();

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        respuesta = true;
                        mensaje = "El estado se actualizó correctamente.";
                    }
                    else
                    {
                        respuesta = false;
                        mensaje = "No se encontró un registro con el número de serie especificado.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }

            return (respuesta, mensaje);
        }

        public async Task<(bool respuesta, string mensaje)> EliminarSerialNumberAsync(ProductoDetalle productoDetalle)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARSERIALNUMBER", oconexion);
                    cmd.Parameters.AddWithValue("idProducto", productoDetalle.IdProducto);
                    cmd.Parameters.AddWithValue("numeroSerie", productoDetalle.NumeroSerie);
                    cmd.Parameters.AddWithValue("idNegocio", productoDetalle.IdNegocio);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
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
