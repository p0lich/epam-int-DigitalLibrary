using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;

namespace Epam.DigitalLibrary.DalContracts
{
    public interface IDataLayer
    {
        public int AddNote(Note note);

        public List<Note> GetAllNotes();

        public Note GetById(Guid id);

        public bool RemoveNote();

        public bool RemoveNote(Note note);

        public bool MarkNote(Note note);

        public int UpdateNote(Guid noteId, Note updatedNote);

        public List<Note> GetAllUnmarkedNotes();
    }
}
