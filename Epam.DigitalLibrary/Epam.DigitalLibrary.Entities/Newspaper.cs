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

        public string PublicationPlace
        {
            get => _publicationPlace;

            private set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                Regex regex = new Regex(@"^(([A-Z]([a-z]*( [A-Z]?)?[a-z]+(-[A-Z])?[a-z]*( [A-Z]?)?[a-z]+))|([А-ЯЁ]([а-яё]*( [А-ЯЁ]?)?[а-яё]+(-[А-ЯЁ])?[а-яё]*( [А-ЯЁ]?)?[а-яё]+)))$");

                if (!regex.IsMatch(value) || value.Length > 200)
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

        public string Number { get; private set; }

        public DateTime ReleaseDate
        {
            get => _releaseDate;

            private set
            {
                if (_publicationDate.Year < value.Year)
                {
                    throw new ArgumentException();
                }

                _releaseDate = value;
            }
        }

        public string ISSN
        {
            get => _issn;

            private set
            {
                if (value is null)
                {
                    _issn = null;
                    return;
                }

                Regex regex = new Regex(@"^ISSN[0-9]{4}-[0-9]{4}$");

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

        public Newspaper(Guid id, string name, string objectNotes, int pagesCount, DateTime publicationDate, bool isDeleted,
            string publicationPlace, string publisher, string number, DateTime releaseDate, string iSSN) :
            base(id, name, objectNotes, pagesCount, publicationDate, isDeleted)
        {
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            Number = number;
            ReleaseDate = releaseDate;
            ISSN = iSSN;
        }

        public override bool IsUnique(List<Note> notes, Guid updateId)
        {
            IEnumerable<Newspaper> newspapers = notes.OfType<Newspaper>();

            foreach (var newspaper in newspapers)
            {
                if (!string.IsNullOrEmpty(newspaper.ISSN))
                {
                    if (newspaper.ID != updateId && ISSN == newspaper.ISSN && Name != newspaper.Name)
                    {
                        return false;
                    }
                }

                if (newspaper.ID != updateId && Name == newspaper.Name &&
                    Publisher == newspaper.Publisher &&
                    ReleaseDate == newspaper.ReleaseDate)
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            string res =  $"Name: {Name};\n" +
                $"Publication place: {PublicationPlace};\n" +
                $"Publisher: {Publisher};\n" +
                $"Publication date: {PublicationDate};\n" +
                $"Page count: {PagesCount};\n" +
                $"Newspaper notes: {ObjectNotes ?? "N/A"};\n" +
                $"Number: {Number ?? "N/A"};\n" +
                $"Release date: {ReleaseDate};\n" +
                $"ISBN: {ISSN ?? "N/A"};\n";

            if (IsDeleted)
            {
                res += "*****MUST BE DELETED*****";
            }

            return res;
        }

        public override bool NoteUpdate(Book note)
        {
            throw new InvalidCastException();
        }

        public override bool NoteUpdate(Newspaper note)
        {
            Name = note.Name;
            ObjectNotes = note.ObjectNotes;
            PagesCount = note.PagesCount;
            PublicationPlace = note.PublicationPlace;
            Publisher = note.Publisher;
            PublicationDate = note.PublicationDate;
            Number = note.Number;
            ReleaseDate = note.ReleaseDate;
            ISSN = note.ISSN;

            return true;
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
                { "PublicationPlace", PublicationPlace },
                { "Publisher", Publisher },
                { "PublicationDate", PublicationDate },
                { "PagesCount", PagesCount },
                { "ObjectNotes", ObjectNotes },
                { "Number", Number },
                { "ReleaseDate", ReleaseDate },
                { "ISSN", ISSN },
                { "IsDeleted", IsDeleted },
            };
        }
    }
}
