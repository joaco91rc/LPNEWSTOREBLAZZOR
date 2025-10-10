using Entities;
using Entities.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_ServicioTecnico
    {
        private readonly string _cadenaConexion;

        public DL_ServicioTecnico(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<ServicioTecnico>> ListarAsync(int idNegocio)
        {
            List<ServicioTecnico> lista = new();
            using SqlConnection conexion = new(_cadenaConexion);
            try
            {
                StringBuilder query = new();
                query.AppendLine("SELECT st.*, c.nombreCompleto AS NombreCliente, c.documento as DniCliente , c.telefono as TelefonoCliente");
                query.AppendLine("FROM SERVICIOTECNICO st");
                query.AppendLine("INNER JOIN CLIENTE c ON st.idCliente = c.idCliente");
                query.AppendLine("WHERE st.estadoServicio <> @estadoCompletado");
                query.AppendLine("AND st.idNegocio = @idNegocio");

                using SqlCommand cmd = new(query.ToString(), conexion);
                cmd.Parameters.AddWithValue("@estadoCompletado", "Completado");
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                await conexion.OpenAsync();

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new ServicioTecnico
                    {
                        IdServicio = Convert.ToInt32(dr["idServicio"]),
                        IdCliente = Convert.ToInt32(dr["idCliente"]),
                        NombreCliente = dr["NombreCliente"].ToString(),
                        DniCliente = dr["DniCliente"].ToString(),
                        TelefonoCliente = dr["TelefonoCliente"].ToString(),
                        Producto = dr["producto"].ToString(),
                        TipoEquipo = dr["tipoEquipo"] != DBNull.Value ? dr["tipoEquipo"].ToString() : string.Empty,
                        Marca = dr["marca"] != DBNull.Value ? dr["marca"].ToString() : string.Empty,
                        Modelo = dr["modelo"] != DBNull.Value ? dr["modelo"].ToString() : string.Empty,
                        SerialNumber = dr["serialNumber"] != DBNull.Value ? dr["serialNumber"].ToString() : string.Empty,
                        AccesoriosEntregados = dr["accesoriosEntregados"] != DBNull.Value ? dr["accesoriosEntregados"].ToString() : string.Empty,
                        FechaRecepcion = Convert.ToDateTime(dr["fechaRecepcion"]),
                        FechaEntregaEstimada = dr["fechaEntregaEstimada"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaEstimada"]) : (DateTime?)null,
                        FechaEntregaReal = dr["fechaEntregaReal"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaReal"]) : (DateTime?)null,
                        DescripcionProblema = dr["descripcionProblema"].ToString(),
                        DescripcionReparacion = dr["descripcionReparacion"] != DBNull.Value ? dr["descripcionReparacion"].ToString() : string.Empty,
                        TasaDiagnostico = dr["tasaDiagnostico"] != DBNull.Value ? Convert.ToDecimal(dr["tasaDiagnostico"]) : 0,
                        EstadoServicio = dr["estadoServicio"].ToString(),
                        CostoReal = dr["costoReal"] != DBNull.Value ? Convert.ToDecimal(dr["costoReal"]) : (decimal?)null,
                        Observaciones = dr["observaciones"] != DBNull.Value ? dr["observaciones"].ToString() : string.Empty,
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        IdNegocio = Convert.ToInt32(dr["idNegocio"])
                    });
                }
            }
            catch (Exception)
            {
                lista = new List<ServicioTecnico>();
                // Log opcional
            }
            return lista;
        }

        public async Task<ServicioTecnico?> ObtenerServicioTecnicoAsync(int idServicio)
        {
            ServicioTecnico? servicio = null;
            using SqlConnection conexion = new(_cadenaConexion);
            try
            {
                StringBuilder query = new();
                query.AppendLine("SELECT st.*, c.nombreCompleto AS NombreCliente");
                query.AppendLine("FROM SERVICIOTECNICO st");
                query.AppendLine("INNER JOIN CLIENTE c ON st.idCliente = c.idCliente");
                query.AppendLine("WHERE st.idServicio = @idServicio");

                using SqlCommand cmd = new(query.ToString(), conexion);
                cmd.Parameters.AddWithValue("@idServicio", idServicio);

                await conexion.OpenAsync();

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    servicio = new ServicioTecnico
                    {
                        IdServicio = Convert.ToInt32(dr["idServicio"]),
                        IdCliente = Convert.ToInt32(dr["idCliente"]),
                        NombreCliente = dr["NombreCliente"].ToString(),
                        Producto = dr["producto"].ToString(),
                        FechaRecepcion = Convert.ToDateTime(dr["fechaRecepcion"]),
                        FechaEntregaEstimada = dr["fechaEntregaEstimada"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaEstimada"]) : (DateTime?)null,
                        FechaEntregaReal = dr["fechaEntregaReal"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaReal"]) : (DateTime?)null,
                        DescripcionProblema = dr["descripcionProblema"].ToString(),
                        DescripcionReparacion = dr["descripcionReparacion"] != DBNull.Value ? dr["descripcionReparacion"].ToString() : string.Empty,
                        EstadoServicio = dr["estadoServicio"].ToString(),
                        CostoReal = dr["costoReal"] != DBNull.Value ? Convert.ToDecimal(dr["costoReal"]) : (decimal?)null,
                        Observaciones = dr["observaciones"] == DBNull.Value ? "" : dr["observaciones"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        IdNegocio = Convert.ToInt32(dr["idNegocio"])
                    };
                }
            }
            catch (Exception)
            {
                servicio = null;
                // Log opcional
            }

            return servicio;
        }

        public async Task<List<ServicioTecnico>> ListarServiciosCompletadosAsync(int idNegocio)
        {
            List<ServicioTecnico> lista = new();
            using SqlConnection conexion = new(_cadenaConexion);
            try
            {
                StringBuilder query = new();
                query.AppendLine("SELECT st.*, c.nombreCompleto AS NombreCliente");
                query.AppendLine("FROM SERVICIOTECNICO st");
                query.AppendLine("INNER JOIN CLIENTE c ON st.idCliente = c.idCliente");
                query.AppendLine("WHERE st.estadoServicio = @estadoCompletado");
                query.AppendLine("AND st.idNegocio = @idNegocio");

                using SqlCommand cmd = new(query.ToString(), conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@estadoCompletado", "COMPLETADO");
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);

                await conexion.OpenAsync();

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new ServicioTecnico
                    {
                        IdServicio = Convert.ToInt32(dr["idServicio"]),
                        IdCliente = Convert.ToInt32(dr["idCliente"]),
                        NombreCliente = dr["NombreCliente"].ToString(),
                        Producto = dr["producto"].ToString(),
                        FechaRecepcion = Convert.ToDateTime(dr["fechaRecepcion"]),
                        FechaEntregaEstimada = dr["fechaEntregaEstimada"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaEstimada"]) : (DateTime?)null,
                        FechaEntregaReal = dr["fechaEntregaReal"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaReal"]) : (DateTime?)null,
                        DescripcionProblema = dr["descripcionProblema"].ToString(),
                        DescripcionReparacion = dr["descripcionReparacion"].ToString(),
                        EstadoServicio = dr["estadoServicio"].ToString(),
                        CostoReal = dr["costoReal"] != DBNull.Value ? Convert.ToDecimal(dr["costoReal"]) : (decimal?)null,
                        Observaciones = dr["observaciones"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        IdNegocio = Convert.ToInt32(dr["idNegocio"])
                    });
                }
            }
            catch (Exception)
            {
                lista = new List<ServicioTecnico>();
                // Opcional: registrar log del error
            }
            return lista;
        }


        public async Task<List<ServicioTecnico>> ListarServiciosCobradosAsync(int idNegocio, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ServicioTecnico> lista = new();
            using SqlConnection conexion = new(_cadenaConexion);
            try
            {
                StringBuilder query = new();
                query.AppendLine("SELECT st.*, c.nombreCompleto AS NombreCliente");
                query.AppendLine("FROM SERVICIOTECNICO st");
                query.AppendLine("INNER JOIN CLIENTE c ON st.idCliente = c.idCliente");
                query.AppendLine("WHERE st.estadoServicio = @estadoCobrado");
                query.AppendLine("AND st.idNegocio = @idNegocio");
                query.AppendLine("AND st.fechaRecepcion BETWEEN @fechaInicio AND @fechaFin");

                using SqlCommand cmd = new(query.ToString(), conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@estadoCobrado", "COBRADO");
                cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Date);
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Date.AddDays(1).AddTicks(-1)); // Incluye todo el día final

                await conexion.OpenAsync();

                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new ServicioTecnico
                    {
                        IdServicio = Convert.ToInt32(dr["idServicio"]),
                        IdCliente = Convert.ToInt32(dr["idCliente"]),
                        NombreCliente = dr["NombreCliente"].ToString(),
                        Producto = dr["producto"].ToString(),
                        FechaRecepcion = Convert.ToDateTime(dr["fechaRecepcion"]),
                        FechaEntregaEstimada = dr["fechaEntregaEstimada"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaEstimada"]) : (DateTime?)null,
                        FechaEntregaReal = dr["fechaEntregaReal"] != DBNull.Value ? Convert.ToDateTime(dr["fechaEntregaReal"]) : (DateTime?)null,
                        DescripcionProblema = dr["descripcionProblema"].ToString(),
                        DescripcionReparacion = dr["descripcionReparacion"].ToString(),
                        EstadoServicio = dr["estadoServicio"].ToString(),
                        CostoReal = dr["costoReal"] != DBNull.Value ? Convert.ToDecimal(dr["costoReal"]) : (decimal?)null,
                        Observaciones = dr["observaciones"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["fechaRegistro"]),
                        IdNegocio = Convert.ToInt32(dr["idNegocio"])
                    });
                }
            }
            catch (Exception)
            {
                lista = new List<ServicioTecnico>();
                // Opcional: log del error
            }
            return lista;
        }

        public async Task<(bool Resultado, string Mensaje, int IdServicioGenerado)> InsertarServicioTecnicoAsync(ServicioTecnico servicioTecnico)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            int idServicioGenerado = 0;

            using SqlConnection conexion = new SqlConnection(_cadenaConexion);
            try
            {
                using SqlCommand cmd = new SqlCommand("SP_REGISTRARSERVICIOTECNICO", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idCliente", servicioTecnico.IdCliente);
                cmd.Parameters.AddWithValue("@producto", servicioTecnico.Producto);
                cmd.Parameters.AddWithValue("@fechaRecepcion", servicioTecnico.FechaRecepcion);
                cmd.Parameters.AddWithValue("@fechaEntregaEstimada", servicioTecnico.FechaEntregaEstimada.HasValue ? (object)servicioTecnico.FechaEntregaEstimada.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaEntregaReal", servicioTecnico.FechaEntregaReal.HasValue ? (object)servicioTecnico.FechaEntregaReal.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@descripcionProblema", servicioTecnico.DescripcionProblema);
                cmd.Parameters.AddWithValue("@descripcionReparacion", string.IsNullOrEmpty(servicioTecnico.DescripcionReparacion) ? DBNull.Value : (object)servicioTecnico.DescripcionReparacion);
                cmd.Parameters.AddWithValue("@estadoServicio", servicioTecnico.EstadoServicio);
                cmd.Parameters.AddWithValue("@costoReal", servicioTecnico.CostoReal.HasValue ? (object)servicioTecnico.CostoReal.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@observaciones", string.IsNullOrEmpty(servicioTecnico.Observaciones) ? DBNull.Value : (object)servicioTecnico.Observaciones);
                cmd.Parameters.AddWithValue("@fechaRegistro", servicioTecnico.FechaRegistro);
                cmd.Parameters.AddWithValue("@idNegocio", servicioTecnico.IdNegocio);

                cmd.Parameters.AddWithValue("@tipoEquipo", servicioTecnico.TipoEquipo);
                cmd.Parameters.AddWithValue("@marca", servicioTecnico.Marca);
                cmd.Parameters.AddWithValue("@modelo", servicioTecnico.Modelo);
                cmd.Parameters.AddWithValue("@serialNumber", servicioTecnico.SerialNumber);
                cmd.Parameters.AddWithValue("@accesoriosEntregados", servicioTecnico.AccesoriosEntregados);
                cmd.Parameters.AddWithValue("@tasaDiagnostico", servicioTecnico.TasaDiagnostico);

                // Parámetros de salida
                SqlParameter pResultado = new SqlParameter("@resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                SqlParameter pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
                SqlParameter pIdServicio = new SqlParameter("@idServicioSalida", SqlDbType.Int) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(pResultado);
                cmd.Parameters.Add(pMensaje);
                cmd.Parameters.Add(pIdServicio);

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado = Convert.ToBoolean(pResultado.Value);
                mensaje = pMensaje.Value.ToString();
                idServicioGenerado = Convert.ToInt32(pIdServicio.Value);
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje, idServicioGenerado);
        }

        public async Task<(bool Resultado, string Mensaje)> ModificarServicioTecnicoAsync(ServicioTecnico servicioTecnico)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            using SqlConnection conexion = new SqlConnection(_cadenaConexion);
            try
            {
                using SqlCommand cmd = new SqlCommand("SP_MODIFICARSERVICIOTECNICO", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idServicioTecnico", servicioTecnico.IdServicio);
                cmd.Parameters.AddWithValue("@idCliente", servicioTecnico.IdCliente);
                cmd.Parameters.AddWithValue("@producto", servicioTecnico.Producto);
                cmd.Parameters.AddWithValue("@fechaRecepcion", servicioTecnico.FechaRecepcion);
                cmd.Parameters.AddWithValue("@fechaEntregaEstimada", servicioTecnico.FechaEntregaEstimada.HasValue ? (object)servicioTecnico.FechaEntregaEstimada.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaEntregaReal", servicioTecnico.FechaEntregaReal.HasValue ? (object)servicioTecnico.FechaEntregaReal.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@descripcionProblema", servicioTecnico.DescripcionProblema);
                cmd.Parameters.AddWithValue("@descripcionReparacion", string.IsNullOrEmpty(servicioTecnico.DescripcionReparacion) ? DBNull.Value : (object)servicioTecnico.DescripcionReparacion);
                cmd.Parameters.AddWithValue("@estadoServicio", servicioTecnico.EstadoServicio);
                cmd.Parameters.AddWithValue("@costoReal", servicioTecnico.CostoReal.HasValue ? (object)servicioTecnico.CostoReal.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@observaciones", string.IsNullOrEmpty(servicioTecnico.Observaciones) ? DBNull.Value : (object)servicioTecnico.Observaciones);
                cmd.Parameters.AddWithValue("@fechaRegistro", servicioTecnico.FechaRegistro);
                cmd.Parameters.AddWithValue("@idNegocio", servicioTecnico.IdNegocio);
                cmd.Parameters.AddWithValue("@tipoEquipo", servicioTecnico.TipoEquipo);
                cmd.Parameters.AddWithValue("@marca", servicioTecnico.Marca);
                cmd.Parameters.AddWithValue("@modelo", servicioTecnico.Modelo);
                cmd.Parameters.AddWithValue("@serialNumber", servicioTecnico.SerialNumber);
                cmd.Parameters.AddWithValue("@accesoriosEntregados", servicioTecnico.AccesoriosEntregados);
                cmd.Parameters.AddWithValue("@tasaDiagnostico", servicioTecnico.TasaDiagnostico);

                // Parámetros de salida
                SqlParameter pResultado = new SqlParameter("@resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                SqlParameter pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(pResultado);
                cmd.Parameters.Add(pMensaje);

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado = Convert.ToBoolean(pResultado.Value);
                mensaje = pMensaje.Value.ToString();
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<(bool Resultado, string Mensaje)> CambiarEstadoIngresadoAEnReparacionAsync(int idServicio)
        {
            return await CambiarEstadoAsync(idServicio, "EN REPARACION", "", "");
        }

        public async Task<(bool Resultado, string Mensaje)> CambiarEstadoPendienteACompletadoAsync(int idServicio, string descripcionReparacion, string observaciones)
        {
            return await CambiarEstadoAsync(idServicio, "COMPLETADO", descripcionReparacion, observaciones);
        }

        public async Task<(bool Resultado, string Mensaje)> CambiarEstadoCompletoACobradoAsync(int idServicio)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            using SqlConnection conexion = new SqlConnection(_cadenaConexion);
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("UPDATE SERVICIOTECNICO SET estadoServicio = 'COBRADO'");
                query.AppendLine("WHERE idServicio = @idServicio");

                using SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@idServicio", idServicio);

                await conexion.OpenAsync();
                resultado = await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.ToString();
            }

            return (resultado, mensaje);
        }

        private async Task<(bool Resultado, string Mensaje)> CambiarEstadoAsync(int idServicio, string nuevoEstado, string descripcionReparacion, string observaciones)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            using SqlConnection conexion = new SqlConnection(_cadenaConexion);
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("UPDATE SERVICIOTECNICO SET estadoServicio = @nuevoEstado");

                if (nuevoEstado == "COMPLETADO")
                {
                    query.AppendLine(", descripcionReparacion = @descripcionReparacion, observaciones = @observaciones");
                }

                query.AppendLine("WHERE idServicio = @idServicio");

                using SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                cmd.Parameters.AddWithValue("@idServicio", idServicio);

                if (nuevoEstado == "COMPLETADO")
                {
                    cmd.Parameters.AddWithValue("@descripcionReparacion", descripcionReparacion);
                    cmd.Parameters.AddWithValue("@observaciones", observaciones);
                }

                await conexion.OpenAsync();
                resultado = await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.ToString();
                // Opcional: loguear error
            }

            return (resultado, mensaje);
        }


        public async Task<(bool Resultado, CostoServicioTecnico ResultadoCosto, string Mensaje)> ObtenerCostosPorServicioAsync(int idServicio)
        {
            CostoServicioTecnico resultado = new CostoServicioTecnico();
            string mensaje = string.Empty;
            bool operacionExitosa = false;

            using SqlConnection conexion = new SqlConnection(_cadenaConexion);
            try
            {
                using SqlCommand cmd = new SqlCommand("SP_CALCULAR_COSTO_ST", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idServicio", idServicio);

                var pManoDeObra = cmd.Parameters.Add("@totalManoDeObra", SqlDbType.Decimal);
                pManoDeObra.Direction = ParameterDirection.Output;
                pManoDeObra.Precision = 18;
                pManoDeObra.Scale = 2;

                var pRepuestos = cmd.Parameters.Add("@totalRepuestos", SqlDbType.Decimal);
                pRepuestos.Direction = ParameterDirection.Output;
                pRepuestos.Precision = 18;
                pRepuestos.Scale = 2;

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado.TotalManoDeObra = Convert.ToDecimal(pManoDeObra.Value);
                resultado.TotalRepuestos = Convert.ToDecimal(pRepuestos.Value);

                decimal costoReal = resultado.TotalManoDeObra + resultado.TotalRepuestos;

                using SqlCommand updateCmd = new SqlCommand("UPDATE SERVICIOTECNICO SET costoReal = @costoReal WHERE idServicio = @idServicio", conexion);
                updateCmd.Parameters.AddWithValue("@costoReal", costoReal);
                updateCmd.Parameters.AddWithValue("@idServicio", idServicio);

                int filasAfectadas = await updateCmd.ExecuteNonQueryAsync();
                operacionExitosa = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                mensaje = "Error al calcular y actualizar el costo: " + ex.Message;
                operacionExitosa = false;
            }

            return (operacionExitosa, resultado, mensaje);
        }

        public async Task<(bool Resultado, string Mensaje)> CobrarServicioTecnicoAsync(int idServicio, decimal precioReal, DateTime fechaEntregaReal)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            using SqlConnection conexion = new SqlConnection(_cadenaConexion);
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("UPDATE SERVICIOTECNICO SET estadoServicio = 'COBRADO',");
                query.AppendLine("precioReal = @precioReal,");
                query.AppendLine("fechaEntregaReal = @fechaEntregaReal");
                query.AppendLine("WHERE idServicio = @idServicio");

                using SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@precioReal", precioReal);
                cmd.Parameters.AddWithValue("@fechaEntregaReal", fechaEntregaReal);
                cmd.Parameters.AddWithValue("@idServicio", idServicio);

                await conexion.OpenAsync();
                resultado = await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.ToString();
                // Opcional: registrar error para depuración
            }

            return (resultado, mensaje);
        }

    }
}

