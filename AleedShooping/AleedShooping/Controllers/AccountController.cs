using AleedShooping.Data;
using AleedShooping.Helpers;
using AleedShooping.Models;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AleedShooping.Controllers
{
    public class AccountController:Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public AccountController(IUserHelper userHelper, DataContext context,  IFlashMessage flashMessage)
        {
            _userHelper = userHelper;
            _context = context;
            _flashMessage = flashMessage;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _userHelper.LoginAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    _flashMessage.Danger("Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                }
                else if (result.IsNotAllowed)
                {
                    _flashMessage.Danger("El usuario no ha sido habilitado, debes de seguir las instrucciones enviadas al correo para poder habilitarlo.");
                }
                else
                {
                    _flashMessage.Danger("Email o contraseña incorrectos.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
