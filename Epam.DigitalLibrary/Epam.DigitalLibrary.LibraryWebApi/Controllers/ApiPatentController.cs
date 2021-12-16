using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models.PatentModels;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Epam.DigitalLibrary.LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiPatentController : ControllerBase
    {
        private readonly ILogger<ApiPatentController> _logger;
        private readonly INoteLogic _logic;

        public ApiPatentController(ILogger<ApiPatentController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if (!IsPatentExist(id, out Patent patent))
            {
                return NotFound($"Cannot find note with id: {id}");
            }

            return Ok(_logic.GetPatentById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] PatentInputViewModel patentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model is not valid");
            }

            Patent patent = new Patent(
                name: patentModel.Name,
                authors: patentModel.AuthorsId.Select(authId => _logic.GetAuthor(authId)).ToList(),
                country: patentModel.Country,
                registrationNumber: patentModel.RegistrationNumber,
                applicationDate: patentModel.ApplicationDate,
                publicationDate: patentModel.PublicationDate,
                pagesCount: patentModel.PagesCount,
                objectNotes: patentModel.ObjectNotes
                );

            int addResult = _logic.AddNote(patent, out Guid noteId);

            return Ok(FillCreateError(addResult));
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] PatentInputViewModel patentModel)
        {
            if (!IsPatentExist(id, out Patent patent))
            {
                return NotFound($"Cannot find note with id: {id}");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("model is not valid");
            }

            Patent updatePatent = new Patent(
                name: patentModel.Name,
                authors: patentModel.AuthorsId.Select(authId => _logic.GetAuthor(authId)).ToList(),
                country: patentModel.Country,
                registrationNumber: patentModel.RegistrationNumber,
                applicationDate: patentModel.ApplicationDate,
                publicationDate: patentModel.PublicationDate,
                pagesCount: patentModel.PagesCount,
                objectNotes: patentModel.ObjectNotes
                );

            int updateResult = _logic.UpdateNote(id, updatePatent);

            return Ok(FillUpdateError(updateResult));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!IsPatentExist(id, out Patent patent))
            {
                return NotFound($"Cannot find note with id: {id}");
            }

            bool deleteResult = _logic.MarkForDelete(patent);

            return Ok(FillDeleteError(deleteResult));
        }

        private bool IsPatentExist(Guid noteId, out Patent foundPatent)
        {
            foundPatent = _logic.GetPatentById(noteId);
            return foundPatent is not null;
        }

        private string FillCreateError(int addResult)
        {
            string errorMessage = null;

            if (addResult == ResultCodes.NoteExist)
            {
                errorMessage = "Cannot add. Same patent already exist";
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
                errorMessage = "Cannot update. Same patent already exist";
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
