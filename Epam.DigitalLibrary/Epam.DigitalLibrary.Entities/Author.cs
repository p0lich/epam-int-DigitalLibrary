using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Author
    {
        private string _firstName, _lastName;

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                Regex regex = new Regex(@"^[A-Z]([a-z]+|[a-z]+-[A-Z][a-z]+){1,49}$"); // for EN language

                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                Regex regex = new Regex(@"^[A-Z]([a-z]+|[a-z]+-[A-Z][a-z]+){1,49}$"); // for EN language

                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _lastName = value;
            }
        }

        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return "First name: " + FirstName + "\n" +
                   "Last name: " + LastName;
        }
    }
}
