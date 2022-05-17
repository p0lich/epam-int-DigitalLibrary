using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Patent : Note
    {
        private DateTime _publicationDate;
        private DateTime? _applicationDate;
        private string _country, _registrationNumber;
        private List<Author> _authors;

        public List<Author> Authors {
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

        public string Country
        {
            get => _country;

            private set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                Regex regex = new Regex(@"^(([A-Z][a-z]+|[A-Z]{2,})|([А-ЯЁ][а-яё]+|[А-ЯЁ]{2,}))$");

                if (!regex.IsMatch(value) || value.Length == 0 || value.Length > 200)
                {
                    throw new ArgumentException();
                }

                _country = value;
            }
        }

        public string RegistrationNumber
        {
            get => _registrationNumber;

            private set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                if (!new Regex(@"^[0-9]{1,9}$").IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _registrationNumber = value;
            }
        }

        public DateTime? ApplicationDate
        {
            get => _applicationDate;

            private set
            {
                if (value.HasValue)
                {
                    if (value.Value.Year < 1474)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }

                _applicationDate = value;
            }
        }

        public override DateTime PublicationDate
        {
            get => _publicationDate;

            protected set
            {
                if (_applicationDate.HasValue)
                {
                    if (value < _applicationDate.Value)
                    {
                        throw new ArgumentException();
                    }
                }

                if (value.Year < 1474 || value > DateTime.Now)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _publicationDate = value;
            }
        }

        public Patent(string name, string objectNotes, int pagesCount,
            List<Author> authors, string country, string registrationNumber, DateTime? applicationDate, DateTime publicationDate) :
            base(name, objectNotes, pagesCount, publicationDate)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            Country = country;
            RegistrationNumber = registrationNumber;
            ApplicationDate = applicationDate;
            PublicationDate = publicationDate;
        }

        public Patent(Guid id, string name, string objectNotes, int pagesCount, bool isDeleted,
            List<Author> authors, string country, string registrationNumber, DateTime? applicationDate, DateTime publicationDate) :
            base(id, name, objectNotes, pagesCount, publicationDate, isDeleted)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            Country = country;
            RegistrationNumber = registrationNumber;
            ApplicationDate = applicationDate;
        }

        public Patent(NoteTypes noteType, string name, string objectNotes, int pagesCount,
            List<Author> authors, string country, string registrationNumber, DateTime? applicationDate, DateTime publicationDate) :
            base(noteType, name, objectNotes, pagesCount, publicationDate)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            Country = country;
            RegistrationNumber = registrationNumber;
            ApplicationDate = applicationDate;
            PublicationDate = publicationDate;
        }

        public Patent(NoteTypes noteType, Guid id, string name, string objectNotes, int pagesCount, bool isDeleted,
            List<Author> authors, string country, string registrationNumber, DateTime? applicationDate, DateTime publicationDate) :
            base(noteType, id, name, objectNotes, pagesCount, publicationDate, isDeleted)
        {
            Authors = authors.OrderBy(a => a.FirstName).ToList();
            Country = country;
            RegistrationNumber = registrationNumber;
            ApplicationDate = applicationDate;
        }

        public override bool IsUnique(List<Note> notes, Guid updateId)
        {
            IEnumerable<Patent> patents = notes.OfType<Patent>();

            foreach (Patent patent in patents)
            {
                if (patent.ID != updateId && Country == patent.Country &&
                    RegistrationNumber == patent.RegistrationNumber)
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder inventors = new StringBuilder();

            for (int i = 0; i < Authors.Count; i++)
            {
                inventors.Append(Authors[i].ToString() + "\n");
            }

            string res = $"Name: {Name};\n" +
                $"Inventors:\n{inventors}" +
                $"Country: {Country};\n" +
                $"Registration number: {RegistrationNumber};\n" +
                $"Application date: {(ApplicationDate is null ? "N/A" : ApplicationDate.ToString())};\n" +
                $"Publication date: {PublicationDate};\n" +
                $"Page count: {PagesCount};\n" +
                $"Patent notes: {ObjectNotes ?? "N/A"};\n";

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
            throw new InvalidCastException();
        }

        public override bool NoteUpdate(Patent note)
        {
            Name = note.Name;
            ObjectNotes = note.ObjectNotes;
            PagesCount = note.PagesCount;
            Country = note.Country;
            RegistrationNumber = note.RegistrationNumber;
            ApplicationDate = note.ApplicationDate;
            PublicationDate = note.PublicationDate;

            return true;
        }

        public override Dictionary<string, object> ToObjectDict()
        {
            return new Dictionary<string, object>
            {
                { "ID", ID },
                { "Name", Name },
                { "Authors", Authors },
                { "Country", Country },
                { "RegistrationNumber", RegistrationNumber },
                { "ApplicationDate", ApplicationDate },
                { "PublicationDate", PublicationDate },
                { "PagesCount", PagesCount },
                { "ObjectNotes", ObjectNotes },
                { "IsDeleted", IsDeleted },
            };
        }
    }
}
