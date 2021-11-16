using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class NewspaperInputViewModel
    {
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string PublicationPlace { get; set; }

        [Required]
        [MaxLength(300)]
        public string Publisher { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public int PagesCount { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ObjectNotes { get; set; }

        public string Number { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public string ISSN { get; set; }
    }
}
