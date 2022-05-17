using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities.Models.NewspaperModels
{
    public class NewspaperDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PublicationPlace { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ObjectNotes { get; set; }
        public string ISSN { get; set; }
    }
}
