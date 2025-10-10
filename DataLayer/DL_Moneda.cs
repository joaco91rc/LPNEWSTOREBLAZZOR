using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Data;
using Microsoft.Data.SqlClient;


namespace DataLayer
{
    

    public class DL_Moneda
    {
        private readonly string _cadenaConexion;

        public DL_Moneda(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<Moneda>> ListarMonedas()
        {
            var lista = new List<Moneda>();

            try
            {
                await using var conexion = new SqlConnection(_cadenaConexion);
                await conexion.OpenAsync();

                await using var cmd = new SqlCommand("[dbo].[SP_LISTARMONEDAS]", conexion)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 30
                };

                await using var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult);

                int ordId = dr.GetOrdinal("idMoneda");
                int ordNombre = dr.GetOrdinal("nombre");
                int ordSimbolo = dr.GetOrdinal("simbolo");

                while (await dr.ReadAsync())
                {
                    lista.Add(new Moneda
                    {
                        IdMoneda = dr.IsDBNull(ordId) ? 0 : dr.GetInt32(ordId),
                        Nombre = dr.IsDBNull(ordNombre) ? "" : dr.GetString(ordNombre),
                        Simbolo = dr.IsDBNull(ordSimbolo) ? "" : dr.GetString(ordSimbolo)
                    });
                }

                return (lista);
            }
            catch (SqlException ex)
            {
                return (new List<Moneda>());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return (new List<Moneda>());
            }
        }


        public async Task<Moneda?> ObtenerMonedaPorId(int idMoneda)
        {
            Moneda? moneda = null;

            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand("SP_OBTENEMONEDA", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("idMoneda", idMoneda);

                await conexion.OpenAsync();

                using var dr = await cmd.ExecuteReaderAsync();
                if (await dr.ReadAsync())
                {
                    moneda = new Moneda
                    {
                        IdMoneda = Convert.ToInt32(dr["idMoneda"]),
                        Nombre = dr["nombre"].ToString(),
                        Simbolo = dr["simbolo"].ToString()
                    };
                }
            }
            catch
            {
                moneda = null;
            }

            return moneda;
        }

        public async Task<(int idGenerado, string mensaje)> RegistrarMoneda(Moneda moneda)
        {
            int idGenerado = 0;
            string mensaje = string.Empty;

            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand("SP_REGISTRARMONEDA", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("nombre", moneda.Nombre);
                cmd.Parameters.AddWithValue("simbolo", moneda.Simbolo);
                cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                idGenerado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }

            return (idGenerado, mensaje);
        }

        public async Task<(bool resultado, string mensaje)> EditarMoneda(Moneda moneda)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand("SP_EDITARMONEDA", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idMoneda", moneda.IdMoneda);
                cmd.Parameters.AddWithValue("nombre", moneda.Nombre);
                cmd.Parameters.AddWithValue("simbolo", moneda.Simbolo);
                cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }

        public async Task<(bool resultado, string mensaje)> EliminarMoneda(int idMoneda)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            try
            {
                using var conexion = new SqlConnection(_cadenaConexion);
                using var cmd = new SqlCommand("SP_ELIMINARMONEDA", conexion)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("idMoneda", idMoneda);
                cmd.Parameters.Add("resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                resultado = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString()!;
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return (resultado, mensaje);
        }
    }

}
