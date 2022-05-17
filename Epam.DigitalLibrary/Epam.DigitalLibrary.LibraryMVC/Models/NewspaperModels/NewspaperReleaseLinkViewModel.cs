using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperReleaseLinkViewModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public DateTime ReleaseDate { get; set; }

        public NewspaperReleaseLinkViewModel(Newspaper newspaper)
        {
            ID = newspaper.ID;
            Name = newspaper.Name;
            Number = newspaper.Number;
            ReleaseDate = newspaper.ReleaseDate;
        }

        public override string ToString()
        {
            string strNumber = string.IsNullOrEmpty(Number) ? "" : $"№{Number}/";
            return $"{Name} {strNumber}{ReleaseDate.Year}";
        }
    }
}
