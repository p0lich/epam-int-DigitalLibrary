using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.DalMemory;
using System.Text.RegularExpressions;

namespace Epam.DigitalLibrary.Logic
{
    public class LibraryLogic : INoteLogic
    {
        private IDataLayer _dataLayer;

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
            return _dataLayer.GetAllNotes().GroupBy(n => n.PublicationDate.Year);
        }

        public bool RemoveNote()
        {
            return _dataLayer.RemoveNote();
        }

        public List<Note> SearchBooksAndPatensByAuthor(Author author)
        {
            IEnumerable<Note> books = _dataLayer.GetAllNotes().OfType<Book>()
                .Where(b => b.Authors.Contains(author));

            IEnumerable<Note> patents = _dataLayer.GetAllNotes().OfType<Patent>()
                .Where(p => p.Authors.Contains(author));

            return books.Concat(patents).ToList();
        }

        public List<Book> SearchBooksByAuthor(Author author)
        {
            return _dataLayer.GetAllNotes().OfType<Book>()
                .Where(p => p.Authors.Contains(author)).ToList();
        }

        public IEnumerable<IGrouping<string, Book>> SearchBooksByCharset(string charSet)
        {
            Regex regex = new Regex($@"^{charSet}");

            return _dataLayer.GetAllNotes().OfType<Book>()
                .Where(b => regex.IsMatch(b.Name)).GroupBy(b => b.Publisher);
        }

        public Note SearchByName(string name)
        {
            return _dataLayer.GetAllNotes().FirstOrDefault(n => n.Name == name);
        }

        public List<Patent> SearchPatentByInventor(Author author)
        {
            return _dataLayer.GetAllNotes().OfType<Patent>()
                .Where(p => p.Authors.Contains(author)).ToList();
        }

        public List<Note> GetCatalog()
        {
            return _dataLayer.GetAllNotes();
        }

        public List<Note> SortInOrder()
        {
            return _dataLayer.GetAllNotes().OrderBy(n => n.PublicationDate.Year).ToList();
        }

        public List<Note> SortInReverseOrder()
        {
            return _dataLayer.GetAllNotes().OrderByDescending(n => n.PublicationDate.Year).ToList();
        }
    }
}
