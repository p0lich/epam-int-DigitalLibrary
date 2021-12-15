using Epam.DigitalLibrary.LibraryWebApi.Models;
using Epam.DigitalLibrary.LibraryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Epam.DigitalLibrary.LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;

        public ApiAuthenticateController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromForm] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(new AuthenticateRequest() { Login = model.Login, Password = model.Password });

            if (response is null)
            {
                return BadRequest(new { message = "Wrong login/password" });
            }

            return Ok(response);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromForm] AuthenticateRequest model)
        {
            var response = _userService.Register(new AuthenticateRequest() { Login = model.Login, Password = model.Password });

            if (response is null)
            {
                return BadRequest(new { message = "Wrong data" });
            }

            return Ok(response);
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            throw new NotImplementedException();

            var login = User.Identity.Name;
            _userService.LogOut(login);
            return Ok();
        }
    }
}
