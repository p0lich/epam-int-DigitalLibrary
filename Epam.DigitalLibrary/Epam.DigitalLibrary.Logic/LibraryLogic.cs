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

        public Dictionary<int, Note> GroupByYear(int year)
        {
            return _dataLayer.GetAllNotes().ToDictionary(n => n.PublicationDate.Year);
        }

        public bool RemoveNote()
        {
            return _dataLayer.RemoveNote();
        }

        public List<Note> SearchBooksAndPatensByAuthor(Author author)
        {
            return _dataLayer.GetAllNotes().Where(n => n is Book || n is Patent)
                .Where(n => (n is Book && (n as Book).Authors.Contains(author))
                || ((n is Patent && (n as Patent).Authors.Contains(author)))).ToList();
        }

        public List<Book> SearchBooksByAuthor(Author author)
        {
            return _dataLayer.GetAllNotes().Where(n => n is Book)
                .Where(n => (n as Book).Authors.Contains(author))
                .Select(n => n as Book).ToList();
        }

        public Dictionary<string, Book> SearchBooksByCharset(string charSet)
        {
            Regex regex = new Regex($@"^{charSet}");

            return _dataLayer.GetAllNotes()
                .Where(n => n is Book && regex.IsMatch((n as Book).Name))
                .Select(n => n as Book).ToDictionary(n => n.Publisher);
        }

        public Note SearchByName(string name)
        {
            return _dataLayer.GetAllNotes().FirstOrDefault(n => n.Name == name);
        }

        public List<Patent> SearchPatentByInventor(Author author)
        {
            return _dataLayer.GetAllNotes().Where(n => n is Patent)
                .Where(n => (n as Patent).Authors.Contains(author))
                .Select(n => n as Patent).ToList();
        }

        public List<Note> GetCatalog()
        {
            return _dataLayer.GetAllNotes();
        }

        // Methods don't change data

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
