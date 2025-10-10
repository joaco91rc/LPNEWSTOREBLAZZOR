using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        public string Ciudad { get; set; }


        public string Direccion { get; set; }
        public bool Estado { get; set; }
        public string FechaRegistro { get; set; }
        public string Hitech1 { get; set; }
        public string Hitech2 { get; set; }
        public string AppleStore { get; set; }
        public string AppleCafe { get; set; }



    }
}
