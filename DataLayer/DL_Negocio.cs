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
    public  class DL_Negocio
    {

        private readonly string _cadenaConexion;

        public DL_Negocio(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<Negocio> ObtenerDatos(int idNegocio)
        {
            Negocio objNegocio = new Negocio();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    await conexion.OpenAsync();
                    string query = "select * from NEGOCIO where idNegocio = @idNegocio";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            objNegocio = new Negocio()
                            {
                                IdNegocio = int.Parse(dr["idNegocio"].ToString()),
                                Nombre = dr["nombre"].ToString(),
                                CUIT = dr["CUIT"].ToString(),
                                Direccion = dr["direccion"].ToString(),
                                Telefono = dr["telefono"].ToString(),
                                Instagram = dr["instagram"].ToString(),
                                Mail = dr["mail"].ToString(),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objNegocio = new Negocio();
            }

            return objNegocio;
        }

        public async Task<List<Negocio>> ListarNegocios()
        {
            List<Negocio> lista = new List<Negocio>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    await conexion.OpenAsync();
                    string query = "select * from NEGOCIO";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Negocio()
                            {
                                IdNegocio = int.Parse(dr["idNegocio"].ToString()),
                                Nombre = dr["nombre"].ToString(),
                                CUIT = dr["CUIT"].ToString(),
                                Direccion = dr["direccion"].ToString(),
                                Telefono = dr["telefono"].ToString(),
                                Instagram = dr["instagram"].ToString(),
                                Mail = dr["mail"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Negocio>();
            }

            return lista;
        }

        public async Task<(bool respuesta, string mensaje)> Guardardatos(Negocio objeto, int idNegocio)
        {
            string mensaje = string.Empty;
            bool respuesta = true;

            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    await conexion.OpenAsync();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO SET nombre = @nombre,");
                    query.AppendLine("cuit = @cuit,");
                    query.AppendLine("direccion = @direccion,");
                    query.AppendLine("mail = @mail,");
                    query.AppendLine("instagram = @instagram,");
                    query.AppendLine("telefono = @telefono");
                    query.AppendLine("where idNegocio = @idNegocio;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("@cuit", objeto.CUIT);
                    cmd.Parameters.AddWithValue("@direccion", objeto.Direccion);
                    cmd.Parameters.AddWithValue("@mail", objeto.Mail);
                    cmd.Parameters.AddWithValue("@instagram", objeto.Instagram);
                    cmd.Parameters.AddWithValue("@telefono", objeto.Telefono);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    if (filasAfectadas < 1)
                    {
                        mensaje = "No se pudo guardar los datos";
                        respuesta = false;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }

            return (respuesta, mensaje);
        }

        public async Task<(byte[] logoBytes, bool obtenido)> ObtenerLogo(int idNegocio)
        {
            bool obtenido = true;
            byte[] logoBytes = new byte[0];

            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    await conexion.OpenAsync();
                    string query = "select logo from NEGOCIO where idNegocio = @idNegocio";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            if (dr["logo"] != DBNull.Value)
                            {
                                logoBytes = (byte[])dr["logo"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obtenido = false;
                logoBytes = new byte[0];
            }

            return (logoBytes, obtenido);
        }

        public async Task<(bool respuesta, string mensaje)> ActualizarLogo(byte[] image, int idNegocio)
        {
            string mensaje = string.Empty;
            bool respuesta = true;

            try
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    await conexion.OpenAsync();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update NEGOCIO SET logo = @imagen");
                    query.AppendLine("where idNegocio = @idNegocio;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@imagen", image);
                    cmd.Parameters.AddWithValue("@idNegocio", idNegocio);
                    cmd.CommandType = CommandType.Text;

                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    if (filasAfectadas < 1)
                    {
                        mensaje = "No se pudo actualizar el logo";
                        respuesta = false;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }

            return (respuesta, mensaje);
        }

        // Método adicional para obtener logo como string base64 (útil para Blazor)
        public async Task<string> ObtenerLogoBase64Async(int idNegocio)
        {
            try
            {
                var (logoBytes, obtenido) = await ObtenerLogo(idNegocio);

                if (obtenido && logoBytes.Length > 0)
                {
                    string base64String = Convert.ToBase64String(logoBytes);
                    return $"data:image/png;base64,{base64String}";
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
            }

            return string.Empty;
        }

        // Método para actualizar logo desde archivo base64 (útil para Blazor)
        public async Task<(bool respuesta, string mensaje)> ActualizarLogoDesdeBase64(string base64Image, int idNegocio)
        {
            try
            {
                // Remover el prefijo data:image si existe
                if (base64Image.Contains(","))
                {
                    base64Image = base64Image.Split(',')[1];
                }

                byte[] imageBytes = Convert.FromBase64String(base64Image);
                return await ActualizarLogo(imageBytes, idNegocio);
            }
            catch (Exception ex)
            {
                return (false, $"Error al procesar la imagen: {ex.Message}");
            }
        }
    }
}
