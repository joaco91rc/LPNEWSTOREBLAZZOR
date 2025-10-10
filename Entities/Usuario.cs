using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Usuario : IdentityUser<int>
    {
        
        public string NombreCompleto { get; set; }
        [NotMapped]
        public Rol oRol { get; set; }

    }
}
