using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Book : Note
    {
        private string _publisher, _publicationPlace, _isbn;
        private DateTime _publicationDate;

        public List<Author> Authors { get; set; }

        public string PublicationPlace
        {
            get
            {
                return _publicationPlace;
            }

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                Regex regex = new Regex(@"^( [A-Z]?)?[A-Z]([a-z]*( [A-Z]?)?[a-z]+((-[A-Z])|(-[a-z]+-[A-Z]))?[a-z]*( [A-Z]?)?[a-z]+)$");

                if (!regex.IsMatch(value) || value.Length > 200)
                {
                    throw new ArgumentException();
                }

                _publicationPlace = value;
            }
        }

        public string Publisher
        {
            get
            {
                return _publisher;
            }

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                if (value.Length > 300)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _publisher = value;
            }
        }

        public override DateTime PublicationDate
        {
            get
            {
                return _publicationDate;
            }

            set
            {
                if (value.Year < 1400 || value.Year > DateTime.Now.Year)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _publicationDate = value;
            }
        }

        public string ISBN
        {
            get
            {
                return _isbn;
            }

            set
            {
                if (value is null)
                {
                    _isbn = null;
                    return;
                }

                Regex regex = new Regex(@"^ISBN ((999[0-9]{2})|(99[4-8][0-9])|(9(([5-8][0-9])|(9[0-3])))|((8[0-9])|(9[0-4]))|[0-7])-[0-9]{1,7}-[0-9]{1,7}-[0-9X]$");

                if (!regex.IsMatch(value) || value.Length != 18)
                {
                    throw new ArgumentException();
                }

                _isbn = value;
            }
        }

        public Book(string name, string objectNotes, int pagesCount,
            List<Author> authors, string publicationPlace, string publisher, DateTime publicationDate, string iSBN) :
            base(name, objectNotes, pagesCount, publicationDate)
        {
            Authors = authors;
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            PublicationDate = publicationDate;
            ISBN = iSBN;
        }
    }
}
