using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LPNEWSTORE.Pages
{
    public class LogoutModel : PageModel
    {
       

        private readonly SignInManager<Usuario> _signInManager;

        public LogoutModel(SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();

            // Eliminar la cookie de sucursal
            Response.Cookies.Delete("sucursalSeleccionada");

            return RedirectToPage("/Login");
        }
    }
}
