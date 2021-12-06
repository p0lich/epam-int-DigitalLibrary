﻿using Epam.DigitalLibrary.CustomExeptions;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryMVC.Models;
using Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels;
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
    public class NewspaperController : Controller
    {
        private readonly ILogger<NewspaperController> _logger;
        private readonly INoteLogic _logic;

        public NewspaperController(ILogger<NewspaperController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("Newspaper/GetNewspaperReleases/{id:Guid}/{pageId:int?}")]
        public ActionResult GetNewspaperReleases(Guid id, string searchString, int pageId = 1)
        {
            try
            {
                IEnumerable<Newspaper> newspaperReleases = _logic.GetNewspaperReleases(id);

                List<NewspaperReleaseLinkViewModel> newspaperLinks = newspaperReleases
                    .OfType<Newspaper>()
                    .Select(n => new NewspaperReleaseLinkViewModel(n))
                    .ToList();

                ViewData["SearchFilter"] = searchString;

                if (!string.IsNullOrEmpty(searchString))
                {
                    newspaperLinks = newspaperLinks.Where(b => b.Name.Contains(searchString)).ToList();
                }

                var model = PagingList<NewspaperReleaseLinkViewModel>.GetPageItems(newspaperLinks, pageId, 20);

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

        [Route("Newspaper/GetAllReleases/{id:int?}")]
        public ActionResult GetAllReleases(string searchString, int pageId = 1)
        {
            try
            {
                List<NewspaperReleaseLinkViewModel> newspaperLinks = _logic.GetCatalog()
                .OfType<Newspaper>()
                .Select(n => new NewspaperReleaseLinkViewModel(n))
                .ToList();

                ViewData["SearchFilter"] = searchString;

                if (!string.IsNullOrEmpty(searchString))
                {
                    newspaperLinks = newspaperLinks.Where(b => b.Name.Contains(searchString)).ToList();
                }

                var model = PagingList<NewspaperReleaseLinkViewModel>.GetPageItems(newspaperLinks, pageId, 20);

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

        [Authorize(Roles = UserRights.Reader +"," + UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Newspaper/Details/{id:Guid}")]
        public ActionResult Details(Guid id)
        {
            try
            {
                Newspaper newspaper = _logic.GetNewspaperById(id);

                if (newspaper is null)
                {
                    return NotFound();
                }

                NewspaperDetailsViewModel newspaperDetails = new NewspaperDetailsViewModel()
                {
                    Name = newspaper.Name,
                    PublicationPlace = newspaper.PublicationPlace,
                    Publisher = newspaper.Publisher,
                    PublicationDate = newspaper.PublicationDate,
                    ObjectNotes = newspaper.ObjectNotes,
                    ISSN = newspaper.ISSN
                };

                NewspaperReleaseDetailsViewModel releaseDetails = new NewspaperReleaseDetailsViewModel()
                {
                    ID = newspaper.ID,
                    PagesCount = newspaper.PagesCount,
                    Number = newspaper.Number,
                    ReleaseDate = newspaper.ReleaseDate,
                    IsDeleted = newspaper.IsDeleted
                };

                NewspaperFullDetails newspaperFullDetails = new NewspaperFullDetails()
                {
                    NewspaperDetails = newspaperDetails,
                    ReleaseDetails = releaseDetails
                };

                return View(newspaperFullDetails);
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
        [Route("Newspaper/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin)]
        [Route("Newspaper/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewspaperFullInput newspaperFullInput)
        {
            try
            {
                NewspaperInputViewModel newspaperInput = newspaperFullInput.NewspaperInput;
                NewspaperReleaseInputViewModel releaseInput = newspaperFullInput.ReleaseInput;

                if (newspaperInput is null || releaseInput is null)
                {
                    return View();
                }

                Newspaper newspaper = new Newspaper(
                    name: newspaperInput.Name,
                    publicationPlace: newspaperInput.PublicationPlace,
                    publisher: newspaperInput.Publisher,
                    publicationDate: newspaperInput.PublicationDate,
                    pagesCount: releaseInput.PagesCount,
                    objectNotes: newspaperInput.ObjectNotes,
                    number: releaseInput.Number,
                    releaseDate: releaseInput.ReleaseDate,
                    iSSN: newspaperInput.ISSN
                    );

                int addResult = _logic.AddNote(newspaper);

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
                return RedirectToAction(nameof(GetAllReleases));
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
        [Route("Newspaper/Edit/{id:Guid}")]
        public ActionResult Edit(Guid id)
        {
            try
            {
                Newspaper newspaper = _logic.GetNewspaperById(id);

                if (newspaper is null)
                {
                    return NotFound();
                }

                NewspaperReleaseInputViewModel releaseDetails = new NewspaperReleaseInputViewModel()
                {
                    PagesCount = newspaper.PagesCount,
                    Number = newspaper.Number,
                    ReleaseDate = newspaper.ReleaseDate
                };

                return View(releaseDetails);
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
        [Route("Newspaper/Edit/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, NewspaperReleaseInputViewModel releaseInput)
        {
            try
            {
                Newspaper newspaper = _logic.GetNewspaperById(id);

                if (newspaper is null)
                {
                    return NotFound();
                }

                Newspaper updatedNewspaper = new Newspaper(
                    name: newspaper.Name,
                    publicationPlace: newspaper.PublicationPlace,
                    publisher: newspaper.Publisher,
                    publicationDate: newspaper.PublicationDate,
                    pagesCount: releaseInput.PagesCount,
                    objectNotes: newspaper.ObjectNotes,
                    number: releaseInput.Number,
                    releaseDate: releaseInput.ReleaseDate,
                    iSSN: newspaper.ISSN
                    );

                int updateResult = _logic.UpdateNote(id, updatedNewspaper);

                if(updateResult == ResultCodes.NoteExist)
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
                return RedirectToAction(nameof(GetAllReleases));
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
        [Route("Newspaper/Delete/{id:Guid}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                Newspaper newspaper = _logic.GetNewspaperById(id);

                if (newspaper is null)
                {
                    return NotFound();
                }

                NewspaperDetailsViewModel newspaperDetails = new NewspaperDetailsViewModel()
                {
                    Name = newspaper.Name,
                    PublicationPlace = newspaper.PublicationPlace,
                    Publisher = newspaper.Publisher,
                    PublicationDate = newspaper.PublicationDate,
                    ObjectNotes = newspaper.ObjectNotes,
                    ISSN = newspaper.ISSN
                };

                NewspaperReleaseDetailsViewModel releaseDetails = new NewspaperReleaseDetailsViewModel()
                {
                    ID = newspaper.ID,
                    PagesCount = newspaper.PagesCount,
                    Number = newspaper.Number,
                    ReleaseDate = newspaper.ReleaseDate,
                    IsDeleted = newspaper.IsDeleted
                };

                NewspaperFullDetails newspaperFullDetails = new NewspaperFullDetails()
                {
                    NewspaperDetails = newspaperDetails,
                    ReleaseDetails = releaseDetails
                };

                return View(newspaperFullDetails);
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
        [Route("Newspaper/Delete/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteDelete(Guid id)
        {
            try
            {
                Newspaper newspaper = _logic.GetNewspaperById(id);

                if (newspaper is null)
                {
                    return NotFound();
                }

                bool deleteResult = _logic.RemoveNote(newspaper);

                if (!deleteResult)
                {
                    TempData["Error"] = "Unable to delete note";
                    return View(nameof(Delete));
                }

                _logger.LogInformation(2, "Note was deleted");
                return RedirectToAction(nameof(GetAllReleases));
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
