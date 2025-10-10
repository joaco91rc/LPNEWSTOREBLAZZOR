using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NegocioService
    {

        private readonly DL_Negocio _negocioDL;

        public NegocioService(DL_Negocio negocioDL)
        {
            _negocioDL = negocioDL;
        }

        public Task<Negocio> ObtenerDatos(int idNegocio)
            => _negocioDL.ObtenerDatos(idNegocio);

        public Task<List<Negocio>> ListarNegocios()
            => _negocioDL.ListarNegocios();

        public Task<(bool respuesta, string mensaje)> Guardardatos(Negocio objeto, int idNegocio)
            => _negocioDL.Guardardatos(objeto, idNegocio);

        public Task<(byte[] logoBytes, bool obtenido)> ObtenerLogo(int idNegocio)
            => _negocioDL.ObtenerLogo(idNegocio);

        public Task<(bool respuesta, string mensaje)> ActualizarLogo(byte[] image, int idNegocio)
            => _negocioDL.ActualizarLogo(image, idNegocio);

        public Task<string> ObtenerLogoBase64Async(int idNegocio)
            => _negocioDL.ObtenerLogoBase64Async(idNegocio);

        public Task<(bool respuesta, string mensaje)> ActualizarLogoDesdeBase64(string base64Image, int idNegocio)
            => _negocioDL.ActualizarLogoDesdeBase64(base64Image, idNegocio);
    }
}
