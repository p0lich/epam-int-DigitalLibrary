using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryMVC.CustomIdentity;
using Epam.DigitalLibrary.LibraryMVC.Models;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IUserRoleProvider _userLogic;

        public AuthenticateController(ILogger<AuthenticateController> logger, IUserRoleProvider userLogic)
        {
            _logger = logger;
            _userLogic = userLogic;
        }

        [Route("Authenticate/Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Authenticate/Login")]
        public async Task<IActionResult> LoginValidate(UserView userView)
        {
            //ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            int authResult = _userLogic.AuthenticateUser(userView.Login, userView.Password);

            if (authResult == AuthenticationCodes.NotExist)
            {
                TempData["Error"] = "Error. User with such login isn't exist";
                return View(nameof(Login));
            }

            if (authResult == AuthenticationCodes.PasswordMismatch)
            {
                TempData["Error"] = "Error. Password is invalid";
                return View(nameof(Login));
            }

            var claims = _userLogic.GetUserClaims(userView.Login);

            _logger.LogInformation(2, $"User {userView.Login} has log in");

            await HttpContext.SignInAsync(_userLogic.GetPrincipals(claims));
            return Redirect("/");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterValidate(UserRegisterView registerUser)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Register));
            }

            int registerResult = _userLogic.RegisterUser(registerUser.Login, registerUser.Password);

            if (registerResult != AuthenticationCodes.RegisterSuccess)
            {
                TempData["Error"] = "Unable to register user";
                return View(nameof(Register));
            }

            var claims = _userLogic.GetUserClaims(registerUser.Login);

            _logger.LogInformation(2, $"User {registerUser.Login} has registered");

            await HttpContext.SignInAsync(_userLogic.GetPrincipals(claims));
            return Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [Route("Denied")]
        public ActionResult Denied()
        {
            _logger.LogWarning(1, "Unauthorized access attempt. Leave this page");
            return View();
        }
    }
}
