using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models;
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

        [HttpGet("GetNotes")]
        public PagingList<NoteLink> GetNotes(string type, string name, int pageNumber = 1, int elementsCount = 20, int minPages = 0, int maxPages = short.MaxValue)
        {
            var notes = _logic.GetCatalog();

            if (!string.IsNullOrEmpty(type))
            {
                notes = notes.Where(n => n.GetType() == GetNoteType(type)).ToList();
            }

            var shortNotes = notes.Select(n => new NoteLink
            {
                Type = n.GetType().ToString(),
                Id = n.ID,
                Name = n.Name,
                PagesCount = n.PagesCount
            }).Where(n => n.PagesCount >= minPages && n.PagesCount <= maxPages);          

            if (!string.IsNullOrEmpty(name))
            {
                shortNotes = shortNotes.Where(n => n.Name.Contains(name));
            }

            if (pageNumber > shortNotes.Count() / elementsCount + 1)
            {
                throw new ValidationException("page index was out of bounds");
            }

            var notesOnPage = PagingList<NoteLink>.GetPageItems(shortNotes.ToList(), pageNumber, elementsCount);

            return notesOnPage;
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

        private Type GetNoteType(string type)
        {
            if (type == "Book")
            {
                return typeof(Book);
            }

            if (type == "Newspaper")
            {
                return typeof(Newspaper);
            }

            if (type == "Patent")
            {
                return typeof(Patent);
            }

            throw new ArgumentException();
        }
    }
}
