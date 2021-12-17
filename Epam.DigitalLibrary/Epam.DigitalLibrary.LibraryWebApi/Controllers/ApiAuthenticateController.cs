using Epam.DigitalLibrary.LibraryWebApi.Models;
using Epam.DigitalLibrary.LibraryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthenticateController : ControllerBase
    {
        private readonly ILogger<ApiAuthenticateController> _logger;
        private readonly IUserService _userService;

        public ApiAuthenticateController(ILogger<ApiAuthenticateController> logger, IUserService userService)
        {
            _logger = logger;
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

            _logger.LogInformation(2, $"User {response.Login} has login");

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

            _logger.LogInformation(2, $"New user {response.Login} has registered");

            return Ok(response);
        }
    }
}
