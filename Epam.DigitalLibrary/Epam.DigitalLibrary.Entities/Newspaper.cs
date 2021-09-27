using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Newspaper : Note
    {
        private string _publisher;
        private DateTime _publicationDate, _releaseDate;
        private int _pagesCount;

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

        public int Number { get; set; }

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

        // ISSN

        public Newspaper(string name, string objectNotes, int pagesCount, DateTime publicatoinDate) : base(name, objectNotes, pagesCount, publicatoinDate)
        {
            
        }
    }
}
