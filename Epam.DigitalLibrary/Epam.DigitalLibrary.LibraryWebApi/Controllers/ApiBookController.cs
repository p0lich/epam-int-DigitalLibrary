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

        [Authorize(Roles = UserRights.Librarian)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public Book Get(Guid id)
        {
            return _logic.GetBookById(id);          
        }

        [HttpPost]
        public IActionResult Post([FromForm] Book value)
        {
            _logic.AddNote(value);

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromForm] Book value)
        {
            if (!IsBookExist(id, out Book book))
            {
                return NotFound();
            }

            _logic.UpdateNote(id, value);

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
    }
}
