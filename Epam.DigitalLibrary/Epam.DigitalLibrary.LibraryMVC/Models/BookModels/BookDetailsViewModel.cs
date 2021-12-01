using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class BookDetailsViewModel
    {
        private string _objectNotes, _iSBN;

        public Guid ID { get; set; }

        public string Name { get; set; }

        public List<Author> Authors { get; set; }

        public string PublicationPlace { get; set; }

        public string Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        public int PagesCount { get; set; }

        public string ObjectNotes
        {
            get
            {
                return _objectNotes ?? "N/A";
            }

            set
            {
                _objectNotes = value;
            }
        }

        public string ISBN {
            get
            {
                return _iSBN ?? "N/A";
            }

            set
            {
                _iSBN = value;
            }
        }

        public bool IsDeleted { get; set; }
    }
}
