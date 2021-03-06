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
        private List<Author> _authors;

        public List<Author> Authors
        {
            get => _authors;

            private set
            {
                if ((value is null) || value.Count == 0)
                {
                    throw new ArgumentNullException();
                }

                _authors = value;
            }
        }

        public string PublicationPlace
        {
            get => _publicationPlace;

            private set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                Regex regex = new Regex(@"^(([A-Z]([a-z]*( [A-Z]?)?[a-z]+((-[A-Z])|(-[a-z]+-[A-Z]))?[a-z]*( [A-Z]?)?[a-z]+))|([А-ЯЁ]([а-яё]*( [А-ЯЁ]?)?[а-яё]+((-[А-ЯЁ])|(-[а-яё]+-[А-ЯЁ]))?[а-яё]*( [А-ЯЁ]?)?[а-яё]+)))$");

                if (!regex.IsMatch(value) || value.Length == 0 || value.Length > 200)
                {
                    throw new ArgumentException();
                }

                _publicationPlace = value;
            }
        }

        public string Publisher
        {
            get => _publisher;

            private set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                if (value.Length == 0 || value.Length > 300)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _publisher = value;
            }
        }

        public override DateTime PublicationDate
        {
            get => _publicationDate;

            protected set
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
            get => _isbn;

            private set
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
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            PublicationDate = publicationDate;
            ISBN = iSBN;
        }

        public Book(Guid id, string name, string objectNotes, int pagesCount, bool isDeleted,
            List<Author> authors, string publicationPlace, string publisher, DateTime publicationDate, string iSBN) :
            base(id, name, objectNotes, pagesCount, publicationDate, isDeleted)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            ISBN = iSBN;
        }

        public Book(NoteTypes noteType, string name, string objectNotes, int pagesCount,
            List<Author> authors, string publicationPlace, string publisher, DateTime publicationDate, string iSBN) :
            base(noteType, name, objectNotes, pagesCount, publicationDate)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            PublicationDate = publicationDate;
            ISBN = iSBN;
        }

        public Book(NoteTypes noteType, Guid id, string name, string objectNotes, int pagesCount, bool isDeleted,
            List<Author> authors, string publicationPlace, string publisher, DateTime publicationDate, string iSBN) :
            base(noteType, id, name, objectNotes, pagesCount, publicationDate, isDeleted)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            ISBN = iSBN;
        }

        public override bool IsUnique(List<Note> notes, Guid updateId)
        {
            IEnumerable<Book> books = notes.OfType<Book>();

            foreach (var book in books)
            {
                if (book.ID != updateId && !string.IsNullOrEmpty(book.ISBN))
                {
                    if (ISBN == book.ISBN)
                    {
                        return false;
                    }
                }

                if (book.ID != updateId && Name == book.Name &&
                    Authors.SequenceEqual(book.Authors) &&
                    PublicationDate == book.PublicationDate)
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder authors = new StringBuilder();

            for (int i = 0; i < Authors.Count; i++)
            {
                authors.Append(Authors[i].ToString() + "\n");
            }

            string res =  $"Name: {Name};\n" +
                $"Authors:\n{authors}" +
                $"Publication place: {PublicationPlace};\n" +
                $"Publisher: {Publisher};\n" +
                $"Publication date: {PublicationDate};\n" +
                $"Page count: {PagesCount};\n" +
                $"Book notes: {ObjectNotes ?? "N/A"};\n" +
                $"ISBN: {ISBN ?? "N/A"};\n";

            if (IsDeleted)
            {
                res += "*****MUST BE DELETED*****";
            }

            return res;
        }

        public override bool NoteUpdate(Book note)
        {
            Name = note.Name;
            ObjectNotes = note.ObjectNotes;
            PagesCount = note.PagesCount;
            PublicationPlace = note.PublicationPlace;
            Publisher = note.Publisher;
            PublicationDate = note.PublicationDate;
            ISBN = note.ISBN;

            return true;
        }

        public override bool NoteUpdate(Newspaper note)
        {
            throw new InvalidCastException();
        }

        public override bool NoteUpdate(Patent note)
        {
            throw new InvalidCastException();
        }

        public override Dictionary<string, object> ToObjectDict()
        {
            return new Dictionary<string, object>
            {
                { "ID", ID },
                { "Name", Name },
                { "Authors", Authors },
                { "PublicationPlace", PublicationPlace },
                { "Publisher", Publisher },
                { "PublicationDate", PublicationDate },
                { "PagesCount", PagesCount },
                { "ObjectNotes", ObjectNotes },
                { "ISBN", ISBN },
                { "IsDeleted", IsDeleted },
            };
        }
    }
}
