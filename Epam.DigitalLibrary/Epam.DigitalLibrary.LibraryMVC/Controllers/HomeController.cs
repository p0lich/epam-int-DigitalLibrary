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

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult GetLibrary()
        {
            string login = "lib_admin";
            System.Security.SecureString password = new System.Security.SecureString();

            password.AppendChar('1');
            password.AppendChar('2');
            password.AppendChar('3');

            password.MakeReadOnly();

            INoteLogic logic = new LibraryLogic(login, password);

            var model = logic.GetCatalog();

            return View(model);
        }
    }
}
