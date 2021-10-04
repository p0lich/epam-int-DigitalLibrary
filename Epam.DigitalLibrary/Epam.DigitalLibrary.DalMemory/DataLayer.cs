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
                if (IsUnique(note))
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

        public bool RemoveNote()
        {
            if (_data.Count == 0)
            {
                return false;
            }

            _data.RemoveAt(0);
            return true;
        }

        private bool IsUnique(Note note)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].IsDuplicate(note))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
