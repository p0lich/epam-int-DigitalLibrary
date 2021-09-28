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
        public int AddNote(Note note);

        public bool RemoveNote();

        public List<Note> GetCatalog();

        public Note SearchByName(string name);

        public List<Note> SortInOrder();

        public List<Note> SortInReverseOrder();

        public List<Book> SearchBooksByAuthor(Author author);

        public List<Patent> SearchPatentByInventor(Author author);

        public List<Note> SearchBooksAndPatensByAuthor(Author author);

        public Dictionary<string, Book> SearchBooksByCharset(string charSet);

        public Dictionary<int, Note> GroupByYear(int year);
    }
}
