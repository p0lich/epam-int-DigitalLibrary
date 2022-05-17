using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperReleaseInputViewModel : IValidatableObject
    {
        [Required]
        [Range(0, short.MaxValue)]
        public int PagesCount { get; set; }

        public string Number { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReleaseDate.Year < 1400 || ReleaseDate.Year > DateTime.Now.Year)
            {
                yield return new ValidationResult(
                    "Release Date have wrong range",
                    new[] { nameof(ReleaseDate) }
                    );
            }
        }
    }
}
