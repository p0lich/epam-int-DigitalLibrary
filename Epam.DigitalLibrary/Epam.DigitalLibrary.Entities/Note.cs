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

                if (value.Length == 0 || value.Length > 300)
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

        public bool IsDeleted { get; private set; }

        public abstract bool NoteUpdate(Book note);
        public abstract bool NoteUpdate(Newspaper note);
        public abstract bool NoteUpdate(Patent note);

        public Note(string name, string objectNotes, int pagesCount, DateTime publicationDate)
        {
            ID = Guid.NewGuid();
            Name = name;
            ObjectNotes = objectNotes;
            PagesCount = pagesCount;
            PublicationDate = publicationDate;
            IsDeleted = false;
        }

        public Note(Guid id, string name, string objectNotes, int pagesCount, DateTime publicationDate, bool isDeleted)
        {
            ID = id;
            Name = name;
            ObjectNotes = objectNotes;
            PagesCount = pagesCount;
            PublicationDate = publicationDate;
            IsDeleted = isDeleted;
        }

        public abstract bool IsUnique(List<Note> notes, Guid updateId);

        public abstract override string ToString();

        public abstract Dictionary<string, object> ToObjectDict();
    }
}
