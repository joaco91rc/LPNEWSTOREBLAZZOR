using DataLayer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Migracion Tablas identity core
            services.AddScoped<MigracionUsuarios>();


            services.AddScoped<CategoriaService>();
            services.AddScoped<DL_Categoria>();

            services.AddScoped<CajaRegistradoraService>();
            services.AddScoped<DL_CajaRegistradora>();

            services.AddScoped<ClienteService>();
            services.AddScoped<DL_Cliente>();
            services.AddScoped<DL_ClienteNegocio>();


            services.AddScoped<DL_Cotizacion>();
            services.AddScoped<CotizacionService>();

            services.AddScoped<CompraService>();
            services.AddScoped<DL_Compra>();

            services.AddScoped<FormaPagoService>();
            services.AddScoped<DL_FormaPago>();

            services.AddScoped<HistorialProductoSTService>();
            services.AddScoped<DL_HistorialProductoST>();

            services.AddScoped<HistorialServicioTecnicoService>();
            services.AddScoped<DL_HistorialServicioTecnico>();

            

            services.AddScoped<MonedaService>();
            services.AddScoped<DL_Moneda>();

            services.AddScoped<NegocioService>();
            services.AddScoped<DL_Negocio>();

            services.AddScoped<OrdenTraspasoService>();
            services.AddScoped<DL_OrdenTraspaso>();

            services.AddScoped<PagoParcialService>();
            services.AddScoped<DL_PagoParcial>();

            
            services.AddScoped<DL_PrecioProducto>();
            services.AddScoped<PrecioProductoService>();


            services.AddScoped<ProductoDetalleService>();
            services.AddScoped<DL_ProductoDetalle>();

            services.AddScoped<ProductoNegocioService>();
            services.AddScoped<DL_ProductoNegocio>();

            services.AddScoped<ProductoService>();
            services.AddScoped<DL_Producto>();

            services.AddScoped<ProveedorService>();
            services.AddScoped<DL_Proveedor>();

            services.AddScoped<RolService>();
            services.AddScoped<DL_Rol>();

            services.AddScoped<ServicioTecnicoService>();
            services.AddScoped<DL_ServicioTecnico>();

            services.AddScoped<UsuarioService>();
            services.AddScoped<DL_Usuario>();

            services.AddScoped<VendedorService>();
            services.AddScoped<DL_Vendedor>();



            return services;
        }
    }
}
