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

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INoteLogic _logic;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logic = new LibraryLogic();
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
            var model = _logic.GetCatalog();

            return View(model);
        }

        [HttpPost]
        public IActionResult InputBook(Book book)
        {
            _logic.AddNote(book);
            return RedirectToAction("GetLibrary");
        }

        [HttpPost]
        public IActionResult InputNewspaper(Newspaper newspaper)
        {
            _logic.AddNote(newspaper);
            return RedirectToAction("GetLibrary");
        }

        [HttpPost]
        public IActionResult InputPatent(Patent patent)
        {
            _logic.AddNote(patent);
            return RedirectToAction("GetLibrary");
        }
    }
}
