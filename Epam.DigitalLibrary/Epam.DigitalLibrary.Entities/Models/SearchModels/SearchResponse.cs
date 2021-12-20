using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities.Models.SearchModels
{
    public class SearchResponse
    {
        public int PageNumber { get; set; }

        public int PagesCount { get; set; }

        public List<ShortNote> FoundNotes { get; set; }

        [JsonIgnore]
        public int ElementsCount { get; set; }
    }
}
