using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;

namespace Epam.DigitalLibrary.DalContracts
{
    public interface INoteDAO
    {
        public bool InsertNote(Guid noteId, Note note);

        public bool DeleteNote(Guid noteId);

        public bool MarkForDelete(Guid noteId);

        public bool UpdateNote(Guid noteId, Note updatedNote);

        public List<Note> GetNotes();

        public Guid GetMainNoteId(Guid noteId);

        public List<Note> GetUnmarkedNotes();

        public Note GetById(Guid id);
    }
}
