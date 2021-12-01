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

        // GET: PatentController
        public ActionResult Index()
        {
            return View();
        }

        [Route("Patent/GetAllPatents/{id:int?}")]
        public ActionResult GetAllPatents(int pageId = 1)
        {
            List<PatentLinkViewModel> patentLinks = _logic.GetCatalog().OfType<Patent>().Select(p => new PatentLinkViewModel(p)).ToList();

            var model = PagingList<PatentLinkViewModel>.GetPageItems(patentLinks, pageId, 20);

            return View(model);
        }

        // GET: PatentController/Details/5
        [Authorize(Roles = UserRights.Reader)]
        [Route("Patent/Details/{id:Guid}")]
        public ActionResult Details(Guid id)
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

        // GET: PatentController/Create
        [Authorize(Roles = UserRights.Librarian)]
        [Route("Patent/Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatentController/Create
        [HttpPost]
        [Authorize(Roles = UserRights.Librarian)]
        [Route("Patent/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatentInputViewModel patentInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(patentInput);
                }

                Patent patent = new Patent(
                    name: patentInput.Name,
                    authors: new List<Author>() { new Author("Smart", "Boy"), new Author("Clever", "Girl") },
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
                    TempData["SamePatent"] = "Same patent already exist";
                    return View(patentInput);
                }

                return RedirectToAction(nameof(GetAllPatents));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatentController/Edit/5
        [Authorize(Roles = UserRights.Librarian)]
        [Route("Patent/Edit/{id:Guid}")]
        public ActionResult Edit(Guid id)
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

        // POST: PatentController/Edit/5
        [HttpPost]
        [Authorize(Roles = UserRights.Librarian)]
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
                    authors: new List<Author>() { new Author("Smart", "Boy"), new Author("Clever", "Girl") },
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
                    TempData["SamePatent"] = "Same patent already exist";
                    return View(patentInput);
                }

                return RedirectToAction(nameof(GetAllPatents));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatentController/Delete/5
        [Authorize(Roles = UserRights.Librarian)]
        [Route("Patent/Delete/{id:Guid}")]
        public ActionResult Delete(Guid id)
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

        // POST: PatentController/Delete/5
        [HttpPost]
        [Authorize(Roles = UserRights.Librarian)]
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

                _logic.RemoveNote(patent);

                return RedirectToAction(nameof(GetAllPatents));
            }
            catch
            {
                return View();
            }
        }
    }
}
