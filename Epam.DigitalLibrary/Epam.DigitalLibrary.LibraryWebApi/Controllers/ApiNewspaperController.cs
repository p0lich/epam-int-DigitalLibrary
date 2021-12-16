using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models.NewspaperModels;
using Epam.DigitalLibrary.LogicContracts;
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
        private readonly Logger<ApiNewspaperController> _logger;
        private readonly INoteLogic _logic;

        public ApiNewspaperController(Logger<ApiNewspaperController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet("GetNewspaperReleases/{id}")]
        public IActionResult GetNewspaperReleases(Guid id)
        {
            if (!IsNewspaperExist(id, out Newspaper newspaper))
            {
                return NotFound("Release with such id isnt exist");
            }

            var releases = _logic.GetNewspaperReleases(id).Select(r => new NewspaperReleaseDetailsViewModel()
            {
                ID = r.ID,
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

        [HttpPost("PostNewspaper")]
        public IActionResult PostNewspaper([FromBody] NewspaperInputViewModel newspaperModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int addResult = _logic.AddNewspaperRelease(newspaperModel, out Guid id);

            return Ok(FillCreateMessage(addResult, id));
        }

        [HttpPost("PostNewspaperRelease")]
        public IActionResult PostNewspaperRelease([FromBody] NewspaperFullInput newspaperModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Newspaper newspaper = new Newspaper(
                name: newspaperModel.NewspaperInput.Name,
                publicationPlace: newspaperModel.NewspaperInput.PublicationPlace,
                publisher: newspaperModel.NewspaperInput.Publisher,
                publicationDate: newspaperModel.NewspaperInput.PublicationDate,
                pagesCount: newspaperModel.ReleaseInput.PagesCount,
                objectNotes: newspaperModel.NewspaperInput.ObjectNotes,
                number: newspaperModel.ReleaseInput.Number,
                releaseDate: newspaperModel.ReleaseInput.ReleaseDate,
                iSSN: newspaperModel.NewspaperInput.ISSN
                );

            int addresult = _logic.AddNote(newspaper, out Guid id);

            return Ok(FillCreateMessage(addresult, id));
        }

        [HttpPut("UpdateNewspaper/{id}")]
        public IActionResult UpdateNewspaper(Guid id, [FromBody] NewspaperInputViewModel newspaperModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int updateResult = _logic.UpdateNewspaperInfo(id, newspaperModel);

            return Ok(FillUpdateError(updateResult));
        }

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

            return Ok(FillUpdateError(updateResult));
        }

        [HttpDelete("DeleteNewspaper/{id}")]
        public IActionResult DeleteNewspaper(Guid id)
        {
            bool deleteResult = _logic.MarkForDeleteNewspaperRelease(id);

            return Ok(FillDeleteError(deleteResult));
        }

        [HttpDelete("DeleteNewspaperRelease/{id}")]
        public IActionResult DeleteNewspaperRelease(Guid id)
        {
            if (!IsNewspaperExist(id, out Newspaper newspaper))
            {
                return BadRequest();
            }

            bool deleteResult = _logic.MarkForDelete(newspaper);

            return Ok(deleteResult);
        }

        private bool IsNewspaperExist(Guid noteId, out Newspaper foundNewspaper)
        {
            foundNewspaper = _logic.GetNewspaperById(noteId);
            return foundNewspaper is not null;
        }

        private object FillCreateMessage(int addResult, Guid id)
        {
            string message = id.ToString();

            if (addResult == ResultCodes.NoteExist)
            {
                message = "Cannot add. Same newspaper already exist";
            }

            if (addResult == ResultCodes.Error)
            {
                message = "Cannot add. Unexpected error";
            }

            return message;
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
