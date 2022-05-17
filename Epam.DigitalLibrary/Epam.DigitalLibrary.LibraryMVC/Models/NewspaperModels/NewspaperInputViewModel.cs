using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperInputViewModel : IValidatableObject
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        [RegularExpression(@"^(([A-Z]([a-z]*( [A-Z]?)?[a-z]+(-[A-Z])?[a-z]*( [A-Z]?)?[a-z]+))|([А-ЯЁ]([а-яё]*( [А-ЯЁ]?)?[а-яё]+(-[А-ЯЁ])?[а-яё]*( [А-ЯЁ]?)?[а-яё]+)))$")]
        public string PublicationPlace { get; set; }

        [Required]
        [StringLength(300)]
        public string Publisher { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [StringLength(2000)]
        public string ObjectNotes { get; set; }

        [StringLength(13)]
        [RegularExpression(@"^ISSN[0-9]{4}-[0-9]{4}$")]
        public string ISSN { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PublicationDate.Year < 1400 || PublicationDate.Year > DateTime.Now.Year)
            {
                yield return new ValidationResult(
                    "Date wrong range",
                    new[] { nameof(PublicationDate) }
                    );
            }
        }
    }
}
