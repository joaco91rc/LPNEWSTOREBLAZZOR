using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UsuarioViejo
    {

        public int idUsuario { get; set; }
        public string documento { get; set; }
        public string nombreCompleto { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }

        public RolViejo oRol { get; set; }
        public bool estado { get; set; }
        public string fechaRegistro { get; set; }



    }
}
