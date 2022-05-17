using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Author : IEquatable<Author>
    {
        private string _firstName, _lastName;

        public Guid ID { get; set; }

        public string FirstName
        {
            get => _firstName;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                Regex regex = new Regex(@"^(([A-Z]([a-z]*(-[A-Z])?[a-z]+))|([А-ЯЁ]([а-яё]*(-[А-ЯЁ])?[а-яё]+)))$");

                if (!regex.IsMatch(value) || value.Length == 0 || value.Length > 50)
                {
                    throw new ArgumentException();
                }

                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                Regex regex = new Regex(@"^((([a-z]{2,3} )?[A-Z][a-z]*('?[a-z]+)?(-[A-Z][a-z]*('?[a-z]+))?)|(([а-яё]{2,3} )?[А-ЯЁ][а-яё]*('?[а-яё]+)?(-[А-ЯЁ][а-яё]*('?[а-яё]+))?))$");

                if (!regex.IsMatch(value) || value.Length > 200)
                {
                    throw new ArgumentException();
                }

                _lastName = value;
            }
        }

        public Author() { }

        public Author(string firstName, string lastName)
        {
            ID = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }

        public Author(Guid id, string firstName, string lastName)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public static bool operator !=(Author a1, Author a2)
        {
            if (a1 is null || a2 is null)
            {
                return !(a1 is null && a2 is null);
            }

            return (a1.FirstName != a2.FirstName || a1.LastName != a2.LastName);
        }

        public static bool operator ==(Author a1, Author a2)
        {
            if (a1 is null || a2 is null)
            {
                return a1 is null && a2 is null;
            }

            return (a1.FirstName == a2.FirstName && a1.LastName == a2.LastName);
        }

        public bool Equals(Author other)
        {
            if (other is null)
            {
                return false;
            }

            return this == other;
        }

        public override bool Equals(object obj)
        {
            Author auth = obj as Author;
            return ((auth is not null) && this == auth);
        }

        public override int GetHashCode()
        {
            return (FirstName, LastName).GetHashCode();
        }

        public override string ToString()
        {
            return "First name: " + FirstName + "\n" +
                   "Last name: " + LastName;
        }
    }
}
