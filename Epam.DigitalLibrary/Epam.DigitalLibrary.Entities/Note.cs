using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public abstract class Note
    {
        private string _name, _objectNotes;
        private int _pagesCount;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value.Length > 300)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _name = value;
            }
        }

        public int PagesCount
        {
            get
            {
                return _pagesCount;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _pagesCount = value;
            }
        }

        public string ObjectNotes
        {
            get
            {
                return _objectNotes;
            }

            set
            {
                if (value.Length > 2000)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _objectNotes = value;
            }
        }

        public abstract DateTime PublicationDate { get; set; }

        public Note(string name, string objectNotes, int pagesCount, DateTime publicationDate)
        {
            Name = name;
            ObjectNotes = objectNotes;
            PagesCount = pagesCount;
            PublicationDate = publicationDate;
        }
    }
}
