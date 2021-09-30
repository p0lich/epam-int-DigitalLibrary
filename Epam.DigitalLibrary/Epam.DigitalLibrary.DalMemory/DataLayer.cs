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

        // 0 - note was added
        // -1 - same note already exist
        // -2 - can't add note
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
            Note targetNote;

            if (note is Book)
            {
                Book book = note as Book;

                if (!string.IsNullOrEmpty(book.ISBN))
                {
                    targetNote = _data.Where(n => n is Book).FirstOrDefault(n => (n as Book).ISBN == book.ISBN);

                    return targetNote is null;
                }

                targetNote = _data.Where(n => n is Book).FirstOrDefault(n => (n as Book).Name == book.Name &&
                (n as Book).Authors.SequenceEqual(book.Authors) &&
                (n as Book).PublicationDate == book.PublicationDate);

                return targetNote is null;
            }

            if (note is Newspaper)
            {
                Newspaper newspaper = note as Newspaper;

                if (!string.IsNullOrEmpty(newspaper.ISSN))
                {
                    targetNote = _data.Where(n => n is Newspaper).FirstOrDefault(n => (n as Newspaper).ISSN == newspaper.ISSN);

                    return targetNote is null;
                }

                targetNote = _data.Where(n => n is Newspaper).FirstOrDefault(n => (n as Newspaper).Name == newspaper.Name &&
                (n as Newspaper).Publisher == newspaper.Name &&
                (n as Newspaper).PublicationDate == newspaper.PublicationDate);

                return targetNote is null;
            }

            Patent patent = note as Patent;

            targetNote = _data.Where(n => n is Patent).FirstOrDefault(n => (n as Patent).RegistrationNumber == patent.RegistrationNumber &&
            (n as Patent).Country == patent.Country);

            return targetNote is null;
        }
    }
}
