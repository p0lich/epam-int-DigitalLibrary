using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities.Models.SearchModels
{
    public class ShortNote
    {
        public NoteTypes Type { get; set; } = NoteTypes.None;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int PagesCount { get; set; }
    }
}
