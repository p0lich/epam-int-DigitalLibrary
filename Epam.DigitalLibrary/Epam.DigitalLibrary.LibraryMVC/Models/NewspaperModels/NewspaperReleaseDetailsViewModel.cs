using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperReleaseDetailsViewModel
    {
        public Guid ID { get; set; }

        public int PagesCount { get; set; }

        public string Number { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
