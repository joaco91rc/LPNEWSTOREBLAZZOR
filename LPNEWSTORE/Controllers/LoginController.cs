using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Entities; // Asegurate de usar el namespace correcto donde está tu clase Usuario

namespace LPNEWSTORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public LoginController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return Unauthorized("Usuario o contraseña inválidos");

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
                return Unauthorized("Usuario o contraseña inválidos");

            return Ok();
        }
    }
}