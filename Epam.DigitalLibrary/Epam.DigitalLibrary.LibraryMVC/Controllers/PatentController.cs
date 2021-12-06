using Epam.DigitalLibrary.CustomExeptions;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryMVC.Models;
using Epam.DigitalLibrary.LibraryMVC.Models.PatentModels;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Controllers
{
    public class PatentController : Controller
    {
        private readonly ILogger<PatentController> _logger;
        private readonly INoteLogic _logic;

        public PatentController(ILogger<PatentController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("Patent/GetAllPatents/{id:int?}")]
        public ActionResult GetAllPatents(string searchString, int pageId = 1)
        {
            try
            {
                List<PatentLinkViewModel> patentLinks = _logic.GetCatalog().OfType<Patent>().Select(p => new PatentLinkViewModel(p)).ToList();

                ViewData["SearchFilter"] = searchString;

                if (!string.IsNullOrEmpty(searchString))
                {
                    patentLinks = patentLinks.Where(b => b.Name.Contains(searchString)).ToList();
                }

                var model = PagingList<PatentLinkViewModel>.GetPageItems(patentLinks, pageId, 20);

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
        [Route("Patent/Details/{id:Guid}")]
        public ActionResult Details(Guid id)
        {
            try
            {
                Patent patent = _logic.GetPatentById(id);

                if (patent is null)
                {
                    return NotFound();
                }

                PatentDetailsViewModel patentDetails = new PatentDetailsViewModel
                {
                    ID = patent.ID,
                    Name = patent.Name,
                    Authors = patent.Authors,
                    Country = patent.Country,
                    RegistrationNumber = patent.RegistrationNumber,
                    ApplicationDate = patent.ApplicationDate,
                    PublicationDate = patent.PublicationDate,
                    PagesCount = patent.PagesCount,
                    ObjectNotes = patent.ObjectNotes,
                    IsDeleted = patent.IsDeleted
                };

                return View(patentDetails);
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
        [Route("Patent/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Patent/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatentInputViewModel patentInput, List<AuthorView> authors)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(patentInput);
                }

                List<Author> inputAuthors = authors.Select(a => new Author(a.FirstName, a.LastName)).ToList();

                Patent patent = new Patent(
                    name: patentInput.Name,
                    authors: inputAuthors,
                    country: patentInput.Country,
                    registrationNumber: patentInput.RegistrationNumber,
                    applicationDate: patentInput.ApplicationDate,
                    publicationDate: patentInput.PublicationDate,
                    pagesCount: patentInput.PagesCount,
                    objectNotes: patentInput.ObjectNotes
                    );

                int addResult = _logic.AddNote(patent);

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
                return RedirectToAction(nameof(GetAllPatents));
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
        [Route("Patent/Edit/{id:Guid}")]
        public ActionResult Edit(Guid id)
        {
            try
            {
                Patent patent = _logic.GetPatentById(id);

                if (patent is null)
                {
                    return NotFound();
                }

                PatentInputViewModel patentModel = new PatentInputViewModel()
                {
                    Name = patent.Name,
                    //Authors = patent.Authors,
                    Country = patent.Country,
                    RegistrationNumber = patent.RegistrationNumber,
                    ApplicationDate = patent.ApplicationDate,
                    PublicationDate = patent.PublicationDate,
                    PagesCount = patent.PagesCount,
                    ObjectNotes = patent.ObjectNotes,
                };

                return View(patentModel);
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
        [Route("Patent/Edit/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, PatentInputViewModel patentInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(patentInput);
                }

                Patent patent = new Patent(
                    name: patentInput.Name,
                    authors: _logic.GetPatentById(id).Authors,
                    country: patentInput.Country,
                    registrationNumber: patentInput.RegistrationNumber,
                    applicationDate: patentInput.ApplicationDate,
                    publicationDate: patentInput.PublicationDate,
                    pagesCount: patentInput.PagesCount,
                    objectNotes: patentInput.ObjectNotes
                    );

                int updateResult = _logic.UpdateNote(id, patent);

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
                return RedirectToAction(nameof(GetAllPatents));
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
        [Route("Patent/Delete/{id:Guid}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                Patent patent = _logic.GetPatentById(id);

                if (patent is null)
                {
                    return NotFound();
                }

                PatentDetailsViewModel patentDetails = new PatentDetailsViewModel
                {
                    ID = patent.ID,
                    Name = patent.Name,
                    Authors = patent.Authors,
                    Country = patent.Country,
                    RegistrationNumber = patent.RegistrationNumber,
                    ApplicationDate = patent.ApplicationDate,
                    PublicationDate = patent.PublicationDate,
                    PagesCount = patent.PagesCount,
                    ObjectNotes = patent.ObjectNotes,
                    IsDeleted = patent.IsDeleted
                };
      
                return View(patentDetails);
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
        [Route("Patent/Delete/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteDelete(Guid id)
        {
            try
            {
                Patent patent = _logic.GetPatentById(id);

                if (patent is null)
                {
                    return NotFound();
                }

                bool deleteResult = _logic.RemoveNote(patent);

                if (!deleteResult)
                {
                    TempData["Error"] = "Unable to delete note";
                    return View(nameof(Delete));
                }

                _logger.LogInformation(2, "Note was deleted");
                return RedirectToAction(nameof(GetAllPatents));
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
