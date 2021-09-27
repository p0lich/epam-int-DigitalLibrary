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
        public bool AddNote(Note note);

        public Note GetNote();

        public bool RemoveNote(Note note);

        public bool UpdateNote(Note note);
    }
}
