using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities.Models.PatentModels
{
    public class PatentInputViewModel : IValidatableObject
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        public List<Guid> AuthorsId { get; set; }

        [Required]
        [StringLength(200)]
        [RegularExpression(@"^(([A-Z][a-z]+|[A-Z]{2,})|([А-ЯЁ][а-яё]+|[А-ЯЁ]{2,}))$")]
        public string Country { get; set; }

        [Required]
        [StringLength(9)]
        [RegularExpression(@"^[0-9]{1,9}$")]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ApplicationDate.HasValue)
            {
                if (ApplicationDate.Value.Year < 1474 || ApplicationDate.Value.Year > DateTime.Now.Year)
                {
                    yield return new ValidationResult(
                        "Appplication Date have wrong range",
                        new[] { nameof(ApplicationDate) }
                        );
                }

                if (PublicationDate.Year < ApplicationDate.Value.Year)
                {
                    yield return new ValidationResult(
                        "Publication Date cannot be less than Application Date",
                        new[] { nameof(PublicationDate) }
                        );
                }
            }

            if (PublicationDate.Year < 1474 || PublicationDate.Year > DateTime.Now.Year)
            {
                yield return new ValidationResult(
                    "Date have wrong range",
                    new[] { nameof(PublicationDate) }
                    );
            }
        }
    }
}
