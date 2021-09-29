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
                Regex regex = new Regex(@"^[A-Z]([a-z]+(-[A-Z])?[a-z]+)$"); // for EN language

                if (!regex.IsMatch(value) || value.Length > 50)
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
                Regex regex = new Regex(@"^(([a-z]{2,3} )?[A-Z]'?([a-z]+'?(-[A-Z])?'?[a-z]+'?[a-z]+))$"); // for EN language

                if (!regex.IsMatch(value) || value.Length > 200)
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

        public static bool operator !=(Author a1, Author a2)
        {
            if (a1.FirstName != a2.FirstName || a1.LastName != a2.LastName)
            {
                return true;
            }

            return false;
        }

        public static bool operator ==(Author a1, Author a2)
        {
            if (a1.FirstName == a2.FirstName && a1.LastName == a2.LastName)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "First name: " + FirstName + "\n" +
                   "Last name: " + LastName;
        }
    }
}
