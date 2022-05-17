using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class UserView : IValidatableObject
    {
        private string _login;

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z]+((_?[A-Za-z0-9]+)|([A-Za-z0-9]*))+[A-Za-z0-9]$")]
        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value?.ToLower();
            }
        }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password.Contains(Login))
            {
                yield return new ValidationResult(
                    "Login cannot be part of password.",
                    new[] { nameof(Password) }
                    );
            }
        }
    }
}
