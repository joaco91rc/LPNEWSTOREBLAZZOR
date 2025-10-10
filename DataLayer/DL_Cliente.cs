using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Cliente
    {
        private readonly string _cadenaConexion;
        private readonly DL_ClienteNegocio _clienteNegocioDL;

        public DL_Cliente(DatabaseSettings settings, DL_ClienteNegocio clienteNegocioDL)
        {
            _cadenaConexion = settings.ConnectionString;
            _clienteNegocioDL = clienteNegocioDL;
        }

       

            public async Task<List<Cliente>> ListarClientes()
        {
                List<Cliente> lista = new List<Cliente>();
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.AppendLine("SELECT c.idCliente, c.documento, c.nombreCompleto, c.correo, c.telefono, c.estado, c.ciudad, c.direccion, ");
                        query.AppendLine("(CASE WHEN cn1.idCliente IS NOT NULL THEN 'Si' ELSE 'No' END) AS hitech1, ");
                        query.AppendLine("(CASE WHEN cn2.idCliente IS NOT NULL THEN 'Si' ELSE 'No' END) AS hitech2, ");
                        query.AppendLine("(CASE WHEN cn3.idCliente IS NOT NULL THEN 'Si' ELSE 'No' END) AS appleStore, ");
                        query.AppendLine("(CASE WHEN cn4.idCliente IS NOT NULL THEN 'Si' ELSE 'No' END) AS appleCafe ");
                        query.AppendLine("FROM Cliente c ");
                        query.AppendLine("LEFT JOIN CLIENTESNEGOCIO cn1 ON c.idCliente = cn1.idCliente AND cn1.idNegocio = 1 "); // ID de hitech1
                        query.AppendLine("LEFT JOIN CLIENTESNEGOCIO cn2 ON c.idCliente = cn2.idCliente AND cn2.idNegocio = 2 "); // ID de hitech2
                        query.AppendLine("LEFT JOIN CLIENTESNEGOCIO cn3 ON c.idCliente = cn3.idCliente AND cn3.idNegocio = 3 "); // ID de appleStore
                        query.AppendLine("LEFT JOIN CLIENTESNEGOCIO cn4 ON c.idCliente = cn4.idCliente AND cn4.idNegocio = 4 "); // ID de appleCafe

                        SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                        cmd.CommandType = CommandType.Text;
                        oconexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Cliente()
                                {
                                    IdCliente = Convert.ToInt32(dr["idCliente"]),
                                    Documento = dr["documento"].ToString(),
                                    NombreCompleto = dr["nombreCompleto"].ToString(),
                                    Correo = dr["correo"].ToString(),
                                    Ciudad = dr["ciudad"].ToString(),
                                    Direccion = dr["direccion"].ToString(),
                                    Telefono = dr["telefono"].ToString(),
                                    Estado = Convert.ToBoolean(dr["estado"]),
                                    Hitech1 = dr["hitech1"].ToString(),
                                    Hitech2 = dr["hitech2"].ToString(),
                                    AppleStore = dr["appleStore"].ToString(),
                                    AppleCafe = dr["appleCafe"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        lista = new List<Cliente>();
                    }
                }
                return lista;
            }


            public List<Cliente> Listar()
            {
                List<Cliente> lista = new List<Cliente>();
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    try
                    {

                        StringBuilder query = new StringBuilder();
                        query.AppendLine("select * from Cliente ");

                        SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                        cmd.CommandType = CommandType.Text;
                        oconexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Cliente()
                                {
                                    IdCliente = Convert.ToInt32(dr["idCliente"]),
                                    Documento = dr["documento"].ToString(),
                                    NombreCompleto = dr["nombreCompleto"].ToString(),
                                    Correo = dr["correo"].ToString(),
                                    Ciudad = dr["ciudad"].ToString(),
                                    Direccion = dr["direccion"].ToString(),
                                    Telefono = dr["telefono"].ToString(),
                                    Estado = Convert.ToBoolean(dr["estado"])

                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        lista = new List<Cliente>();
                    }

                }
                return lista;
            }

            public Cliente ObtenerClientePorDocumentoYNombre(string documentoCliente, string nombreCompleto)
            {
                Cliente cliente = null; // Valor por defecto si no se encuentra el cliente

                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    try
                    {
                        // Crear la consulta SQL
                        StringBuilder query = new StringBuilder();
                        query.AppendLine("SELECT *");
                        query.AppendLine("FROM Cliente WHERE documento = @documento AND nombreCompleto = @nombreCompleto");

                        SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                        cmd.CommandType = CommandType.Text;

                        // Agregar parámetros para evitar inyecciones SQL
                        cmd.Parameters.AddWithValue("@documento", documentoCliente);
                        cmd.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);

                        oconexion.Open();

                        // Ejecutar el comando y leer los datos
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                cliente = new Cliente
                                {
                                    IdCliente = Convert.ToInt32(dr["idCliente"]),
                                    Documento = dr["documento"].ToString(),
                                    NombreCompleto = dr["nombreCompleto"].ToString(),
                                    Correo = dr["correo"].ToString(),
                                    Ciudad = dr["ciudad"].ToString(),
                                    Direccion = dr["direccion"].ToString(),
                                    Telefono = dr["telefono"].ToString(),
                                    Estado = Convert.ToBoolean(dr["estado"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores (opcional: podrías registrar el error)
                        cliente = null; // Asegurarse de que se devuelve null en caso de error
                    }
                }

                return cliente;
            }

            public int ObtenerIdClientePorDocumentoYNombre(string documentoCliente, string nombreCompleto)
            {
                int idCliente = -1; // Valor por defecto si no se encuentra el cliente
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    try
                    {
                        // Crear la consulta SQL
                        StringBuilder query = new StringBuilder();
                        query.AppendLine("SELECT idCliente FROM Cliente WHERE documento = @documento AND nombreCompleto = @nombreCompleto");

                        SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                        cmd.CommandType = CommandType.Text;

                        // Agregar parámetros para evitar inyecciones SQL
                        cmd.Parameters.AddWithValue("@documento", documentoCliente);
                        cmd.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);

                        oconexion.Open();

                        // Ejecutar el comando y obtener el resultado
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            idCliente = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores (opcional: podrías registrar el error)
                        idCliente = -1; // Asegurarte de que se devuelve un valor por defecto
                    }
                }
                return idCliente;
            }

            public Cliente ObtenerClienteById(int idCliente)
            {
                Cliente cliente = null;
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.AppendLine("select * from CLIENTE where idCliente= @idCliente");


                        SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@idCLiente", idCliente);

                        oconexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                cliente = new Cliente()
                                {
                                    IdCliente = Convert.ToInt32(dr["idCliente"]),
                                    NombreCompleto = dr["nombreCompleto"].ToString(),
                                    Documento = dr["documento"].ToString(),
                                    Correo = dr["correo"].ToString(),
                                    Ciudad = dr["ciudad"].ToString(),
                                    Telefono = dr["telefono"].ToString(),
                                    Direccion = dr["direccion"].ToString(),
                                    Estado = Convert.ToBoolean(dr["estado"]),
                                    FechaRegistro = dr["fechaRegistro"].ToString(),

                                };

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        cliente = null;
                    }
                }
                return cliente;
            }

        public async Task<List<Cliente>> ListarClientesPorNegocio(int idNegocio)
        {
            List<Cliente> lista = new List<Cliente>();

            // Obtener los IDs de los clientes asociados al negocio
            List<int> clienteIds = await _clienteNegocioDL.ListarClientesPorNegocio(idNegocio);

            if (clienteIds.Count == 0)
                return lista; // Devuelve lista vacía si no hay clientes asociados

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT *");
                    query.AppendLine("FROM Cliente");
                    query.AppendLine("WHERE idCliente IN (" + string.Join(",", clienteIds) + ")");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), oconexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        await oconexion.OpenAsync();

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                lista.Add(new Cliente
                                {
                                    IdCliente = Convert.ToInt32(dr["idCliente"]),
                                    Documento = dr["documento"].ToString(),
                                    NombreCompleto = dr["nombreCompleto"].ToString(),
                                    Correo = dr["correo"].ToString(),
                                    Ciudad = dr["ciudad"].ToString(),
                                    Direccion = dr["direccion"].ToString(),
                                    Telefono = dr["telefono"].ToString(),
                                    Estado = Convert.ToBoolean(dr["estado"])
                                });
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Logueá o tratá el error si hace falta
                    lista = new List<Cliente>();
                }
            }

            return lista;
        }




        public async Task<int> Registrar(Cliente objCliente, Action<string> setMensaje)
        {
            int idClienteGenerado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_REGISTRARCLIENTE", oconexion))
                    {
                        cmd.Parameters.AddWithValue("documento", objCliente.Documento);
                        cmd.Parameters.AddWithValue("nombreCompleto", objCliente.NombreCompleto);
                        cmd.Parameters.AddWithValue("correo", objCliente.Correo);
                        cmd.Parameters.AddWithValue("telefono", objCliente.Telefono);
                        cmd.Parameters.AddWithValue("estado", objCliente.Estado);
                        cmd.Parameters.AddWithValue("ciudad", objCliente.Ciudad);
                        cmd.Parameters.AddWithValue("direccion", objCliente.Direccion);

                        cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;

                        await oconexion.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        idClienteGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                        setMensaje?.Invoke(cmd.Parameters["mensaje"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                idClienteGenerado = 0;
                setMensaje?.Invoke($"Error: {ex.Message}");
            }

            return idClienteGenerado;
        }


        public async Task<bool> Editar(Cliente objCliente, Action<string> setMensaje)
        {
            bool respuesta = false;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_MODIFICARCLIENTE", oconexion))
                {
                    cmd.Parameters.AddWithValue("idCliente", objCliente.IdCliente);
                    cmd.Parameters.AddWithValue("documento", objCliente.Documento);
                    cmd.Parameters.AddWithValue("nombreCompleto", objCliente.NombreCompleto);
                    cmd.Parameters.AddWithValue("correo", objCliente.Correo);
                    cmd.Parameters.AddWithValue("telefono", objCliente.Telefono);
                    cmd.Parameters.AddWithValue("ciudad", objCliente.Ciudad);
                    cmd.Parameters.AddWithValue("direccion", objCliente.Direccion);
                    cmd.Parameters.AddWithValue("estado", objCliente.Estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    setMensaje?.Invoke(cmd.Parameters["mensaje"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                setMensaje?.Invoke($"Error: {ex.Message}");
            }

            return respuesta;
        }



        public async Task<bool> Eliminar(Cliente objCliente, Action<string> setMensaje)
        {
            bool respuesta = false;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_ELIMINARCLIENTE", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idCliente", objCliente.IdCliente);
                    cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@resultado"].Value);
                    setMensaje?.Invoke(cmd.Parameters["@mensaje"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                setMensaje?.Invoke($"Error: {ex.Message}");
            }

            return respuesta;
        }



    }
}
