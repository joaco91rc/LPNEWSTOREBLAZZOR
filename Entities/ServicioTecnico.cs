using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ServicioTecnico
    {
        public int IdServicio { get; set; }  // Llave primaria
        public int IdCliente { get; set; }  // Foreign Key a la tabla Clientes
        public string NombreCliente { get; set; }
        public string DniCliente { get; set; }
        public string TelefonoCliente { get; set; }

        public string Producto { get; set; }  // Campo de texto para el producto que traen a reparar
        public DateTime FechaRecepcion { get; set; }  // Fecha en que el equipo fue recibido
        public DateTime? FechaEntregaEstimada { get; set; }  // Fecha estimada de entrega (puede ser nula)
        public DateTime? FechaEntregaReal { get; set; }  // Fecha real de entrega (puede ser nula)
        public string DescripcionProblema { get; set; }  // Descripción del problema
        public string DescripcionReparacion { get; set; }  // Descripción de la reparación
        public string EstadoServicio { get; set; }  // Estado actual del servicio (Ej.: 'Pendiente', 'En Reparación', etc.)
                                                    // public decimal? CostoEstimado { get; set; }  // Costo estimado de la reparación (puede ser nulo)
        public decimal? CostoReal { get; set; }  // Costo real de la reparación (puede ser nulo)
        public string Observaciones { get; set; }  // Observaciones adicionales
        public DateTime FechaRegistro { get; set; }  // Fecha de registro en el sistema
        public int IdNegocio { get; set; }
        public string TipoEquipo { get; set; }
        public string AccesoriosEntregados { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string SerialNumber { get; set; }
        public decimal TasaDiagnostico { get; set; }


    }
}
