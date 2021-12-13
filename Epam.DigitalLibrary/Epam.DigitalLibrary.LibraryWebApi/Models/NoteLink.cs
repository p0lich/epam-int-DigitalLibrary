using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Models
{
    public class NoteLink
    {
        //public Type Type { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int PagesCount { get; set; }
    }
}
