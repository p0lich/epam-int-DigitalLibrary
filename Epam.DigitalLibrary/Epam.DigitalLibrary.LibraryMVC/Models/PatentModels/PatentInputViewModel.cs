using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.PatentModels
{
    public class PatentInputViewModel
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        //public List<Author> Authors { get; set; }

        [Required]
        [StringLength(200)]
        public string Country { get; set; }

        [Required]
        [StringLength(9)]
        public string RegistrationNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApplicationDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        public int PagesCount { get; set; }

        [StringLength(2000)]
        public string ObjectNotes { get; set; }
    }
}
