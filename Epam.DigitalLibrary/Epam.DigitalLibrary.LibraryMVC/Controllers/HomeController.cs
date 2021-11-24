using Epam.DigitalLibrary.LibraryMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.Logic;
using Epam.DigitalLibrary.Entities;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INoteLogic _logic;
        private readonly IUserRightsProvider _userLogic;

        public HomeController(ILogger<HomeController> logger, INoteLogic logic, IUserRightsProvider userLogic)
        {
            _logger = logger;
            _logic = logic;
            _userLogic = userLogic;

            //_logger.LogDebug(1, "NLog was injected");

            //for (int i = 0; i < 10; i++)
            //{
            //    _logic.AddNote(new Book(
            //    name: ("book" + (i + 1).ToString()),
            //    authors: new List<Author> { new Author("Ivan", "Karasev"), new Author("Aleksei", "Ivanov") },
            //    publicationPlace: "Saratov",
            //    publisher: "booker",
            //    publicationDate: new DateTime(1900, 01, 01),
            //    pagesCount: 50,
            //    objectNotes: "aoaoaoaoa",
            //    iSBN: null
            //    ));
            //}
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = UserRights.Reader)]
        public IActionResult GetLibrary(int pageId = 1)
        {
            List<BookLinkViewModel> booksLink = _logic.GetCatalog().OfType<Book>().Select(n => new BookLinkViewModel(n)).ToList();

            var model = PagingList<BookLinkViewModel>.GetPageItems(booksLink, pageId, 20);

            return View(model);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            //ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Validate(UserView user)
        {
            //ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            if (!_userLogic.IsCredentialRight())
            {
                TempData["Error"] = "Error. Login/Password is invalid";
                return View("Login");
            }

            var claims = new List<Claim>();

            claims.Add(new Claim("login", user.Login));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Login));
            claims.Add(new Claim(ClaimTypes.Name, user.Login));

            List<string> roles = _userLogic.GetRoles();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            _logger.LogInformation(0, $"User {user.Login} has log in");

            await HttpContext.SignInAsync(claimsPrincipal);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet("Denied")]
        public IActionResult DeniedPage()
        {
            _logger.LogWarning(1, "Unauthorized access attempt");
            return View();
        }
    }
}
