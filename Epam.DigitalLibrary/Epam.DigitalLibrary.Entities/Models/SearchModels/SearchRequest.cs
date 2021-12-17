using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities.Models.SearchModels
{
    public class SearchRequest
    {
        public NoteTypes Type { get; set; } = NoteTypes.None;

        public string NamePattern { get; set; }

        public int? MinPagesCount { get; set; }

        public int? MaxPagesCount { get; set; }

        public int PageNumber { get; set; } = 1;

        public int ElementsCount { get; set; } = 20;
    }
}
