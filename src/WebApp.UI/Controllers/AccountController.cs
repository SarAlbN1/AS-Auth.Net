using Microsoft.AspNetCore.Mvc;
using WebApp.UI.Services;

namespace WebApp.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiClient _authApi;

        public AccountController(IAuthApiClient authApi)
        {
            _authApi = authApi;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var result = await _authApi.LoginAsync(userName, password);

            ViewBag.Message = result.Message;
            ViewBag.IsSuccess = result.Success;

            return View();
        }
    }
}
