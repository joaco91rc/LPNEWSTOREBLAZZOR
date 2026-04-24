using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class UsuarioSesionService
{
    

    public int IdUsuario { get; set; }
    public int IdEmpresa { get; set; }
    public int IdNegocio { get; set; }
    public string Rol { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string NombreEmpresa { get; set; } = string.Empty;
    public string NombreNegocio { get; set; } = string.Empty;
    public bool EsAdmin { get; set; }
    public bool EsVendedor { get; set; }

   
}