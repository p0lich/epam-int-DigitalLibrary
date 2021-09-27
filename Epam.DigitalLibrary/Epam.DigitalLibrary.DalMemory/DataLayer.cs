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
        private List<Note> _data;

        // 0 - note was added
        // -1 - same note already exist
        // -2 - can't add note
        public int AddNote(Note note)
        {
            try
            {
                _data.Add(note);

                return 0;
            }
            catch (Exception)
            {


                throw;
            }
        }

        public List<Note> GetAllNotes()
        {
            return _data;
        }

        public bool RemoveNote(Note note)
        {
            throw new NotImplementedException();
        }
    }
}
