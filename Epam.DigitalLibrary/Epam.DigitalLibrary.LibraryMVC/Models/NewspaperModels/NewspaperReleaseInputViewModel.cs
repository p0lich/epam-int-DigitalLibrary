using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperReleaseInputViewModel
    {
        [Required]
        [Range(0, short.MaxValue)]
        public int PagesCount { get; set; }

        public string Number { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
}
