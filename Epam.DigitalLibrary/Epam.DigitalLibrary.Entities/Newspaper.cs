using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Newspaper : Note
    {
        private string _publicationPlace, _publisher, _issn;
        private DateTime _publicationDate, _releaseDate;
        private int _pagesCount;

        public string PublicationPlace
        {
            get
            {
                return _publicationPlace;
            }

            set
            {
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

        public string Number { get; set; }

        public DateTime ReleaseDate
        {
            get
            {
                return _releaseDate;
            }

            set
            {
                if (_publicationDate != value)
                {
                    throw new ArgumentException();
                }

                _releaseDate = value;
            }
        }

        public string ISSN
        {
            get
            {
                return _issn;
            }

            set
            {
                Regex regex = new Regex(@"^ISSN\x20[0-9]{4}-[0-9]{4}$");

                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _issn = value;
            }
        }

        public Newspaper(string name, string objectNotes, int pagesCount, DateTime publicationDate,
            string publicationPlace, string publisher, string number, DateTime releaseDate, string iSSN) :
            base(name, objectNotes, pagesCount, publicationDate)
        {
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            PublicationDate = publicationDate;
            Number = number;
            ReleaseDate = releaseDate;
            ISSN = iSSN;
        }
    }
}
