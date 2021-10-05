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

        public Guid ID { get; }

        public string Name
        {
            get => _name;

            protected set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                if (value.Length > 300)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _name = value;
            }
        }

        public int PagesCount
        {
            get => _pagesCount;

            protected set
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
            get => _objectNotes;

            protected set
            {
                if (value is null)
                {
                    _objectNotes = null;
                    return;
                }

                if (value.Length > 2000)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _objectNotes = value;
            }
        }

        public abstract DateTime PublicationDate { get; protected set; }   

        public Note(string name, string objectNotes, int pagesCount, DateTime publicationDate)
        {
            ID = new Guid();
            Name = name;
            ObjectNotes = objectNotes;
            PagesCount = pagesCount;
            PublicationDate = publicationDate;
        }

        public abstract bool IsUnique(List<Note> notes);

        public abstract override string ToString();
    }
}
