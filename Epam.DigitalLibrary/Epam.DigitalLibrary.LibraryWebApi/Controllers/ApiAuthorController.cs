using Epam.DigitalLibrary.AppCodes;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuthorController : ControllerBase
    {
        private readonly ILogger<ApiAuthorController> _logger;
        private readonly INoteLogic _logic;

        public ApiAuthorController(ILogger<ApiAuthorController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if (!IsAuthorExist(id, out Author author))
            {
                return NotFound();
            }

            return Ok(author);
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPost]
        public IActionResult Post([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Validation error");
            }

            int addResult = _logic.AddAuthor(author, out Guid id);

            if (addResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"Author {id} was added | User: {User.Identity.Name}");
                return Ok(id);
            }

            return BadRequest(FillCreateError(addResult));
        }

        [Authorize(Roles = UserRights.Librarian + "," + UserRights.Admin + "," + UserRights.ExternalClient)]
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Author authorUpdate)
        {
            if (!IsAuthorExist(id, out Author author))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Validation error");
            }

            int updateResult = _logic.UpdateAuthor(id, authorUpdate);

            if (updateResult == ResultCodes.Successfull)
            {
                _logger.LogInformation(2, $"Author {id} was udpated | User: {User.Identity.Name}");
                return Ok($"Author {id} was updated");
            }

            return BadRequest(FillUpdateError(updateResult));
        }

        private bool IsAuthorExist(Guid id, out Author author)
        {
            author = _logic.GetAuthor(id);
            return author is not null;
        }

        private object FillCreateError(int addResult)
        {
            string errorMessage = null;

            if (addResult == ResultCodes.NoteExist)
            {
                errorMessage = "Cannot add. Same author already exist";
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
                errorMessage = "Cannot update. Same author already exist";
            }

            if (updateResult == ResultCodes.Error)
            {
                errorMessage = "Cannot update. Unexpected error";
            }

            return errorMessage;
        }
    }
}
