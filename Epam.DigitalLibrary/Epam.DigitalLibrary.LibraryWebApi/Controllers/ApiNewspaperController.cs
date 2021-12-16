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

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewspaperInputViewModel newspaperModel)
        {
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] NewspaperInputViewModel value)
        {
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
        }

        private bool IsNewspaperExist(Guid noteId, out Newspaper foundNewspaper)
        {
            foundNewspaper = _logic.GetNewspaperById(noteId);
            return foundNewspaper is not null;
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
