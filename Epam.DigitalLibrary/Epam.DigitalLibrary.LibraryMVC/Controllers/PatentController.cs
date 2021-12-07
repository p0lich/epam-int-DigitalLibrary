using Epam.DigitalLibrary.CustomExeptions;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryMVC.Models;
using Epam.DigitalLibrary.LibraryMVC.Models.PatentModels;
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
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Reader + "," + UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Patent/Details/{id:Guid}")]
        public ActionResult Details(Guid id)
        {
            try
            {
                if (!IsPatentExist(id, out Patent patent))
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
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Patent/Create")]
        public ActionResult Create()
        {
            DownloadAvailableAuthors();
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
                    DownloadAvailableAuthors();
                    return View();
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

                if (FillCreateError(addResult) is not null)
                {
                    return RedirectToAction(nameof(Create));
                }

                _logger.LogInformation(2, $"Presentation layer | User: {User.Identity.Name} | Note was added");
                return RedirectToAction(nameof(GetAllPatents));
            }

            catch (DataAccessException e)
            {
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Patent/Edit/{id:Guid}")]
        public ActionResult Edit(Guid id)
        {
            try
            {
                if (!IsPatentExist(id, out Patent patent))
                {
                    return NotFound();
                }

                PatentInputViewModel patentModel = new PatentInputViewModel()
                {
                    Name = patent.Name,
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
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
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
                if (!IsPatentExist(id, out Patent patent))
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return View(patentInput);
                }

                Patent updatedPatent = new Patent(
                    name: patentInput.Name,
                    authors: _logic.GetPatentById(id).Authors,
                    country: patentInput.Country,
                    registrationNumber: patentInput.RegistrationNumber,
                    applicationDate: patentInput.ApplicationDate,
                    publicationDate: patentInput.PublicationDate,
                    pagesCount: patentInput.PagesCount,
                    objectNotes: patentInput.ObjectNotes
                    );

                int updateResult = _logic.UpdateNote(id, updatedPatent);

                if (FillUpdateError(updateResult) is not null)
                {
                    return RedirectToAction(nameof(Edit));
                }

                _logger.LogInformation(2, $"Presentation layer | User: {User.Identity.Name} | Note was edited");
                return RedirectToAction(nameof(GetAllPatents));
            }

            catch (DataAccessException e)
            {
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Patent/Delete/{id:Guid}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                if(!IsPatentExist(id, out Patent patent))
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
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
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
                if (!IsPatentExist(id, out Patent patent))
                {
                    return NotFound();
                }

                bool deleteResult = _logic.RemoveNote(patent);

                if (FillDeleteError(deleteResult) is not null)
                {
                    return RedirectToAction(nameof(Delete));
                }

                _logger.LogInformation(2, $"Presentation layer | User: {User.Identity.Name} | Note was deleted");
                return RedirectToAction(nameof(GetAllPatents));
            }

            catch (DataAccessException e)
            {
                _logger.LogError(4, $"Error on data acces layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (BusinessLogicException e)
            {
                _logger.LogError(4, $"Error on business layer |Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }

            catch (Exception e)
            {
                _logger.LogError(4, $"Unhandled exception | Method: {e.TargetSite.Name} | User: {User.Identity.Name} | Exception Path: {e.StackTrace}");
                return Redirect("/");
            }
        }

        private bool IsPatentExist(Guid noteId, out Patent foundPatent)
        {
            foundPatent = _logic.GetPatentById(noteId);
            return foundPatent is not null;
        }

        private object FillCreateError(int addResult)
        {
            if (addResult == ResultCodes.NoteExist)
            {
                TempData["Error"] = "Cannot add. Same note already exist";
            }

            if (addResult == ResultCodes.Error)
            {
                TempData["Error"] = "Cannot add. Unexpected error";
            }

            return TempData["Error"];
        }

        private object FillUpdateError(int updateResult)
        {
            if (updateResult == ResultCodes.NoteExist)
            {
                TempData["Error"] = "Cannot update. Same note already exist";
            }

            if (updateResult == ResultCodes.Error)
            {
                TempData["Error"] = "Cannot update. Unexpected error";
            }

            return TempData["Error"];
        }

        private object FillDeleteError(bool deleteResult)
        {
            if (deleteResult == ResultCodes.ErrorDelete)
            {
                TempData["Error"] = "Cannot delete. Undexpected error";
            }

            return TempData["Error"];
        }

        private bool DownloadAvailableAuthors()
        {
            ViewBag.AvailableAuthors = new SelectList(
                    _logic.GetAvailableAuthors().Select(a => string.Format($"{a.FirstName}; {a.LastName}")),
                    "Select author");

            return true;
        }
    }
}