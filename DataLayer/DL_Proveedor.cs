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
    public class DL_Proveedor
    {
        private readonly string _cadenaConexion;
        

        public DL_Proveedor(DatabaseSettings settings, DL_ClienteNegocio clienteNegocioDL)
        {
            _cadenaConexion = settings.ConnectionString;
            
        }

        public async Task<List<Proveedor>> Listar()
        {
            List<Proveedor> lista = new();
            using SqlConnection oconexion = new(_cadenaConexion);
            try
            {
                string query = "SELECT idProveedor, documento, razonSocial, correo, telefono, estado FROM Proveedor";

                using SqlCommand cmd = new(query, oconexion);
                cmd.CommandType = CommandType.Text;

                await oconexion.OpenAsync();
                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    lista.Add(new Proveedor
                    {
                        IdProveedor = Convert.ToInt32(dr["idProveedor"]),
                        Documento = dr["documento"].ToString(),
                        RazonSocial = dr["razonSocial"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Estado = Convert.ToBoolean(dr["estado"])
                    });
                }
            }
            catch
            {
                lista = new();
            }

            return lista;
        }

        public async Task<(int idGenerado, string mensaje)> Registrar(Proveedor proveedor)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using SqlConnection oconexion = new(_cadenaConexion);
                using SqlCommand cmd = new("SP_REGISTRARPROVEEDOR", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("documento", proveedor.Documento);
                cmd.Parameters.AddWithValue("razonSocial", proveedor.RazonSocial);
                cmd.Parameters.AddWithValue("correo", proveedor.Correo);
                cmd.Parameters.AddWithValue("telefono", proveedor.Telefono);
                cmd.Parameters.AddWithValue("estado", proveedor.Estado);

                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await oconexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return (idGenerado, mensaje);
        }

        public async Task<(bool exito, string mensaje)> Editar(Proveedor proveedor)
        {
            bool exito = false;
            string mensaje = string.Empty;

            try
            {
                using SqlConnection oconexion = new(_cadenaConexion);
                using SqlCommand cmd = new("SP_EDITARPROVEEDOR", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idProveedor", proveedor.IdProveedor);
                cmd.Parameters.AddWithValue("documento", proveedor.Documento);
                cmd.Parameters.AddWithValue("razonSocial", proveedor.RazonSocial);
                cmd.Parameters.AddWithValue("correo", proveedor.Correo);
                cmd.Parameters.AddWithValue("telefono", proveedor.Telefono);
                cmd.Parameters.AddWithValue("estado", proveedor.Estado);

                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await oconexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                exito = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return (exito, mensaje);
        }

        public async Task<(bool exito, string mensaje)> Eliminar(int idProveedor)
        {
            bool exito = false;
            string mensaje = string.Empty;

            try
            {
                using SqlConnection oconexion = new(_cadenaConexion);
                using SqlCommand cmd = new("SP_ELIMINARPROVEEDOR", oconexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idProveedor", idProveedor);
                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await oconexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                exito = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (SqlException ex) when (ex.Number == 547) // Violación de FK
            {
                mensaje = "No se puede eliminar el proveedor porque tiene registros relacionados.";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return (exito, mensaje);
        }

    }
}
