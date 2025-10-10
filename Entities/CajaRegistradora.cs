using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CajaRegistradora
    {

        public int IdCajaRegistradora { get; set; }
        public int IdNegocio { get; set; }
        public string FechaApertura { get; set; }
        public string FechaCierre { get; set; }
        public string UsuarioAperturaCaja { get; set; }
        public decimal Saldo { get; set; }
        public decimal SaldoMP { get; set; }
        public decimal SaldoUSS { get; set; }
        public decimal SaldoGalicia { get; set; }
        public decimal SaldoInicio { get; set; }
        public decimal SaldoInicioMP { get; set; }
        public decimal SaldoInicioUSS { get; set; }
        public decimal SaldoInicioGalicia { get; set; }
        public bool Estado { get; set; }

    }
}
