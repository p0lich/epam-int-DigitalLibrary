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

        // GET: NewspaperController
        public ActionResult Index()
        {
            return View();
        }

        [Route("Newspaper/GetNewspaperReleases/{id:Guid}/{pageId:int?}")]
        public ActionResult GetNewspaperReleases(Guid id, int pageId = 1)
        {
            IEnumerable<Newspaper> newspaperReleases = _logic.GetNewspaperReleases(id);

            List<NewspaperReleaseLinkViewModel> newspaperLinks = newspaperReleases
                .OfType<Newspaper>()
                .Select(n => new NewspaperReleaseLinkViewModel(n))
                .ToList();

            var model = PagingList<NewspaperReleaseLinkViewModel>.GetPageItems(newspaperLinks, pageId, 20);

            return View(model);
        }

        [Route("Newspaper/GetAllReleases/{id:int?}")]
        public ActionResult GetAllReleases(int pageId = 1)
        {
            List<NewspaperReleaseLinkViewModel> newspaperLinks = _logic.GetCatalog()
                .OfType<Newspaper>()
                .Select(n => new NewspaperReleaseLinkViewModel(n))
                .ToList();

            var model = PagingList<NewspaperReleaseLinkViewModel>.GetPageItems(newspaperLinks, pageId, 20);

            return View(model);
        }

        // GET: NewspaperController/Details/5
        [Route("Newspaper/Details/{id:Guid}")]
        public ActionResult Details(Guid id)
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

        // GET: NewspaperController/Create
        [Authorize(Roles = UserRights.Librarian)]
        [Route("Newspaper/Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewspaperController/Create
        [HttpPost]
        [Authorize(Roles = UserRights.Librarian)]
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

                int inputResult = _logic.AddNote(newspaper);

                if (inputResult == ResultCodes.NoteExist)
                {
                    TempData["SameNewspaper"] = "Same newspaper release already exist";
                    return View();
                }

                return RedirectToAction(nameof(GetAllReleases));
            }

            catch
            {
                return View();
            }
        }

        // GET: NewspaperController/Edit/5
        [Authorize(Roles = UserRights.Librarian)]
        [Route("Newspaper/Edit/{id:Guid}")]
        public ActionResult Edit(Guid id)
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

        // POST: NewspaperController/Edit/5
        [HttpPost]
        [Authorize(Roles = UserRights.Librarian)]
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

                if (updateResult == ResultCodes.NoteExist)
                {
                    TempData["SameNewspaper"] = "Same newspaper release already exist";
                    return View();
                }

                return RedirectToAction(nameof(GetAllReleases));
            }

            catch
            {
                return View();
            }
        }

        // GET: NewspaperController/Delete/5
        [Authorize(Roles = UserRights.Librarian)]
        [Route("Newspaper/Delete/{id:Guid}")]
        public ActionResult Delete(Guid id)
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

        // POST: NewspaperController/Delete/5
        [HttpPost]
        [Authorize(Roles = UserRights.Librarian)]
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

                _logic.RemoveNote(newspaper);

                return RedirectToAction(nameof(GetAllReleases));
            }

            catch
            {
                return View();
            }
        }
    }
}
