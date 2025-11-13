using Microsoft.AspNetCore.Mvc;

namespace WebApp.UI.Controllers
{
    public class AccountController : Controller
    {
        // TODO: inyectar HttpClient e IConfiguration para hablar con Auth.Api

        [HttpGet]
        public IActionResult Login()
        {
            // TODO: devolver vista de login
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            // TODO: consumir Auth.Api vía HttpClient y validar credenciales
            // TODO: mostrar mensaje de éxito o error en la vista

            return View();
        }
    }
}
