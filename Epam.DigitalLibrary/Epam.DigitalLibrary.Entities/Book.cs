using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Book : Note
    {
        private string _publisher, _isbn;
        private DateTime _publicationDate;

        // Authors

        // Publication place

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

        // ISBN

        public Book(string name, string objectNotes, int pagesCount, DateTime publicatoinDate) : base(name, objectNotes, pagesCount, publicatoinDate)
        {

        }
    }
}
