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

        public IEnumerable<IGrouping<string, Book>> SearchBooksByCharset(string charSet);

        public IEnumerable<IGrouping<int, Note>> GroupByYear();

        public int UpdateNote(Guid noteId, Note updatedNote);

        public bool RemoveNote(Note note);

        public List<Note> GetUnmarkedNotes();

        public bool MarkForDelete(Note note);

        public Note GetById(Guid id);

        public Book GetBookById(Guid id);

        public Newspaper GetNewspaperById(Guid id);

        public Patent GetPatentById(Guid id);

        public List<Author> GetAvailableAuthors();

        public IEnumerable<IGrouping<string, Newspaper>> GroupNewspapersByName();

        public List<Newspaper> GetNewspaperReleases(Guid newspaperId);
    }
}
