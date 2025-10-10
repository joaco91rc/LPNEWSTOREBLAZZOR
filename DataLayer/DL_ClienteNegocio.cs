using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_ClienteNegocio
    {

        private readonly string _cadenaConexion;

        public DL_ClienteNegocio(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public bool ClienteAsignadoANegocio(int idCliente, int idNegocio)
        {
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT COUNT(*) FROM CLIENTESNEGOCIO");
                    query.AppendLine("WHERE idCliente = @idCliente AND idNegocio = @idNegocio");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    int count = (int)cmd.ExecuteScalar(); // Obtener el número de coincidencias
                    return count > 0; // Retorna true si hay coincidencias, false en caso contrario
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones (opcional)
                    return false; // Retorna false en caso de error
                }
            }
        }

        public async Task<(bool exito, string mensaje)> AsignarClienteANegocio(int idCliente, int idNegocio)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SP_ASIGNARCLIENTEANEGOCIO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.Parameters.AddWithValue("@fechaAsignacion", DateTime.Now);

                    cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    await oconexion.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    bool exito = Convert.ToInt32(cmd.Parameters["@resultado"].Value) == 1;
                    string mensaje = cmd.Parameters["@mensaje"].Value.ToString();

                    return (exito, mensaje);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        public async Task<List<int>> ListarClientesPorNegocio(int idNegocio)
        {
            List<int> clientes = new List<int>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT idCliente FROM CLIENTESNEGOCIO");
                    query.AppendLine("WHERE idNegocio = @idNegocio");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), oconexion))
                    {
                        cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                        cmd.CommandType = CommandType.Text;

                        await oconexion.OpenAsync();

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                clientes.Add(Convert.ToInt32(dr["idCliente"]));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log opcional
                    clientes = new List<int>();
                }
            }

            return clientes;
        }





        public List<int> ListarNegociosDeCliente(int idCliente)
        {
            List<int> negocios = new List<int>();

            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT idNegocio FROM CLIENTESNEGOCIO");
                    query.AppendLine("WHERE idCliente = @idCliente");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            negocios.Add(Convert.ToInt32(dr["idNegocio"])); // Agregar cada idNegocio a la lista
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones (opcional)
                    negocios = new List<int>(); // Retornar lista vacía en caso de error
                }
            }

            return negocios;
        }

        public bool ModificarClientesEnNegocios(int idCliente, List<int> listaNegocios)
        {
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                SqlTransaction transaccion = null;
                try
                {
                    oconexion.Open();
                    transaccion = oconexion.BeginTransaction();

                    // 1. Eliminar todos los negocios del cliente
                    StringBuilder queryEliminar = new StringBuilder();
                    queryEliminar.AppendLine("DELETE FROM CLIENTESNEGOCIO WHERE idCliente = @idCliente");

                    SqlCommand cmdEliminar = new SqlCommand(queryEliminar.ToString(), oconexion, transaccion);
                    cmdEliminar.Parameters.AddWithValue("@idCliente", idCliente);
                    cmdEliminar.CommandType = CommandType.Text;
                    cmdEliminar.ExecuteNonQuery();
                    DateTime fechaAsignacion = DateTime.Now;
                    // 2. Insertar los nuevos negocios
                    StringBuilder queryInsertar = new StringBuilder();
                    queryInsertar.AppendLine("INSERT INTO CLIENTESNEGOCIO (idCliente, idNegocio, fechaAsignacion)");
                    queryInsertar.AppendLine("VALUES (@idCliente, @idNegocio, @fechaAsignacion)");

                    foreach (int idNegocio in listaNegocios)
                    {
                        SqlCommand cmdInsertar = new SqlCommand(queryInsertar.ToString(), oconexion, transaccion);
                        cmdInsertar.Parameters.AddWithValue("@idCliente", idCliente);
                        cmdInsertar.Parameters.AddWithValue("@idNegocio", idNegocio);
                        cmdInsertar.Parameters.AddWithValue("@fechaAsignacion", fechaAsignacion);
                        cmdInsertar.CommandType = CommandType.Text;
                        cmdInsertar.ExecuteNonQuery(); // Ejecuta la inserción
                    }

                    // 3. Confirmar la transacción
                    transaccion.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (transaccion != null)
                    {
                        transaccion.Rollback(); // En caso de error, deshacer la transacción
                    }
                    return false;
                }
            }
        }

        public bool EliminarClientesDeNegocio(int idCliente)
        {
            using (SqlConnection oconexion = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("DELETE FROM CLIENTESNEGOCIO WHERE idCliente = @idCliente");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery(); // Ejecuta la eliminación y obtiene el número de filas afectadas
                    return filasAfectadas > 0; // Retorna true si se eliminaron filas
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones (opcional)
                    return false; // Retorna false en caso de error
                }
            }
        }
    }
}
