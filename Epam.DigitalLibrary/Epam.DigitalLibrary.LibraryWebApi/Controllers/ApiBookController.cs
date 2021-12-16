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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        //[Authorize(Roles = UserRights.Librarian)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public Book Get(Guid id)
        {
            if (!IsBookExist(id, out Book book))
            {
                return null;
            }

            return _logic.GetBookById(id);          
        }

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

            _logic.AddNote(book, out Guid noteId);

            return Ok(noteId);
        }

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

            _logic.UpdateNote(id, updateBook);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!IsBookExist(id, out Book book))
            {
                return NotFound();
            }

            _logic.MarkForDelete(_logic.GetBookById(id));

            return Ok();
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
