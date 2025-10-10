using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LPNEWSTORE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public LoginModel(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }

            [Required(ErrorMessage = "Debe seleccionar una sucursal.")]
            public string Sucursal { get; set; }
        }

        public void OnGet()
        {
            // Si quieres limpiar errores u otra lógica al entrar a la página
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Debe completar todos los campos.";
                return Page();
            }

            var user = await _userManager.FindByNameAsync(Input.Username);
            if (user == null)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user, Input.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            // Guardar sucursal en cookie una sola vez
            Response.Cookies.Append("sucursalSeleccionada", Input.Sucursal, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = false,
                IsEssential = true,
                Expires = System.DateTimeOffset.UtcNow.AddDays(1)
            });

            return Redirect("/");
        }

    }
}
