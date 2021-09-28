using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.DalMemory;

namespace Epam.DigitalLibrary.Logic
{
    public class LibraryLogic : INoteLogic
    {
        private IDataLayer _dataLayer;

        public LibraryLogic()
        {
            _dataLayer = new DataLayer();
        }

        public bool AddNote(Note note)
        {
            throw new NotImplementedException();
        }

        public List<Note> GroupByYear(int year)
        {
            throw new NotImplementedException();
        }

        public bool RemoveNote()
        {
            throw new NotImplementedException();
        }

        public List<Note> SearchBooksAndPatensByAuthors()
        {
            throw new NotImplementedException();
        }

        public List<Book> SearchBooksByAuthors()
        {
            throw new NotImplementedException();
        }

        public List<Book> SearchBooksByCharset(string charSet)
        {
            throw new NotImplementedException();
        }

        public Note SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Patent> SearchPatentByInventors()
        {
            throw new NotImplementedException();
        }

        public List<Note> GetCatalog()
        {
            throw new NotImplementedException();
        }

        public bool SortInOrder()
        {
            throw new NotImplementedException();
        }

        public bool SortInReverseOrder()
        {
            throw new NotImplementedException();
        }
    }
}
