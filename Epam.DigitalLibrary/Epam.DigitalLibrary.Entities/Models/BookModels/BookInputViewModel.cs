using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;

namespace Epam.DigitalLibrary.Entities.Models.BookModels
{
    public class BookInputViewModel : IValidatableObject
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        public List<Guid> AuthorsId { get; set; }

        [Required]
        [StringLength(200)]
        [RegularExpression(@"^(([A-Z]([a-z]*( [A-Z]?)?[a-z]+((-[A-Z])|(-[a-z]+-[A-Z]))?[a-z]*( [A-Z]?)?[a-z]+))|([А-ЯЁ]([а-яё]*( [А-ЯЁ]?)?[а-яё]+((-[А-ЯЁ])|(-[а-яё]+-[А-ЯЁ]))?[а-яё]*( [А-ЯЁ]?)?[а-яё]+)))$")]
        public string PublicationPlace { get; set; }

        [Required]
        [StringLength(300)]
        public string Publisher { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        public int PagesCount { get; set; }

        [StringLength(2000)]
        public string ObjectNotes { get; set; }

        [StringLength(18)]
        [RegularExpression(@"^ISBN ((999[0-9]{2})|(99[4-8][0-9])|(9(([5-8][0-9])|(9[0-3])))|((8[0-9])|(9[0-4]))|[0-7])-[0-9]{1,7}-[0-9]{1,7}-[0-9X]$")]
        public string ISBN { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PublicationDate.Year < 1400 || PublicationDate.Year > DateTime.Now.Year)
            {
                yield return new ValidationResult(
                    "Date have wrong range",
                    new[] { nameof(PublicationDate) }
                    );
            }
        }
    }
}
