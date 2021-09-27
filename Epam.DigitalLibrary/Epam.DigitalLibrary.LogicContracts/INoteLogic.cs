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

        public List<Book> SearchBooksByAuthors();

        public List<Patent> SearchPatentByInventors();

        public List<Note> SearchBooksAndPatensByAuthors();

        public List<Book> SearchBooksByCharset(string charSet);

        public List<Note> GroupByYear(int year);
    }
}
