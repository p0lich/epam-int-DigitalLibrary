using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class AuthorView
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^(([A-Z]([a-z]*(-[A-Z])?[a-z]+))|([А-ЯЁ]([а-яё]*(-[А-ЯЁ])?[а-яё]+)))$")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        [RegularExpression(@"^((([a-z]{2,3} )?[A-Z][a-z]*('?[a-z]+)?(-[A-Z][a-z]*('?[a-z]+))?)|(([а-яё]{2,3} )?[А-ЯЁ][а-яё]*('?[а-яё]+)?(-[А-ЯЁ][а-яё]*('?[а-яё]+))?))$")]
        public string LastName { get; set; }
    }
}
