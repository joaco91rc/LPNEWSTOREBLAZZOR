using DataLayer;
using Entities;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServicioTecnicoService
    {
        private readonly DL_ServicioTecnico _servicioTecnicoDL;

        public ServicioTecnicoService(DL_ServicioTecnico servicioTecnicoDL)
        {
            _servicioTecnicoDL = servicioTecnicoDL;
        }

        public async Task<List<ServicioTecnico>> ListarAsync(int idNegocio)
        {
            return await _servicioTecnicoDL.ListarAsync(idNegocio);
        }

        public async Task<ServicioTecnico?> ObtenerServicioTecnicoAsync(int idServicio)
        {
            return await _servicioTecnicoDL.ObtenerServicioTecnicoAsync(idServicio);
        }

        public async Task<List<ServicioTecnico>> ListarServiciosCompletadosAsync(int idNegocio)
        {
            return await _servicioTecnicoDL.ListarServiciosCompletadosAsync(idNegocio);
        }

        public async Task<List<ServicioTecnico>> ListarServiciosCobradosAsync(int idNegocio, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _servicioTecnicoDL.ListarServiciosCobradosAsync(idNegocio, fechaInicio, fechaFin);
        }

        public async Task<(bool Resultado, string Mensaje, int IdServicioGenerado)> InsertarServicioTecnicoAsync(ServicioTecnico servicioTecnico)
        {
            return await _servicioTecnicoDL.InsertarServicioTecnicoAsync(servicioTecnico);
        }

        public async Task<(bool Resultado, string Mensaje)> ModificarServicioTecnicoAsync(ServicioTecnico servicioTecnico)
        {
            return await _servicioTecnicoDL.ModificarServicioTecnicoAsync(servicioTecnico);
        }

        public async Task<(bool Resultado, string Mensaje)> CambiarEstadoIngresadoAEnReparacionAsync(int idServicio)
        {
            return await _servicioTecnicoDL.CambiarEstadoIngresadoAEnReparacionAsync(idServicio);
        }

        public async Task<(bool Resultado, string Mensaje)> CambiarEstadoPendienteACompletadoAsync(int idServicio, string descripcionReparacion, string observaciones)
        {
            return await _servicioTecnicoDL.CambiarEstadoPendienteACompletadoAsync(idServicio, descripcionReparacion, observaciones);
        }

        public async Task<(bool Resultado, string Mensaje)> CambiarEstadoCompletoACobradoAsync(int idServicio)
        {
            return await _servicioTecnicoDL.CambiarEstadoCompletoACobradoAsync(idServicio);
        }

        public async Task<(bool Resultado, CostoServicioTecnico ResultadoCosto, string Mensaje)> ObtenerCostosPorServicioAsync(int idServicio)
        {
            return await _servicioTecnicoDL.ObtenerCostosPorServicioAsync(idServicio);
        }

        public async Task<(bool Resultado, string Mensaje)> CobrarServicioTecnicoAsync(int idServicio, decimal precioReal, DateTime fechaEntregaReal)
        {
            return await _servicioTecnicoDL.CobrarServicioTecnicoAsync(idServicio, precioReal, fechaEntregaReal);
        }
    }
}
