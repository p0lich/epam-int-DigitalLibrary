using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.PatentModels
{
    public class PatentDetailsViewModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public List<Author> Authors { get; set; }

        public string Country { get; set; }

        public string RegistrationNumber { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public DateTime PublicationDate { get; set; }

        public int PagesCount { get; set; }

        public string ObjectNotes { get; set; }

        public bool IsDeleted { get; set; }
    }
}
