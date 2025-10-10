using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ApplicationDbContext : IdentityDbContext<Usuario, Rol, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Opcional: cambiar nombres de tablas si querés
            builder.Entity<Usuario>().ToTable("Usuarios");
            builder.Entity<Rol>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UsuarioRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UsuarioClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UsuarioLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RolClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UsuarioTokens");
        }
    }
}
