using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models;
using Epam.DigitalLibrary.Entities.Models.SearchModels;
using Epam.DigitalLibrary.LibraryWebApi.Models;
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
    public class SearchController : ControllerBase
    {

        private readonly ILogger<SearchController> _logger;
        private readonly INoteLogic _logic;

        public SearchController(ILogger<SearchController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpPost("GetNotes")]
        public SearchResponse GetNotes([FromBody] SearchRequest searchRequest)
        {
            return _logic.GetFilteredShortNotes(searchRequest, searchRequest.Type);
        }

        [HttpGet("GetAuthors")]
        public IEnumerable<Author> GetAuthors(string namePattern)
        {
            return _logic.GetFilteredAuthors(namePattern);
        }
    }
}
