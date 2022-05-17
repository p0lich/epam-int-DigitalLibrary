using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperFullInput : IValidatableObject
    {
        public NewspaperInputViewModel NewspaperInput { get; set; }

        public NewspaperReleaseInputViewModel ReleaseInput { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReleaseInput.ReleaseDate.Year < NewspaperInput.PublicationDate.Year)
            {
                yield return new ValidationResult(
                    "Release date cannot be less than Publication date",
                    new[] { nameof(ReleaseInput.ReleaseDate) }
                    );
            }
        }
    }
}
