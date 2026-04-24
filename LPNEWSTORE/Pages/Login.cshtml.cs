using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace LPNEWSTORE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly NegocioService _negocioService;
        private readonly UsuarioService _usuarioService;

        public LoginModel(
    SignInManager<Usuario> signInManager,
    UserManager<Usuario> userManager,
    NegocioService negocioService,
    UsuarioService usuarioService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _negocioService = negocioService;
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty]
        public bool UsuarioValidado { get; set; }

        [BindProperty]
        public string NombreEmpresa { get; set; } = string.Empty;

        [BindProperty]
        public string NombreUsuario { get; set; } = string.Empty;

        [BindProperty]
        public string NombreRol { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public List<SelectListItem> Sucursales { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }

            public int IdEmpresa { get; set; }

            [Required(ErrorMessage = "Debe seleccionar una sucursal.")]
            public string Sucursal { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostValidarUsuarioAsync()
        {
            LimpiarEstadoVisual();

            if (string.IsNullOrWhiteSpace(Input.Username) || string.IsNullOrWhiteSpace(Input.Password))
            {
                ErrorMessage = "Debe ingresar usuario y contraseña.";
                return Page();
            }

            var user = await _usuarioService.ObtenerUsuarioConEmpresaAsync(Input.Username);

            if (user == null)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var passwordOk = _userManager.PasswordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                Input.Password
            ) == PasswordVerificationResult.Success;

            if (!passwordOk)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            if (user.OEmpresa.IdEmpresa <= 0)
            {
                ErrorMessage = "El usuario no tiene una empresa asignada.";
                return Page();
            }

            if (user.OEmpresa == null)
            {
                ErrorMessage = "No se pudo obtener la empresa del usuario.";
                return Page();
            }

            if (!user.OEmpresa.Activa)
            {
                ErrorMessage = "La empresa se encuentra inactiva.";
                return Page();
            }

            var sucursales = await _negocioService.ListarSucursalesPorEmpresa(user.OEmpresa.IdEmpresa);

            if (sucursales == null || !sucursales.Any())
            {
                ErrorMessage = "La empresa no tiene sucursales asociadas.";
                return Page();
            }

            UsuarioValidado = true;
            Input.IdEmpresa = user.OEmpresa.IdEmpresa;
            NombreEmpresa = user.OEmpresa.Nombre;
            NombreUsuario = user.NombreCompleto ?? user.UserName ?? string.Empty;
            NombreRol = user.oRol?.Name ?? string.Empty;

            Sucursales = sucursales.Select(x => new SelectListItem
            {
                Value = x.IdNegocio.ToString(),
                Text = x.Nombre
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostEntrarSistemaAsync()
        {
            LimpiarEstadoVisual();

            if (string.IsNullOrWhiteSpace(Input.Username) || string.IsNullOrWhiteSpace(Input.Password))
            {
                ErrorMessage = "Debe volver a ingresar usuario y contraseña.";
                return Page();
            }

            if (Input.IdEmpresa <= 0)
            {
                ErrorMessage = "No se pudo determinar la empresa del usuario.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Input.Sucursal))
            {
                ErrorMessage = "Debe seleccionar una sucursal.";
                await RecargarSucursalesAsync(Input.Username);
                UsuarioValidado = true;
                return Page();
            }

            var user = await _usuarioService.ObtenerUsuarioConEmpresaAsync(Input.Username);

            if (user == null)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var passwordOk = _userManager.PasswordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                Input.Password
            ) == PasswordVerificationResult.Success;

            if (!passwordOk)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            if (user.OEmpresa.IdEmpresa <= 0)
            {
                ErrorMessage = "El usuario no tiene una empresa asignada.";
                return Page();
            }

            if (user.OEmpresa == null)
            {
                ErrorMessage = "No se pudo obtener la empresa del usuario.";
                return Page();
            }

            if (user.OEmpresa.IdEmpresa != Input.IdEmpresa)
            {
                ErrorMessage = "La empresa del usuario no es válida.";
                return Page();
            }

            if (!user.OEmpresa.Activa)
            {
                ErrorMessage = "La empresa se encuentra inactiva.";
                return Page();
            }

            var sucursal = await _negocioService.ObtenerPorIdAsync(Convert.ToInt32(Input.Sucursal));

            if (sucursal == null)
            {
                ErrorMessage = "La sucursal seleccionada no existe.";
                await RecargarSucursalesAsync(Input.Username);
                UsuarioValidado = true;
                return Page();
            }

            if (sucursal.IdEmpresa != user.OEmpresa.IdEmpresa)
            {
                ErrorMessage = "La sucursal seleccionada no pertenece a la empresa del usuario.";
                await RecargarSucursalesAsync(Input.Username);
                UsuarioValidado = true;
                return Page();
            }

            var identityUser = await _userManager.FindByNameAsync(Input.Username);

            if (identityUser == null)
            {
                ErrorMessage = "No se pudo iniciar sesión.";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                identityUser,
                Input.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ErrorMessage = "No se pudo iniciar sesión.";
                await RecargarSucursalesAsync(Input.Username);
                UsuarioValidado = true;
                return Page();
            }

            await AgregarClaimsEmpresaSucursalAsync(identityUser, user.OEmpresa, sucursal);

            Response.Cookies.Append("sucursalSeleccionada", Input.Sucursal, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = false,
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return Redirect("/");
        }

        private void LimpiarEstadoVisual()
        {
            ErrorMessage = string.Empty;
            Sucursales = new List<SelectListItem>();
        }

        private async Task RecargarSucursalesAsync(string username)
        {
            var user = await _usuarioService.ObtenerUsuarioConEmpresaAsync(username);

            if (user == null || user.OEmpresa.IdEmpresa <= 0)
                return;

            NombreEmpresa = user.OEmpresa?.Nombre ?? string.Empty;

            var sucursales = await _negocioService.ListarSucursalesPorEmpresa(user.OEmpresa.IdEmpresa);

            Sucursales = sucursales.Select(x => new SelectListItem
            {
                Value = x.IdNegocio.ToString(),
                Text = x.Nombre
            }).ToList();
        }

        private async Task AgregarClaimsEmpresaSucursalAsync(Usuario identityUser, Empresa empresa, Negocio sucursal)
        {
            var claimsActuales = await _userManager.GetClaimsAsync(identityUser);

            var claimsAQuitar = claimsActuales
                .Where(c =>
                    c.Type == "IdEmpresa" ||
                    c.Type == "NombreEmpresa" ||
                    c.Type == "IdNegocio" ||
                    c.Type == "NombreNegocio" ||
                    c.Type == "NombreCompleto")
                .ToList();

            if (claimsAQuitar.Any())
            {
                await _userManager.RemoveClaimsAsync(identityUser, claimsAQuitar);
            }

            var nuevosClaims = new List<Claim>
    {
        new Claim("IdEmpresa", empresa.IdEmpresa.ToString()),
        new Claim("NombreEmpresa", empresa.Nombre ?? string.Empty),
        new Claim("IdNegocio", sucursal.IdNegocio.ToString()),
        new Claim("NombreNegocio", sucursal.Nombre ?? string.Empty),
        new Claim("NombreCompleto", identityUser.NombreCompleto ?? identityUser.UserName ?? string.Empty)
    };

            await _userManager.AddClaimsAsync(identityUser, nuevosClaims);

            
            await _signInManager.SignInWithClaimsAsync(identityUser, isPersistent: false, nuevosClaims);
        }
    }
}