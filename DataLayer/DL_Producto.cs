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
    public class DL_Producto
    {
        private readonly string _cadenaConexion;

        public DL_Producto(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<decimal> ObtenerCostoProducto(int idProducto)
        {
            decimal costo = 0;
            using SqlConnection conn = new SqlConnection(_cadenaConexion);
            try
            {
                var query = new StringBuilder();
                query.AppendLine("SELECT precioCompra");
                query.AppendLine("FROM PRECIO_PRODUCTO");
                query.AppendLine("WHERE idProducto = @IdProducto AND idMoneda = 2");

                using SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                await conn.OpenAsync();
                object result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                    costo = Convert.ToDecimal(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el costo del producto: {ex.Message}");
            }
            return costo;
        }

        public async Task<(bool, string)> Eliminar(Producto producto)
        {
            string mensaje = string.Empty;
            bool respuesta = false;
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("SP_ELIMINARPRODUCTO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("idProducto", producto.IdProducto);
                cmd.Parameters.Add("respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                respuesta = Convert.ToBoolean(cmd.Parameters["respuesta"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return (respuesta, mensaje);
        }

        public async Task<(bool, string)> DarBajaLogica(int idProducto)
        {
            string mensaje = string.Empty;
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                var query = "UPDATE Producto SET estado = 0 WHERE idProducto = @idProducto";

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);

                await conn.OpenAsync();
                int filas = await cmd.ExecuteNonQueryAsync();

                return (filas > 0, filas > 0 ? string.Empty : "No se pudo eliminar el producto.");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<bool> ActualizarProductoDolar(int idProducto, bool productoDolar)
        {
            string query = "UPDATE PRODUCTO SET productoDolar = @productoDolar WHERE idProducto = @idProducto";
            using SqlConnection conn = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@productoDolar", productoDolar);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<(bool, string)> Editar(Producto producto)
        {
            string mensaje = string.Empty;
            bool respuesta = false;

            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("SP_EDITARPRODUCTO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("idProducto", producto.IdProducto);
                cmd.Parameters.AddWithValue("codigo", producto.Codigo);
                cmd.Parameters.AddWithValue("precioLista", producto.PrecioLista);
                cmd.Parameters.AddWithValue("nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("idCategoria", producto.OCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("estado", producto.Estado);
                cmd.Parameters.AddWithValue("costoPesos", producto.CostoPesos);
                cmd.Parameters.AddWithValue("precioCompra", producto.PrecioCompra);
                cmd.Parameters.AddWithValue("precioVenta", producto.PrecioVenta);
                cmd.Parameters.AddWithValue("ventaPesos", producto.VentaPesos);
                cmd.Parameters.AddWithValue("prodSerializable", producto.ProdSerializable);
                cmd.Parameters.AddWithValue("productoDolar", producto.ProductoDolar);

                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return (respuesta, mensaje);
        }

        public async Task<Producto> ObtenerProductoPorId(int idProducto)
        {
            Producto producto = null;
            using SqlConnection conn = new SqlConnection(_cadenaConexion);
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("SELECT p.idProducto, p.costoPesos, p.codigo, p.nombre, p.descripcion, ");
                query.AppendLine("c.idCategoria, c.descripcion AS DescripcionCategoria, p.stock, ");
                query.AppendLine("p.precioCompra, p.precioVenta, p.estado, p.prodSerializable, ");
                query.AppendLine("p.ventaPesos, p.productoDolar FROM Producto p ");
                query.AppendLine("INNER JOIN CATEGORIA c ON c.idCategoria = p.idCategoria ");
                query.AppendLine("WHERE p.idProducto = @idProducto");

                using SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);

                await conn.OpenAsync();
                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    producto = new Producto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Codigo = dr["codigo"].ToString(),
                        Nombre = dr["nombre"].ToString(),
                        Descripcion = dr["descripcion"].ToString(),
                        OCategoria = new Categoria
                        {
                            IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                            Descripcion = dr["DescripcionCategoria"].ToString()
                        },
                        CostoPesos = dr["costoPesos"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["costoPesos"]),
                        PrecioCompra = dr["precioCompra"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["precioCompra"]),
                        PrecioVenta = dr["precioVenta"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["precioVenta"]),
                        Estado = Convert.ToBoolean(dr["estado"]),
                        ProductoDolar = Convert.ToBoolean(dr["productoDolar"]),
                        ProdSerializable = Convert.ToBoolean(dr["prodSerializable"]),
                        VentaPesos = dr["ventaPesos"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["ventaPesos"])
                    };
                }
            }
            catch
            {
                producto = null;
            }
            return producto;
        }

        public async Task<(int, string)> Registrar(Producto producto)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;
            try
            {
                using SqlConnection conn = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("SP_REGISTRARPRODUCTO", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("codigo", producto.Codigo);
                cmd.Parameters.AddWithValue("nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("idCategoria", producto.OCategoria.IdCategoria);
                cmd.Parameters.AddWithValue("prodSerializable", producto.ProdSerializable);
                cmd.Parameters.AddWithValue("productoDolar", producto.ProductoDolar);
                cmd.Parameters.AddWithValue("estado", producto.Estado);

                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
            }
            return (idGenerado, mensaje);
        }



        public async Task<(bool Exito, string Mensaje)> SumarStockPorRMAAsync(int idProducto, int cantidad, int idNegocio)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    using SqlCommand cmd = new SqlCommand("SP_SUMARPRODUCTOXRMA", oconexion);
                    cmd.Parameters.AddWithValue("idProducto", idProducto);
                    cmd.Parameters.AddWithValue("cantidad", cantidad);
                    cmd.Parameters.AddWithValue("idNegocio", idNegocio);

                    cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    bool resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    string mensaje = cmd.Parameters["mensaje"].Value.ToString();

                    return (resultado, mensaje);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Exito, string Mensaje)> RestarStockPorRMAAsync(int idProducto, int cantidad, int idNegocio)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    using SqlCommand cmd = new SqlCommand("SP_EDITARPRODUCTOXRMA", oconexion);
                    cmd.Parameters.AddWithValue("idProducto", idProducto);
                    cmd.Parameters.AddWithValue("cantidad", cantidad);
                    cmd.Parameters.AddWithValue("idNegocio", idNegocio);

                    cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    bool resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    string mensaje = cmd.Parameters["mensaje"].Value.ToString();

                    return (resultado, mensaje);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<List<Producto>> ListarPorNegocioAsync(int idNegocio)
        {
            var lista = new List<Producto>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.idProducto, p.codigo, p.nombre, p.descripcion,");
                    query.AppendLine("c.idCategoria, c.descripcion AS DescripcionCategoria,");
                    query.AppendLine("ISNULL(pn.stock, 0) AS stock,");
                    query.AppendLine("ISNULL(ppDolar.precioCompra, 0) AS precioCompra,");
                    query.AppendLine("ISNULL(ppDolar.precioVenta, 0) AS precioVenta,");
                    query.AppendLine("ISNULL(ppPesos.precioVenta, 0) AS ventaPesos,");
                    query.AppendLine("CASE WHEN p.productoDolar = 1 THEN ISNULL(ppDolar.precioLista, 0) ELSE ISNULL(ppPesos.precioLista, 0) END AS precioLista,");
                    query.AppendLine("ISNULL(ppPesos.precioCompra, 0) AS costoPesos,");
                    query.AppendLine("p.prodSerializable, p.estado, p.productoDolar");
                    query.AppendLine("FROM Producto p");
                    query.AppendLine("INNER JOIN CATEGORIA c ON c.idCategoria = p.idCategoria");
                    query.AppendLine("LEFT JOIN PRODUCTONEGOCIO pn ON pn.idProducto = p.idProducto AND pn.idNegocio = @idNegocio");
                    query.AppendLine("LEFT JOIN (");
                    query.AppendLine("    SELECT idProducto, precioCompra, precioVenta, precioLista FROM PRECIO_PRODUCTO WHERE idMoneda = 2");
                    query.AppendLine(") ppDolar ON ppDolar.idProducto = p.idProducto");
                    query.AppendLine("LEFT JOIN (");
                    query.AppendLine("    SELECT idProducto, precioVenta, precioLista, precioCompra FROM PRECIO_PRODUCTO WHERE idMoneda = 1");
                    query.AppendLine(") ppPesos ON ppPesos.idProducto = p.idProducto");
                    query.AppendLine("WHERE p.estado = 1");

                    using SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(dr["idProducto"]),
                            Codigo = dr["codigo"].ToString(),
                            Nombre = dr["nombre"].ToString(),
                            Descripcion = dr["descripcion"].ToString(),
                            OCategoria = new Categoria
                            {
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Descripcion = dr["DescripcionCategoria"].ToString()
                            },
                            CostoPesos = Convert.ToDecimal(dr["costoPesos"]),
                            PrecioCompra = Convert.ToDecimal(dr["precioCompra"]),
                            PrecioVenta = Convert.ToDecimal(dr["precioVenta"]),
                            Estado = Convert.ToBoolean(dr["estado"]),
                            Stock = Convert.ToInt32(dr["stock"]),
                            PrecioLista = dr["precioLista"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["precioLista"]),
                            ProdSerializable = Convert.ToBoolean(dr["prodSerializable"]),
                            VentaPesos = dr["ventaPesos"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["ventaPesos"]),
                            ProductoDolar = Convert.ToBoolean(dr["productoDolar"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Producto>();
                }
            }
            return lista;
        }




        public async Task<List<Producto>> ListarAsync()
        {
            var lista = new List<Producto>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.idProducto AS ProductoId, p.codigo, p.nombre, p.descripcion,");
                    query.AppendLine("c.idCategoria, c.descripcion AS DescripcionCategoria,");
                    query.AppendLine("ISNULL(SUM(pn.stock), 0) AS stock,");
                    query.AppendLine("ISNULL(SUM(CASE WHEN pn.idNegocio = 1 THEN pn.stock ELSE 0 END), 0) AS stockH1,");
                    query.AppendLine("ISNULL(SUM(CASE WHEN pn.idNegocio = 2 THEN pn.stock ELSE 0 END), 0) AS stockH2,");
                    query.AppendLine("ISNULL(SUM(CASE WHEN pn.idNegocio = 3 THEN pn.stock ELSE 0 END), 0) AS stockAS,");
                    query.AppendLine("ISNULL(SUM(CASE WHEN pn.idNegocio = 4 THEN pn.stock ELSE 0 END), 0) AS stockAC,");
                    query.AppendLine("ISNULL(ppDolar.precioCompra, 0) AS precioCompra,");
                    query.AppendLine("ISNULL(ppDolar.precioVenta, 0) AS precioVenta,");
                    query.AppendLine("ISNULL(ppPesos.precioVenta, 0) AS ventaPesos,");
                    query.AppendLine("ISNULL(ppPesos.precioLista, 0) AS precioLista,");
                    query.AppendLine("ISNULL(ppPesos.precioCompra, 0) AS costoPesos,");
                    query.AppendLine("p.prodSerializable");
                    query.AppendLine("FROM Producto p");
                    query.AppendLine("INNER JOIN CATEGORIA c ON c.idCategoria = p.idCategoria");
                    query.AppendLine("LEFT JOIN PRODUCTONEGOCIO pn ON pn.idProducto = p.idProducto");
                    query.AppendLine("LEFT JOIN (SELECT idProducto, precioCompra, precioVenta FROM PRECIO_PRODUCTO WHERE idMoneda = 2) ppDolar ON ppDolar.idProducto = p.idProducto");
                    query.AppendLine("LEFT JOIN (SELECT idProducto, precioVenta, precioLista, precioCompra FROM PRECIO_PRODUCTO WHERE idMoneda = 1) ppPesos ON ppPesos.idProducto = p.idProducto");
                    query.AppendLine("WHERE p.estado = 1");
                    query.AppendLine("GROUP BY p.idProducto, p.codigo, p.nombre, p.descripcion, c.idCategoria, c.descripcion,");
                    query.AppendLine("ppDolar.precioCompra, ppDolar.precioVenta, ppPesos.precioVenta, ppPesos.precioLista, ppPesos.precioCompra, p.prodSerializable");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(dr["ProductoId"]),
                                Codigo = dr["codigo"].ToString(),
                                Nombre = dr["nombre"].ToString(),
                                Descripcion = dr["descripcion"].ToString(),
                                OCategoria = new Categoria
                                {
                                    IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                    Descripcion = dr["DescripcionCategoria"].ToString()
                                },
                                CostoPesos = dr["costoPesos"] != DBNull.Value ? Convert.ToDecimal(dr["costoPesos"]) : 0,
                                PrecioCompra = dr["precioCompra"] != DBNull.Value ? Convert.ToDecimal(dr["precioCompra"]) : 0,
                                PrecioVenta = dr["precioVenta"] != DBNull.Value ? Convert.ToDecimal(dr["precioVenta"]) : 0,
                                Estado = true,
                                Stock = Convert.ToInt32(dr["stock"]),
                                StockH1 = Convert.ToInt32(dr["stockH1"]),
                                StockH2 = Convert.ToInt32(dr["stockH2"]),
                                StockAS = Convert.ToInt32(dr["stockAS"]),
                                StockAC = Convert.ToInt32(dr["stockAC"]),
                                ProdSerializable = Convert.ToBoolean(dr["prodSerializable"]),
                                PrecioLista = dr["precioLista"] != DBNull.Value ? Convert.ToDecimal(dr["precioLista"]) : 0,
                                VentaPesos = dr["ventaPesos"] != DBNull.Value ? Convert.ToDecimal(dr["ventaPesos"]) : 0
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return lista;
        }




        public async Task<List<Producto>> ListarSerializablesPorNegocioAsync(int idNegocio)
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.idProducto, p.codigo, p.nombre, p.descripcion, c.idCategoria, c.descripcion[DescripcionCategoria], p.ventaPesos,");
                    query.AppendLine("ISNULL(pn.stock, 0) AS stock, p.precioCompra, p.precioVenta, p.estado, p.costoPesos, p.prodSerializable");
                    query.AppendLine("FROM Producto p");
                    query.AppendLine("INNER JOIN CATEGORIA c ON c.idCategoria = p.idCategoria");
                    query.AppendLine("LEFT JOIN PRODUCTONEGOCIO pn ON pn.idProducto = p.idProducto");
                    query.AppendLine("WHERE p.estado = 1 AND pn.idNegocio = @idNegocio AND p.prodSerializable = 1");

                    using SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Producto()
                        {
                            IdProducto = Convert.ToInt32(dr["idProducto"]),
                            Codigo = dr["codigo"].ToString(),
                            Nombre = dr["nombre"].ToString(),
                            Descripcion = dr["descripcion"].ToString(),
                            OCategoria = new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Descripcion = dr["DescripcionCategoria"].ToString()
                            },
                            CostoPesos = Convert.ToDecimal(dr["costoPesos"]),
                            PrecioCompra = Convert.ToDecimal(dr["precioCompra"]),
                            PrecioVenta = Convert.ToDecimal(dr["precioVenta"]),
                            Estado = Convert.ToBoolean(dr["estado"]),
                            Stock = Convert.ToInt32(dr["stock"]),
                            ProdSerializable = Convert.ToBoolean(dr["prodSerializable"]),
                            VentaPesos = Convert.ToDecimal(dr["ventaPesos"])
                        });
                    }
                }
                catch (Exception)
                {
                    lista = new List<Producto>();
                }
            }
            return lista;
        }

        public async Task<List<Producto>> ListarSerializablesAsync(int idLocal)
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.idProducto, p.codigo, p.nombre, p.descripcion, c.idCategoria, c.descripcion AS DescripcionCategoria,");
                    query.AppendLine("p.ventaPesos, ISNULL(pn.stock, 0) AS stock, p.precioCompra, p.precioVenta, p.estado,");
                    query.AppendLine("p.costoPesos, p.prodSerializable, pn.idNegocio");
                    query.AppendLine("FROM Producto p");
                    query.AppendLine("INNER JOIN CATEGORIA c ON c.idCategoria = p.idCategoria");
                    query.AppendLine("LEFT JOIN PRODUCTONEGOCIO pn ON pn.idProducto = p.idProducto");
                    query.AppendLine("WHERE p.estado = 1 AND p.prodSerializable = 1");
                    query.AppendLine("AND pn.idNegocio = @idLocal AND ISNULL(pn.stock, 0) > 0");

                    using SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idLocal", idLocal);
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        var producto = new Producto()
                        {
                            IdProducto = Convert.ToInt32(dr["idProducto"]),
                            Codigo = dr["codigo"].ToString(),
                            Nombre = dr["nombre"].ToString(),
                            Descripcion = dr["descripcion"].ToString(),
                            OCategoria = new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Descripcion = dr["DescripcionCategoria"].ToString()
                            },
                            CostoPesos = dr["costoPesos"] != DBNull.Value ? Convert.ToDecimal(dr["costoPesos"]) : 0,
                            PrecioCompra = dr["precioCompra"] != DBNull.Value ? Convert.ToDecimal(dr["precioCompra"]) : 0,
                            PrecioVenta = dr["precioVenta"] != DBNull.Value ? Convert.ToDecimal(dr["precioVenta"]) : 0,
                            Estado = Convert.ToBoolean(dr["estado"]),
                            Stock = Convert.ToInt32(dr["stock"]),
                            ProdSerializable = Convert.ToBoolean(dr["prodSerializable"]),
                            VentaPesos = dr["ventaPesos"] != DBNull.Value ? Convert.ToDecimal(dr["ventaPesos"]) : 0
                        };

                        int idNegocio = dr["idNegocio"] != DBNull.Value ? Convert.ToInt32(dr["idNegocio"]) : 0;
                        producto.NombreLocal = idNegocio switch
                        {
                            1 => "HITECH 1",
                            2 => "HITECH 2",
                            3 => "APPLE 49",
                            4 => "APPLE CAFÉ",
                            _ => ""
                        };

                        lista.Add(producto);
                    }
                }
                catch (Exception)
                {
                    lista = new List<Producto>();
                }
            }
            return lista;
        }

        public async Task<List<Producto>> ListarAsync(int idNegocio)
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.idProducto AS ProductoId, p.codigo, p.nombre, p.descripcion,p.productoDolar,");
                    query.AppendLine("c.idCategoria, c.descripcion AS DescripcionCategoria,");
                    query.AppendLine("ISNULL(pn.stock, 0) AS stock,");
                    query.AppendLine("ISNULL(ppDolar.precioCompra, 0) AS precioCompra,");
                    query.AppendLine("ISNULL(ppDolar.precioVenta, 0) AS precioVenta,");
                    query.AppendLine("ISNULL(ppPesos.precioVenta, 0) AS ventaPesos,");
                    query.AppendLine("ISNULL(ppPesos.precioLista, 0) AS precioLista,");
                    query.AppendLine("ISNULL(ppPesos.precioCompra, 0) AS costoPesos,");
                    query.AppendLine("p.prodSerializable");
                    query.AppendLine("FROM Producto p");
                    query.AppendLine("INNER JOIN CATEGORIA c ON c.idCategoria = p.idCategoria");
                    query.AppendLine("LEFT JOIN PRODUCTONEGOCIO pn ON pn.idProducto = p.idProducto AND pn.idNegocio = @idNegocio");
                    query.AppendLine("LEFT JOIN (");
                    query.AppendLine("    SELECT idProducto, precioCompra, precioVenta");
                    query.AppendLine("    FROM PRECIO_PRODUCTO");
                    query.AppendLine("    WHERE idMoneda = 2");
                    query.AppendLine(") ppDolar ON ppDolar.idProducto = p.idProducto");
                    query.AppendLine("LEFT JOIN (");
                    query.AppendLine("    SELECT idProducto, precioVenta, precioLista, precioCompra");
                    query.AppendLine("    FROM PRECIO_PRODUCTO");
                    query.AppendLine("    WHERE idMoneda = 1");
                    query.AppendLine(") ppPesos ON ppPesos.idProducto = p.idProducto");
                    query.AppendLine("WHERE p.estado = 1");
                    query.AppendLine("GROUP BY p.idProducto, p.codigo, p.nombre, p.descripcion,p.productoDolar,");
                    query.AppendLine("c.idCategoria, c.descripcion, pn.stock,");
                    query.AppendLine("ppDolar.precioCompra, ppDolar.precioVenta,");
                    query.AppendLine("ppPesos.precioVenta, ppPesos.precioLista, ppPesos.precioCompra,");
                    query.AppendLine("p.prodSerializable");

                    using SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Producto()
                        {
                            IdProducto = Convert.ToInt32(dr["ProductoId"]),
                            Codigo = dr["codigo"].ToString(),
                            Nombre = dr["nombre"].ToString(),
                            Descripcion = dr["descripcion"].ToString(),
                            OCategoria = new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Descripcion = dr["DescripcionCategoria"].ToString()
                            },
                            CostoPesos = Convert.ToDecimal(dr["costoPesos"]),
                            PrecioCompra = Convert.ToDecimal(dr["precioCompra"]),
                            PrecioVenta = Convert.ToDecimal(dr["precioVenta"]),
                            Estado = true,
                            Stock = Convert.ToInt32(dr["stock"]),
                            ProdSerializable = Convert.ToBoolean(dr["prodSerializable"]),
                            PrecioLista = Convert.ToDecimal(dr["precioLista"]),
                            VentaPesos = Convert.ToDecimal(dr["ventaPesos"]),
                            ProductoDolar = Convert.ToBoolean(dr["productoDolar"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    lista = new List<Producto>();
                }
            }
            return lista;
        }

        public async Task<List<Producto>> ListarProductosEnStockAsync(int idNegocio)
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.idProducto AS ProductoId, p.codigo, p.nombre, p.descripcion, p.productoDolar,");
                    query.AppendLine("c.idCategoria, c.descripcion AS DescripcionCategoria,");
                    query.AppendLine("ISNULL(pn.stock, 0) AS stock,");
                    query.AppendLine("ISNULL(ppDolar.precioCompra, 0) AS precioCompra,");
                    query.AppendLine("ISNULL(ppDolar.precioVenta, 0) AS precioVenta,");
                    query.AppendLine("ISNULL(ppPesos.precioVenta, 0) AS ventaPesos,");
                    query.AppendLine("ISNULL(ppPesos.precioLista, 0) AS precioLista,");
                    query.AppendLine("ISNULL(ppPesos.precioCompra, 0) AS costoPesos,");
                    query.AppendLine("p.prodSerializable");
                    query.AppendLine("FROM Producto p");
                    query.AppendLine("INNER JOIN CATEGORIA c ON c.idCategoria = p.idCategoria");
                    query.AppendLine("LEFT JOIN PRODUCTONEGOCIO pn ON pn.idProducto = p.idProducto AND pn.idNegocio = @idNegocio");
                    query.AppendLine("LEFT JOIN (");
                    query.AppendLine("    SELECT idProducto, precioCompra, precioVenta");
                    query.AppendLine("    FROM PRECIO_PRODUCTO");
                    query.AppendLine("    WHERE idMoneda = 2");
                    query.AppendLine(") ppDolar ON ppDolar.idProducto = p.idProducto");
                    query.AppendLine("LEFT JOIN (");
                    query.AppendLine("    SELECT idProducto, precioVenta, precioLista, precioCompra");
                    query.AppendLine("    FROM PRECIO_PRODUCTO");
                    query.AppendLine("    WHERE idMoneda = 1");
                    query.AppendLine(") ppPesos ON ppPesos.idProducto = p.idProducto");
                    query.AppendLine("WHERE p.estado = 1 AND ISNULL(pn.stock, 0) > 0");
                    query.AppendLine("GROUP BY p.idProducto, p.codigo, p.nombre, p.descripcion, p.productoDolar,");
                    query.AppendLine("c.idCategoria, c.descripcion, pn.stock,");
                    query.AppendLine("ppDolar.precioCompra, ppDolar.precioVenta,");
                    query.AppendLine("ppPesos.precioVenta, ppPesos.precioLista, ppPesos.precioCompra,");
                    query.AppendLine("p.prodSerializable");

                    using SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    await oconexion.OpenAsync();

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Producto()
                        {
                            IdProducto = Convert.ToInt32(dr["ProductoId"]),
                            Codigo = dr["codigo"].ToString(),
                            Nombre = dr["nombre"].ToString(),
                            Descripcion = dr["descripcion"].ToString(),
                            OCategoria = new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Descripcion = dr["DescripcionCategoria"].ToString()
                            },
                            CostoPesos = Convert.ToDecimal(dr["costoPesos"]),
                            PrecioCompra = Convert.ToDecimal(dr["precioCompra"]),
                            PrecioVenta = Convert.ToDecimal(dr["precioVenta"]),
                            Estado = true,
                            Stock = Convert.ToInt32(dr["stock"]),
                            ProdSerializable = Convert.ToBoolean(dr["prodSerializable"]),
                            PrecioLista = Convert.ToDecimal(dr["precioLista"]),
                            VentaPesos = Convert.ToDecimal(dr["ventaPesos"]),
                            ProductoDolar = Convert.ToBoolean(dr["productoDolar"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    lista = new List<Producto>();
                }
            }
            return lista;
        }

        public async Task<string> ObtenerNombreProductoAsync(int idProducto)
        {
            string nombreProducto = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("ObtenerNombreProducto", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idProducto", idProducto);

                    await oconexion.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                    {
                        nombreProducto = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    nombreProducto = string.Empty;
                }
            }

            return nombreProducto;
        }



    }
}
