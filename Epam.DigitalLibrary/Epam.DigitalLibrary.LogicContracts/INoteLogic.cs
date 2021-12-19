using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models.NewspaperModels;
using Epam.DigitalLibrary.Entities.Models.SearchModels;

namespace Epam.DigitalLibrary.LogicContracts
{
    public interface INoteLogic
    {
        public int AddNote(Note note, out Guid noteId);

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

        public Author GetAuthor(Guid id);

        public List<Author> GetAvailableAuthors();

        public int AddAuthor(Author author, out Guid id);

        public int UpdateAuthor(Guid id, Author updateAuthor);

        public IEnumerable<IGrouping<Guid?, Newspaper>> GroupNewspapersByReleaseId();

        public List<Newspaper> GetNewspaperReleases(Guid newspaperId);

        public bool SetReleaseToNewspaper(Guid newspaperId, Guid releaseId);

        public NewspaperDetailsViewModel GetNewspaperDetails(Guid id);

        public int UpdateNewspaperInfo(Guid id, NewspaperInputViewModel newspaperModel);

        public bool MarkForDeleteNewspaperRelease(Guid id);

        public int AddNewspaperRelease(NewspaperInputViewModel newspaperModel, out Guid id);

        public List<NewspaperDetailsViewModel> GetAllNewspaperReleases();

        public List<ShortNote> GetFilteredShortNotes(SearchRequest searchRequest, NoteTypes noteType);

        public List<Author> GetFilteredAuthors(string namePattern);

        public List<Newspaper> GetReleaseNewspapers(Guid newspaperReleaseId);
    }
}
