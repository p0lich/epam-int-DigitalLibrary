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
        private IEnumerable<Note> _data;

        public bool AddNote(Note note)
        {
            throw new NotImplementedException();
        }

        public Note GetNote()
        {
            throw new NotImplementedException();
        }

        public bool RemoveNote(Note note)
        {
            throw new NotImplementedException();
        }

        public bool UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }
    }
}
