using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.Entities.Models.SearchModels;

namespace Epam.DigitalLibrary.DalMemory
{
    public class DataLayer : IDataLayer
    {
        private readonly List<Note> _data;

        public DataLayer()
        {
            _data = new List<Note>();
        }

        public int AddAuthor(Author author, out Guid id)
        {
            throw new NotImplementedException();
        }

        public int AddNote(Note note, out Guid noteId)
        {
            try
            {
                if (note.IsUnique(GetAllNotes(), note.ID))
                {
                    _data.Add(note);
                    noteId = note.ID;
                    return 0;
                }

                noteId = new Guid();
                return -1;
            }

            catch (Exception)
            {
                noteId = new Guid();
                return -2;
            }
        }

        public List<Note> GetAllNotes()
        {
            return _data.Select(n => n).ToList();
        }

        public List<Note> GetAllUnmarkedNotes()
        {
            throw new NotImplementedException();
        }

        public Author GetAuthor(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Author> GetAvailableAuthors()
        {
            throw new NotImplementedException();
        }

        public Book GetBookById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Note GetById(Guid id)
        {
            return _data.FirstOrDefault(n => n.ID == id);
        }

        public List<Author> GetFilteredAuthors(string namepattern)
        {
            throw new NotImplementedException();
        }

        public SearchResponse GetFilteredNotes(SearchRequest request, NoteTypes noteType)
        {
            throw new NotImplementedException();
        }

        public Newspaper GetNewspaperById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Patent GetPatentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool MarkNote(Note note)
        {
            throw new NotImplementedException();
        }

        public bool RemoveNote()
        {
            if (_data.Count == 0)
            {
                return false;
            }

            _data.RemoveAt(0);
            return true;
        }

        public bool RemoveNote(Note note)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAuthor(Guid id, Author updatedAuthor)
        {
            throw new NotImplementedException();
        }

        public int UpdateNote(Guid noteId, Note updatedNote)
        {
            throw new NotImplementedException();
        }

        int IDataLayer.UpdateAuthor(Guid id, Author updatedAuthor)
        {
            throw new NotImplementedException();
        }
    }
}
