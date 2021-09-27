using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;

namespace Epam.DigitalLibrary.LogicContracts
{
    public interface INoteLogic
    {
        public bool AddNote(Note note);

        public bool RemoveNote(Note note);

        public bool ShowCatalog();

        public Note SearchByName(string name);

        public bool SortInOrder();

        public bool SortInReverseOrder();

        public IEnumerable<Book> SearchBooksByAuthors();

        public IEnumerable<Patent> SearchPatentByInventors();

        public IEnumerable<Note> SearchBooksAndPatensByAuthors();

        public IEnumerable<Book> SearchBooksByCharset(string charSet);

        public IEnumerable<Note> GroupByYear(int year);
    }
}
