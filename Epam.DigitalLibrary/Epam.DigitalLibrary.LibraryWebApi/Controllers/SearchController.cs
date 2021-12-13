using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryWebApi.Models;
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
    public class SearchController : ControllerBase
    {

        private readonly ILogger<SearchController> _logger;
        private readonly INoteLogic _logic;

        public SearchController(ILogger<SearchController> logger, INoteLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet("GetNotes")]
        public IEnumerable<NoteLink> GetNotes(string type, string name, int minPages = 0, int maxPages = short.MaxValue)
        {
            var notes = _logic.GetCatalog().Select(n => new NoteLink()
            {
                //Type = n.GetType(),
                Id = n.ID,
                Name = n.Name,
                PagesCount = n.PagesCount
            }).Where(n => n.PagesCount >= minPages && n.PagesCount <= maxPages);

            if (!string.IsNullOrEmpty(name))
            {
                notes = notes.Where(n => n.Name.Contains(name));
            }

            return notes;
        }

        [HttpGet("GetAuthors")]
        public IEnumerable<Author> GetAuthors(string name)
        {
            IEnumerable<Author> authors = _logic.GetAvailableAuthors();

            if (!string.IsNullOrEmpty(name))
            {
                authors = authors.Where(a => string.Format($"{a.FirstName} {a.LastName}").Contains(name));
            }

            return authors;
        }
    }
}
