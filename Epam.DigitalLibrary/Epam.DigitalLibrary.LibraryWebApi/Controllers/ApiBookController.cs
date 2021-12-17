using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.Logic;
using Epam.DigitalLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Epam.DigitalLibrary.AppCodes;
using Microsoft.AspNetCore.Http;
using Epam.DigitalLibrary.Entities.Models.BookModels;

namespace Epam.DigitalLibrary.LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBookController : ControllerBase
    {
        private readonly ILogger<ApiBookController> _logger;
        private readonly INoteLogic _logic;

        public ApiBookController(ILogger<ApiBookController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if (!IsBookExist(id, out Book book))
            {
                return NotFound();
            }

            return Ok(_logic.GetBookById(id));          
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPost]
        public IActionResult Post([FromBody] BookInputViewModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            Book book = new Book(
                name: bookModel.Name,
                authors: bookModel.AuthorsId.Select(a => _logic.GetAuthor(a)).ToList(),
                publicationPlace: bookModel.PublicationPlace,
                publisher: bookModel.Publisher,
                publicationDate: bookModel.PublicationDate,
                pagesCount: bookModel.PagesCount,
                objectNotes: bookModel.ObjectNotes,
                iSBN: bookModel.ISBN
                );

            int addResult = _logic.AddNote(book, out Guid noteId);

            if (addResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"New book was added | User : {User.Identity.Name}");
                return Ok(noteId);
            }

            return BadRequest(FillCreateError(addResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] BookInputViewModel bookModel)
        {
            if (!IsBookExist(id, out Book book))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return null;
            }

            Book updateBook = new Book(
                name: bookModel.Name,
                authors: bookModel.AuthorsId.Select(a => _logic.GetAuthor(a)).ToList(),
                publicationPlace: bookModel.PublicationPlace,
                publisher: bookModel.Publisher,
                publicationDate: bookModel.PublicationDate,
                pagesCount: bookModel.PagesCount,
                objectNotes: bookModel.ObjectNotes,
                iSBN: bookModel.ISBN
                );

            int updateResult = _logic.UpdateNote(id, updateBook);

            if (updateResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"Book {id} was updated | User : {User.Identity.Name}");
                return Ok("Book was updated");
            }

            return BadRequest(FillUpdateError(updateResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!IsBookExist(id, out Book book))
            {
                return NotFound();
            }

            bool deleteResult = _logic.MarkForDelete(_logic.GetBookById(id));

            if (deleteResult)
            {
                _logger.LogInformation(2, $"Book {id} was marked for delet | User : {User.Identity.Name}");
                return Ok("Book was deleted");
            }

            return Ok(FillDeleteError(deleteResult));
        }

        private bool IsBookExist(Guid noteId, out Book foundBook)
        {
            foundBook = _logic.GetBookById(noteId);
            return foundBook is not null;
        }

        private object FillCreateError(int addResult)
        {
            string errorMessage = null;

            if (addResult == ResultCodes.NoteExist)
            {
                errorMessage = "Cannot add. Same book already exist";
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
                errorMessage = "Cannot update. Same book already exist";
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
