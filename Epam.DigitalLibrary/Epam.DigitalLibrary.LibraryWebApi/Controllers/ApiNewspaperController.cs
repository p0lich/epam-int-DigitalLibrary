using Epam.DigitalLibrary.AppCodes;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models.NewspaperModels;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Epam.DigitalLibrary.LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiNewspaperController : ControllerBase
    {
        private readonly ILogger<ApiNewspaperController> _logger;
        private readonly INoteLogic _logic;

        public ApiNewspaperController(ILogger<ApiNewspaperController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet("GetNewspaperReleases/{id}")]
        public IActionResult GetNewspaperReleases(Guid id)
        {
            if (!IsNewspaperExist(id, out Newspaper newspaperRelease))
            {
                return NotFound("Release with such id isnt exist");
            }

            var releases = _logic.GetNewspaperReleases(id).Select(r => new NewspaperReleaseDetailsViewModel()
            {
                ID = r.ID,
                ReleaseId = r.ReleaseId,
                PagesCount = r.PagesCount,
                Number = r.Number,
                ReleaseDate = r.ReleaseDate,
                IsDeleted = r.IsDeleted,
            });

            return Ok(releases);
        }

        [HttpGet("GetRelease/{id}")]
        public IActionResult GetRelease(Guid id)
        {
            if (!IsNewspaperExist(id, out Newspaper newspaper))
            {
                return NotFound("Such release isnt exist");
            }

            return Ok(_logic.GetNewspaperById(id));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPost("PostNewspaper")]
        public IActionResult PostNewspaper([FromBody] NewspaperInputViewModel newspaperModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int addResult = _logic.AddNewspaperRelease(newspaperModel, out Guid id);

            if (addResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"Newspaper {id} was created | User: {User.Identity.Name}");
                return Ok(id);
            }

            return BadRequest(FillCreateError(addResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPost("PostNewspaperRelease")]
        public IActionResult PostNewspaperRelease([FromBody] NewspaperFullInput newspaperModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            NewspaperDetailsViewModel newspaperDetailsView = _logic.GetNewspaperDetails(newspaperModel.NewspaperReleaseInputId);

            Newspaper newspaper = new Newspaper(
                noteType: NoteTypes.Newspaper,
                releaseId: newspaperModel.ReleaseInput.ReleaseId,
                name: newspaperDetailsView.Name,
                publicationPlace: newspaperDetailsView.PublicationPlace,
                publisher: newspaperDetailsView.Publisher,
                publicationDate: newspaperDetailsView.PublicationDate,
                pagesCount: newspaperModel.ReleaseInput.PagesCount,
                objectNotes: newspaperDetailsView.ObjectNotes,
                number: newspaperModel.ReleaseInput.Number,
                releaseDate: newspaperModel.ReleaseInput.ReleaseDate,
                iSSN: newspaperDetailsView.ISSN
                );

            int addResult = _logic.AddNote(newspaper, out Guid id);

            if (addResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"Release {id} of newspaper was created | User: {User.Identity.Name}");
                return Ok(id);
            }

            return BadRequest(FillCreateError(addResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPut("UpdateNewspaper/{id}")]
        public IActionResult UpdateNewspaper(Guid id, [FromBody] NewspaperInputViewModel newspaperModel)
        {
            if (!IsNewspaperReleaseExist(id, out NewspaperDetailsViewModel newspaperRelease))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int updateResult = _logic.UpdateNewspaperInfo(id, newspaperModel);

            if (updateResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"Newspaper {id} was updated | User: {User.Identity.Name}");
                return Ok("Newspaper was updated");
            }

            return BadRequest(FillUpdateError(updateResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPut("UpdateNewspaperRelease/{id}")]
        public IActionResult UpdateNewspaperRelease(Guid id, [FromBody] NewspaperReleaseInputViewModel newspaperReleaseModel)
        {
            if (!IsNewspaperExist(id, out Newspaper newspaper))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Newspaper newspaperUpdate = new Newspaper(
                name: newspaper.Name,
                releaseId: newspaper.ReleaseId,
                publicationPlace: newspaper.PublicationPlace,
                publisher: newspaper.Publisher,
                publicationDate: newspaper.PublicationDate,
                pagesCount: newspaperReleaseModel.PagesCount,
                objectNotes: newspaper.ObjectNotes,
                number: newspaperReleaseModel.Number,
                releaseDate: newspaperReleaseModel.ReleaseDate,
                iSSN: newspaper.ISSN
                );

            int updateResult = _logic.UpdateNote(id, newspaperUpdate);

            if (updateResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"Release {id} of newspaper was updated | User: {User.Identity.Name}");
                return Ok("Release of newspaper was updated");
            }

            return BadRequest(FillUpdateError(updateResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpDelete("DeleteNewspaper/{id}")]
        public IActionResult DeleteNewspaper(Guid id)
        {
            if (!IsNewspaperReleaseExist(id, out NewspaperDetailsViewModel newspaperRelease))
            {
                return NotFound();
            }

            bool deleteResult = _logic.MarkForDeleteNewspaperRelease(id);

            if (deleteResult)
            {
                _logger.LogInformation(2, $"Newspaper {id} was marked for delete | User: {User.Identity.Name}");
                return Ok($"Newspaper {id} was marked for delete");
            }

            return BadRequest(FillDeleteError(deleteResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpDelete("DeleteNewspaperRelease/{id}")]
        public IActionResult DeleteNewspaperRelease(Guid id)
        {
            if (!IsNewspaperExist(id, out Newspaper newspaper))
            {
                return NotFound();
            }

            bool deleteResult = _logic.MarkForDelete(newspaper);

            if (deleteResult)
            {
                _logger.LogInformation(2, $"Newspaper {id} of newspaper was marked for delete | User: {User.Identity.Name}");
                return Ok($"Release {id} of newspaper was marked for delete");
            }

            return BadRequest(FillDeleteError(deleteResult));
        }

        private bool IsNewspaperExist(Guid noteId, out Newspaper foundNewspaper)
        {
            foundNewspaper = _logic.GetNewspaperById(noteId);
            return foundNewspaper is not null;
        }

        private bool IsNewspaperReleaseExist(Guid newspaperReleaseId, out NewspaperDetailsViewModel foundNewspaperRelease)
        {
            foundNewspaperRelease = _logic.GetNewspaperDetails(newspaperReleaseId);
            return foundNewspaperRelease is not null;
        }

        private object FillCreateError(int addResult)
        {
            string errorMessage = null;

            if (addResult == ResultCodes.NoteExist)
            {
                errorMessage = "Cannot add. Same newspaper already exist";
            }

            if (addResult == ResultCodes.Error)
            {
                errorMessage = "Cannot add. Unexpected error";
            }

            return errorMessage;
        }

        private object FillUpdateError(int updateResult)
        {
            string errorMessage = null;

            if (updateResult == ResultCodes.NoteExist)
            {
                errorMessage = "Cannot update. Same newspaper already exist";
            }

            if (updateResult == ResultCodes.Error)
            {
                errorMessage = "Cannot update. Unexpected error";
            }

            return errorMessage;
        }

        private object FillDeleteError(bool deleteResult)
        {
            string errorMessage = null;

            if (deleteResult == ResultCodes.ErrorDelete)
            {
                errorMessage = "Cannot delete. Undexpected error";
            }

            return errorMessage;
        }
    }
}
