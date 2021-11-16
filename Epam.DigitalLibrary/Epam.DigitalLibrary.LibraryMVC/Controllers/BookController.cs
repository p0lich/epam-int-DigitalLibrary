using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryMVC.Models;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INoteLogic _logic;

        public BookController(ILogger<HomeController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        // GET: BookController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BookController/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookInputViewModel bookModel)
        {
            try
            {
                Book book = GetBook(bookModel);

                _logic.AddNote(book);

                return RedirectToAction(nameof(HomeController.GetLibrary));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, BookInputViewModel bookModel)
        {
            try
            {
                Book book = GetBook(bookModel);

                _logic.UpdateNote(id, book);

                return RedirectToAction(nameof(HomeController.GetLibrary));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, BookInputViewModel bookModel)
        {
            try
            {
                Book book = GetBook(bookModel);

                _logic.RemoveNote(book);

                return RedirectToAction(nameof(HomeController.GetLibrary));
            }
            catch
            {
                return View();
            }
        }

        private Book GetBook(BookInputViewModel bookModel)
        {
            return new Book(
                    name: bookModel.Name,
                    authors: new List<Author>() { new Author("Ivan", "Karasev"), new Author("Aleksei", "Alekseev") },
                    publicationPlace: bookModel.PublicationPlace,
                    publisher: bookModel.Publisher,
                    publicationDate: bookModel.PublicationDate,
                    pagesCount: bookModel.PagesCount,
                    objectNotes: bookModel.ObjectNotes,
                    iSBN: bookModel.ISBN
                );
        }
    }
}
