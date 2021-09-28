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

        //public Note GetNote();

        public bool RemoveNote();

        //public bool UpdateNote(Note note);
    }
}
