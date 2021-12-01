using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.PatentModels
{
    public class PatentLinkViewModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public DateTime PublicationDate { get; set; }

        public PatentLinkViewModel(Patent patent)
        {
            ID = patent.ID;
            Name = patent.Name;
            PublicationDate = patent.PublicationDate;
        }

        public override string ToString()
        {
            return $"<<{Name}>> от {PublicationDate.ToString("dd.MM.yyyy")}";
        }
    }
}
