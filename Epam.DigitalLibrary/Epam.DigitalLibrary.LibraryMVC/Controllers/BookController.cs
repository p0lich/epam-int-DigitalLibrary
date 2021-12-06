using Epam.DigitalLibrary.CustomExeptions;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryMVC.Models;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly INoteLogic _logic;

        public BookController(ILogger<BookController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("Book/GetAllBooks/{id:int?}")]
        public ActionResult GetAllBooks(string searchString, int pageId = 1)
        {
            try
            {
                List<BookLinkViewModel> booksLink = _logic.GetCatalog().OfType<Book>().Select(n => new BookLinkViewModel(n)).ToList();

                ViewData["SearchFilter"] = searchString;

                if (!string.IsNullOrEmpty(searchString))
                {
                    booksLink = booksLink.Where(b => b.Name.Contains(searchString)).ToList();
                }

                var model = PagingList<BookLinkViewModel>.GetPageItems(booksLink, pageId, 20);

                return View(model);
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Reader + "," + UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Book/Details/{id:Guid}")]
        public ActionResult Details(Guid id)
        {
            try
            {
                Book book = _logic.GetBookById(id);

                BookDetailsViewModel bookDetails = new BookDetailsViewModel
                {
                    ID = book.ID,
                    Name = book.Name,
                    Authors = book.Authors,
                    PublicationPlace = book.PublicationPlace,
                    Publisher = book.Publisher,
                    PublicationDate = book.PublicationDate,
                    PagesCount = book.PagesCount,
                    ObjectNotes = book.ObjectNotes,
                    ISBN = book.ISBN,
                    IsDeleted = book.IsDeleted
                };

                return View(bookDetails);
            }
            
            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Book/Create")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.AvailableAuthors = new SelectList(
                    _logic.GetAvailableAuthors().Select(a => string.Format($"{a.FirstName}; {a.LastName}")),
                    "Select author");
                return View();
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Book/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookInputViewModel bookModel, List<AuthorView> authors)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                List<Author> inputAuthors = authors.Select(a => new Author(a.FirstName, a.LastName)).ToList();

                Book book = new Book(
                    name: bookModel.Name,
                    authors: inputAuthors,
                    publicationPlace: bookModel.PublicationPlace,
                    publisher: bookModel.Publisher,
                    publicationDate: bookModel.PublicationDate,
                    pagesCount: bookModel.PagesCount,
                    objectNotes: bookModel.ObjectNotes,
                    iSBN: bookModel.ISBN
                );

                int addResult = _logic.AddNote(book);

                if (addResult == ResultCodes.NoteExist)
                {
                    TempData["Error"] = "Same note already exist";
                    return View(nameof(Create));
                }

                if (addResult == ResultCodes.Error)
                {
                    TempData["Error"] = "Unable add note";
                    return View(nameof(Create));
                }

                _logger.LogInformation(2, "Note was added");
                return RedirectToAction(nameof(GetAllBooks));
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Book/Edit/{id:Guid}")]
        public ActionResult Edit(Guid id)
        {
            try
            {
                Book book = _logic.GetBookById(id);

                if (book is null)
                {
                    return NotFound();
                }

                BookInputViewModel bookModel = new BookInputViewModel
                {
                    Name = book.Name,
                    PublicationPlace = book.PublicationPlace,
                    Publisher = book.Publisher,
                    PublicationDate = book.PublicationDate,
                    PagesCount = book.PagesCount,
                    ObjectNotes = book.ObjectNotes,
                    ISBN = book.ISBN
                };

                return View(bookModel);
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Book/Edit/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id,
            [Bind("ID,Name,PublicationPlace,Publisher,PublicationDate,PagesCount,ObjectNotes,ISBN")] BookInputViewModel bookModel)
        {
            try
            {
                Book book = new Book(
                    name: bookModel.Name,
                    authors: _logic.GetBookById(id).Authors,
                    publicationPlace: bookModel.PublicationPlace,
                    publisher: bookModel.Publisher,
                    publicationDate: bookModel.PublicationDate,
                    pagesCount: bookModel.PagesCount,
                    objectNotes: bookModel.ObjectNotes,
                    iSBN: bookModel.ISBN
                    );

                int updateResult = _logic.UpdateNote(id, book);

                if (updateResult == ResultCodes.NoteExist)
                {
                    TempData["Error"] = "Same note already exist";
                    return View(nameof(Edit));
                }

                if (updateResult == ResultCodes.Error)
                {
                    TempData["Error"] = "Unable update note";
                    return View(nameof(Edit));
                }

                _logger.LogInformation(2, "Note was edited");
                return RedirectToAction(nameof(GetAllBooks));
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Book/Delete/{id:Guid}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                Book book = _logic.GetBookById(id);

                if (book is null)
                {
                    return NotFound();
                }

                BookDetailsViewModel bookModel = new BookDetailsViewModel
                {
                    ID = book.ID,
                    Name = book.Name,
                    Authors = book.Authors,
                    PublicationPlace = book.PublicationPlace,
                    Publisher = book.Publisher,
                    PublicationDate = book.PublicationDate,
                    PagesCount = book.PagesCount,
                    ObjectNotes = book.ObjectNotes,
                    ISBN = book.ISBN,
                    IsDeleted = book.IsDeleted
                };

                return View(bookModel);
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Book/Delete/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteDelete(Guid id)
        {
            try
            {
                Book book = _logic.GetBookById(id);

                bool deleteResult = _logic.RemoveNote(book);

                if (!deleteResult)
                {
                    TempData["Error"] = "Unable to delete note";
                    return View(nameof(Delete));
                }

                _logger.LogInformation(2, "Note was deleted");
                return RedirectToAction(nameof(GetAllBooks));
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [Route("Book/Mark/{id:Guid}")]
        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        public IActionResult Mark(Guid id)
        {
            try
            {
                Book book = _logic.GetBookById(id);

                if (book is null)
                {
                    return NotFound();
                }

                BookDetailsViewModel bookModel = new BookDetailsViewModel
                {
                    ID = book.ID,
                    Name = book.Name,
                    Authors = book.Authors,
                    PublicationPlace = book.PublicationPlace,
                    Publisher = book.Publisher,
                    PublicationDate = book.PublicationDate,
                    PagesCount = book.PagesCount,
                    ObjectNotes = book.ObjectNotes,
                    ISBN = book.ISBN,
                    IsDeleted = book.IsDeleted
                };

                return View(bookModel);
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }

        [HttpPost]
        [Route("Book/Mark/{id:Guid}")]
        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        public IActionResult MarkForDelete(Guid id)
        {
            try
            {
                Book book = _logic.GetBookById(id);

                _logic.MarkForDelete(book);

                return RedirectToAction(nameof(GetAllBooks));
            }

            catch (DataAccessException e)
            {
                _logger.LogInformation(4, "Error on data acces layer");
                return Redirect("/");
            }

            catch (BusinessLogicException)
            {
                _logger.LogInformation(4, "Error on business layer");
                return Redirect("/");
            }

            catch (Exception e) when (e is not DataAccessException && e is not BusinessLogicException)
            {
                _logger.LogInformation(4, "Unhandled exception");
                return Redirect("/");
            }
        }
    }
}
