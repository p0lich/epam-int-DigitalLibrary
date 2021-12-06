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
    }
}
