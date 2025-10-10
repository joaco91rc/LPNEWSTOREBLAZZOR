using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SesionNegocioService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SesionNegocioService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int ObtenerIdNegocio()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null && context.Request.Cookies.TryGetValue("sucursalSeleccionada", out var valor))
            {
                if (int.TryParse(valor, out int idNegocio))
                {
                    return idNegocio;
                }
            }

            return 0; // Valor por defecto si no se encuentra la cookie
        }
    }
}
