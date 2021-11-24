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

        public LibraryLogic()
        {
            _dataLayer = new DataLayer();
        }

        //public LibraryLogic(IDataLayer dataLayer)
        //{
        //    _dataLayer = dataLayer;
        //}

        public int AddNote(Note note)
        {
            try
            {
                return _dataLayer.AddNote(note);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public IEnumerable<IGrouping<int, Note>> GroupByYear()
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().GroupBy(n => n.PublicationDate.Year);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public bool RemoveNote()
        {
            try
            {
                return _dataLayer.RemoveNote();
            }
            
            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
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
                throw new BusinessLogicException();
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
                throw new BusinessLogicException();
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
                throw new BusinessLogicException();
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
                throw new BusinessLogicException();
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
                throw new BusinessLogicException();
            }
        }

        public List<Note> GetCatalog()
        {
            try
            {
                return _dataLayer.GetAllNotes();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public List<Note> SortInOrder()
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes().OrderBy(n => n.PublicationDate.Year).ToList();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
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
                throw new BusinessLogicException();
            }
        }

        public int UpdateNote(Guid noteId, Note updatedNote)
        {
            try
            {
                return _dataLayer.UpdateNote(noteId, updatedNote);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            } 
        }

        public bool RemoveNote(Note note)
        {
            try
            {
                return _dataLayer.RemoveNote(note);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public List<Note> GetUnmarkedNotes()
        {
            try
            {
                return _dataLayer.GetAllUnmarkedNotes();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public bool MarkForDelete(Note note)
        {
            try
            {
                return _dataLayer.MarkNote(note);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public Note GetById(Guid id)
        {
            try
            {
                return _dataLayer.GetById(id);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public Book GetBookById(Guid id)
        {
            try
            {
                return _dataLayer.GetBookById(id);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public Newspaper GetNewspaperId(Guid id)
        {
            try
            {
                return _dataLayer.GetNewspaperById(id);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public Patent GetPatentById(Guid id)
        {
            try
            {
                return _dataLayer.GetPatentById(id);

            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }

        public List<Author> GetAvailableAuthors()
        {
            try
            {
                return _dataLayer.GetAvailableAuthors();
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException();
            }
        }
    }
}
