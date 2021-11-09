using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.SqlDal;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.DalMemory;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Security;

namespace Epam.DigitalLibrary.Logic
{
    public class LibraryLogic : INoteLogic
    {
        private IDataLayer _dataLayer;

        public LibraryLogic(string login, SecureString password)
        {
            _dataLayer = new SqlDataAccessObject(new SqlCredential(login, password));
        }

        public LibraryLogic()
        {
            _dataLayer = new DataLayer();
        }

        public int AddNote(Note note)
        {
            return _dataLayer.AddNote(note);
        }

        public IEnumerable<IGrouping<int, Note>> GroupByYear()
        {
            return _dataLayer.GetAllUnmarkedNotes().GroupBy(n => n.PublicationDate.Year);
        }

        public bool RemoveNote()
        {
            return _dataLayer.RemoveNote();
        }

        public List<Note> SearchBooksAndPatensByAuthor(Author author)
        {
            IEnumerable<Note> books = _dataLayer.GetAllUnmarkedNotes().OfType<Book>()
                .Where(b => b.Authors.Contains(author));

            IEnumerable<Note> patents = _dataLayer.GetAllUnmarkedNotes().OfType<Patent>()
                .Where(p => p.Authors.Contains(author));

            return books.Concat(patents).ToList();
        }

        public List<Book> SearchBooksByAuthor(Author author)
        {
            return _dataLayer.GetAllUnmarkedNotes().OfType<Book>()
                .Where(p => p.Authors.Contains(author)).ToList();
        }

        public IEnumerable<IGrouping<string, Book>> SearchBooksByCharset(string charSet)
        {
            Regex regex = new Regex($@"^{charSet}");

            return _dataLayer.GetAllUnmarkedNotes().OfType<Book>()
                .Where(b => regex.IsMatch(b.Name)).GroupBy(b => b.Publisher);
        }

        public Note SearchByName(string name)
        {
            return _dataLayer.GetAllUnmarkedNotes().FirstOrDefault(n => n.Name == name);
        }

        public List<Patent> SearchPatentByInventor(Author author)
        {
            return _dataLayer.GetAllUnmarkedNotes().OfType<Patent>()
                .Where(p => p.Authors.Contains(author)).ToList();
        }

        public List<Note> GetCatalog()
        {
            return _dataLayer.GetAllNotes();
        }

        public List<Note> SortInOrder()
        {
            return _dataLayer.GetAllUnmarkedNotes().OrderBy(n => n.PublicationDate.Year).ToList();
        }

        public List<Note> SortInReverseOrder()
        {
            return _dataLayer.GetAllUnmarkedNotes().OrderByDescending(n => n.PublicationDate.Year).ToList();
        }

        public int UpdateNote(Guid noteId, Note updatedNote)
        {
            return _dataLayer.UpdateNote(noteId, updatedNote);
        }

        public bool RemoveNote(Note note)
        {
            return _dataLayer.RemoveNote(note);
        }

        public List<Note> GetUnmarkedNotes()
        {
            return _dataLayer.GetAllUnmarkedNotes();
        }

        public bool MarkForDelete(Note note)
        {
            return _dataLayer.MarkNote(note);
        }
    }
}
