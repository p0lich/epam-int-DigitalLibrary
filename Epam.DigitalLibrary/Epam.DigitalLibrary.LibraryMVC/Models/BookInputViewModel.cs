using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class BookInputViewModel
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        //public List<Author> Authors { get; set; }

        [Required]
        [StringLength(200)]
        public string PublicationPlace { get; set; }

        [Required]
        [StringLength(300)]
        public string Publisher { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public int PagesCount { get; set; }

        [StringLength(2000)]
        public string ObjectNotes { get; set; }

        [StringLength(18)]
        public string ISBN { get; set; }
    }
}
