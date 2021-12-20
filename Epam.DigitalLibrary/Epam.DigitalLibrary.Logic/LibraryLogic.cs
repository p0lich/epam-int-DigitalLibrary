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
using Epam.DigitalLibrary.Entities.Models.NewspaperModels;
using Epam.DigitalLibrary.Entities.Models.SearchModels;

namespace Epam.DigitalLibrary.Logic
{
    public class LibraryLogic : INoteLogic
    {
        private IDataLayer _dataLayer;
        private INewspaperReleaseDAO _newspaperReleaseDAO;

        public LibraryLogic(string connString)
        {
            _dataLayer = new SqlDataAccessObject(connString);
            _newspaperReleaseDAO = new NewspaperReleaseDAO(connString);
        }

        public LibraryLogic()
        {
            _dataLayer = new DataLayer();
        }

        public int AddNote(Note note, out Guid noteId)
        {
            return _dataLayer.AddNote(note, out noteId);
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
            if (note is Newspaper)
            {
                Newspaper newspaper = note as Newspaper;

                if (newspaper.ReleaseId.HasValue && IsAllNewspaperReleasesMarked(note.ID))
                {
                    _newspaperReleaseDAO.MarkForDeleteNewspaperRelease(newspaper.ReleaseId.Value);
                }
            }

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

        public SearchResponse GetFilteredShortNotes(SearchRequest searchRequest, NoteTypes noteType)
        {
            return _dataLayer.GetFilteredNotes(searchRequest, noteType);
        }

        #region AUTHOR_LOGIC
        public List<Author> GetAvailableAuthors()
        {
            return _dataLayer.GetAvailableAuthors();
        }

        public Author GetAuthor(Guid id)
        {
            return _dataLayer.GetAuthor(id);
        }

        public int AddAuthor(Author author, out Guid id)
        {
            return _dataLayer.AddAuthor(author, out id);
        }

        public int UpdateAuthor(Guid id, Author updateAuthor)
        {
            return _dataLayer.UpdateAuthor(id, updateAuthor);
        }

        public List<Author> GetFilteredAuthors(string namePattern)
        {
            return _dataLayer.GetFilteredAuthors(namePattern);
        }
        #endregion

        #region NEWSPAPER_UNIQUE_LOGIC
        public IEnumerable<IGrouping<Guid?, Newspaper>> GroupNewspapersByReleaseId()
        {
            try
            {
                return _dataLayer.GetAllNotes().OfType<Newspaper>().GroupBy(n => n.ReleaseId);
            }

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
        }

        public List<NewspaperDetailsViewModel> GetAllNewspaperReleases()
        {
            return _newspaperReleaseDAO.GetAllNewspapers();
        }

        public List<Newspaper> GetNewspaperReleases(Guid newspaperId)
        {
            return _newspaperReleaseDAO.GetAllNewspaperReleases(newspaperId);
        }

        public bool SetReleaseToNewspaper(Guid newspaperId, Guid releaseId)
        {
            return _newspaperReleaseDAO.SetRelease(newspaperId, releaseId);
        }

        public NewspaperDetailsViewModel GetNewspaperDetails(Guid id)
        {
            return _newspaperReleaseDAO.GetNewspaperRelease(id);
        }

        public int UpdateNewspaperInfo(Guid id, NewspaperInputViewModel newspaperModel)
        {
            return _newspaperReleaseDAO.UpdateNewspaperRelease(id, newspaperModel);
        }

        public bool MarkForDeleteNewspaperRelease(Guid id)
        {
            try
            {
                List<Newspaper> releaseNewspapers = GetReleaseNewspapers(id);

                foreach (var newspaper in releaseNewspapers)
                {
                    _dataLayer.MarkNote(newspaper);
                }

                return _newspaperReleaseDAO.MarkForDeleteNewspaperRelease(id);
            }

            catch (Exception e)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
            
        }

        public int AddNewspaperRelease(NewspaperInputViewModel newspaperModel, out Guid id)
        {
            return _newspaperReleaseDAO.AddNewspaperRelease(newspaperModel, out id);
        }

        public List<Newspaper> GetReleaseNewspapers(Guid newspaperReleaseId)
        {
            return _newspaperReleaseDAO.GetReleaseNewspapers(newspaperReleaseId);
        }

        private bool IsAllNewspaperReleasesMarked(Guid newspaperId)
        {
            try
            {
                List<Newspaper> releaseNewspapers = _newspaperReleaseDAO.GetAllNewspaperReleases(newspaperId);

                foreach (var newspaper in releaseNewspapers)
                {
                    if (!newspaper.IsDeleted)
                    {
                        return false;
                    }
                }

                return true;
            }

            catch (Exception e)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }  
        }
        #endregion
    }
}
