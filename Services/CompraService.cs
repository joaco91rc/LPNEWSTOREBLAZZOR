using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class CompraService
    {
        private readonly DL_Compra _compraDL;

        public CompraService(DL_Compra compraDL)
        {
            _compraDL = compraDL;
        }

        public Task<int> ObtenerCorrelativo()
            => _compraDL.ObtenerCorrelativo();

        // (exito, mensaje, idCompraSalida)
        public Task<(bool exito, string mensaje, int idCompraSalida)> Registrar(Compra compra, DataTable detalleCompra)
            => _compraDL.Registrar(compra, detalleCompra);

        public Task<List<Compra>> ObtenerComprasConDetalleEntreFechas(int idNegocio, DateTime fechaInicio, DateTime fechaFin)
            => _compraDL.ObtenerComprasConDetalleEntreFechas(idNegocio, fechaInicio, fechaFin);

        public Task<List<Compra>> ObtenerComprasConDetalle(int idNegocio)
            => _compraDL.ObtenerComprasConDetalle(idNegocio);

        public Task<Compra> ObtenerCompra(string numero, int idNegocio)
            => _compraDL.ObtenerCompra(numero, idNegocio);

        public Task<List<DetalleCompra>> ObtenerDetalleCompra(int idCompra)
            => _compraDL.ObtenerDetalleCompra(idCompra);

        // (exito, mensaje)
        public Task<(bool exito, string mensaje)> EliminarCompraConDetalle(int idCompra)
            => _compraDL.EliminarCompraConDetalle(idCompra);
    }
}
