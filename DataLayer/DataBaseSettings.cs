using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; }

        public DatabaseSettings(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("cadena_conexion")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'cadena_conexion'.");
        }
    }
}
