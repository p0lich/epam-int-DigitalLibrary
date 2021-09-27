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

        public string ISBN
        {
            get
            {
                return _isbn;
            }

            set
            {
                Regex regex = new Regex(@"^ISBN\x20(?=.{13}$)[0-9]{1,5}([-])[0-9]{1,7}\1[0-9]{1,6}\1([0-9]|X)$");

                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _isbn = value;
            }
        }

        public Book(string name, string objectNotes, int pagesCount, DateTime publicatoinDate) :
            base(name, objectNotes, pagesCount, publicatoinDate)
        {

        }
    }
}
