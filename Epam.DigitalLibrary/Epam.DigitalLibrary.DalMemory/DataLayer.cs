using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.DalContracts;

namespace Epam.DigitalLibrary.DalMemory
{
    public class DataLayer : IDataLayer
    {
        private readonly List<Note> _data;

        public DataLayer()
        {
            _data = new List<Note>();
        }

        public int AddNote(Note note)
        {
            try
            {
                if (note.IsUnique(GetAllNotes()))
                {
                    _data.Add(note);
                    return 0;
                }

                return -1;
            }

            catch (Exception)
            {
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

        public Note GetById(Guid id)
        {
            return _data.FirstOrDefault(n => n.ID == id);
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

        public int UpdateNote(Guid noteId, Note updatedNote)
        {
            throw new NotImplementedException();
        }
    }
}
