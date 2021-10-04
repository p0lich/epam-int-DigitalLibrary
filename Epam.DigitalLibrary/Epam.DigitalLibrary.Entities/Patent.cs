using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Patent : Note
    {
        private DateTime _publicationDate;
        private DateTime? _applicationDate;
        private string _country, _registrationNumber;

        // At the moment inventors are authors
        public List<Author> Authors { get; set; }

        public string Country
        {
            get
            {
                return _country;
            }

            private set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                Regex regex = new Regex(@"^(([A-Z][a-z]+|[A-Z]{2,})|([А-ЯЁ][а-яё]+|[А-ЯЁ]{2,}))$"); // For EN language

                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _country = value;
            }
        }

        public string RegistrationNumber
        {
            get
            {
                return _registrationNumber;
            }

            private set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                if (!new Regex(@"^[0-9]{1,9}$").IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _registrationNumber = value;
            }
        }

        public DateTime? ApplicationDate
        {
            get
            {
                return _applicationDate;
            }

            private set
            {
                if (value.HasValue)
                {
                    if (value.Value.Year < 1474)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }

                _applicationDate = value;
            }
        }

        public override DateTime PublicationDate
        {
            get
            {
                return _publicationDate;
            }

            protected set
            {
                if (_applicationDate.HasValue)
                {
                    if (value < _applicationDate.Value)
                    {
                        throw new ArgumentException();
                    }
                }

                if (value.Year < 1474 || value > DateTime.Now)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _publicationDate = value;
            }
        }

        public Patent(string name, string objectNotes, int pagesCount,
            List<Author> authors, string country, string registrationNumber, DateTime? applicationDate, DateTime publicationDate) :
            base(name, objectNotes, pagesCount, publicationDate)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            Country = country;
            RegistrationNumber = registrationNumber;
            ApplicationDate = applicationDate;
            PublicationDate = publicationDate;
        }

        public override string ToString()
        {
            StringBuilder inventors = new StringBuilder();

            for (int i = 0; i < Authors.Count; i++)
            {
                inventors.Append(Authors[i].ToString() + "\n");
            }

            return $"Name: {Name};\n" +
                   $"Inventors:\n{inventors}" +
                   $"Country: {Country};\n" +
                   $"Registration number: {RegistrationNumber};\n" +
                   $"Application date: {(ApplicationDate is null ? "N/A" : ApplicationDate.ToString())};\n" +
                   $"Publication date: {PublicationDate};\n" +
                   $"Page count: {PagesCount};\n" +
                   $"Patent notes: {ObjectNotes ?? "N/A"};\n";
        }
    }
}
