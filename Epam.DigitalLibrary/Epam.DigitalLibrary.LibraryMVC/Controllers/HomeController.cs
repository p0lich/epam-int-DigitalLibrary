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

        public HomeController(ILogger<HomeController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;

            _logic.AddNote(new Book(
                name: "book1",
                authors: new List<Author> { new Author("Ivan", "Karasev"), new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "booker",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 1-56389-668-0"
                ));
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

        [HttpGet]
        public IActionResult InputNewspaper()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InputNewspaper(Newspaper newspaper)
        {
            _logic.AddNote(newspaper);
            return RedirectToAction("GetLibrary");
        }




        [HttpGet]
        public IActionResult InputPatent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InputPatent(Patent patent)
        {
            _logic.AddNote(patent);
            return RedirectToAction("GetLibrary");
        }
    }
}
