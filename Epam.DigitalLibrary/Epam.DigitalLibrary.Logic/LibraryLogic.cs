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
using Epam.DigitalLibrary.CustomExeptions;

namespace Epam.DigitalLibrary.Logic
{
    public class LibraryLogic : INoteLogic
    {
        private IDataLayer _dataLayer;

        public LibraryLogic(string login, SecureString password)
        {
            _dataLayer = new SqlDataAccessObject(new SqlCredential(login, password));
        }

        public LibraryLogic(string connectionString, SqlCredential credential)
        {
            _dataLayer = new SqlDataAccessObject(connectionString, credential);
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
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().GroupBy(n => n.PublicationDate.Year);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public bool RemoveNote()
        {
            return _dataLayer.RemoveNote();
        }

        public List<Note> SearchBooksAndPatensByAuthor(Author author)
        {
            try
            {
                IEnumerable<Note> books = _dataLayer.GetAllUnmarkedNotes().OfType<Book>()
                .Where(b => b.Authors.Contains(author));

                IEnumerable<Note> patents = _dataLayer.GetAllUnmarkedNotes().OfType<Patent>()
                    .Where(p => p.Authors.Contains(author));

                return books.Concat(patents).ToList();
            }
   
            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public List<Book> SearchBooksByAuthor(Author author)
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().OfType<Book>()
                .Where(p => p.Authors.Contains(author)).ToList();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public IEnumerable<IGrouping<string, Book>> SearchBooksByCharset(string charSet)
        {
            Regex regex = new Regex($@"^{charSet}");

            try
            {
                return _dataLayer.GetAllUnmarkedNotes().OfType<Book>()
                .Where(b => regex.IsMatch(b.Name)).GroupBy(b => b.Publisher);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public Note SearchByName(string name)
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().FirstOrDefault(n => n.Name == name);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public List<Patent> SearchPatentByInventor(Author author)
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().OfType<Patent>()
                .Where(p => p.Authors.Contains(author)).ToList();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public List<Note> GetCatalog()
        {
            return _dataLayer.GetAllNotes();
        }

        public List<Note> SortInOrder()
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().OrderBy(n => n.PublicationDate.Year).ToList();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public List<Note> SortInReverseOrder()
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().OrderByDescending(n => n.PublicationDate.Year).ToList();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
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

        public Note GetById(Guid id)
        {
            return _dataLayer.GetById(id);
        }

        public Book GetBookById(Guid id)
        {
            return _dataLayer.GetBookById(id);
        }

        public Newspaper GetNewspaperById(Guid id)
        {
            return _dataLayer.GetNewspaperById(id);
        }

        public Patent GetPatentById(Guid id)
        {
            return _dataLayer.GetPatentById(id);
        }

        public List<Author> GetAvailableAuthors()
        {
            return _dataLayer.GetAvailableAuthors();
        }

        public IEnumerable<IGrouping<string, Newspaper>> GroupNewspapersByName()
        {
            try
            {
                return _dataLayer.GetAllNotes().OfType<Newspaper>().GroupBy(n => n.Name);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public List<Newspaper> GetNewspaperReleases(Guid newspaperId)
        {
            try
            {
                var newspaperGroups = GroupNewspapersByName();

                Newspaper newspaper = _dataLayer.GetNewspaperById(newspaperId);

                return newspaperGroups
                    .FirstOrDefault(g => g.Key == newspaper.Name)
                    .ToList();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }
    }
}
