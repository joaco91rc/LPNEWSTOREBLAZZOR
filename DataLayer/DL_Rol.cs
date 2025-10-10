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
    public class DL_Rol
    {

        private readonly string _cadenaConexion;

        public DL_Rol(DatabaseSettings settings)
        {
            _cadenaConexion = settings.ConnectionString;
        }

        public async Task<List<Rol>> Listar()
        {
            var lista = new List<Rol>();

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_ROLES", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Rol
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
