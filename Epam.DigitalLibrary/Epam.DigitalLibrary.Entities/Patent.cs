using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Patent : Note
    {
        private DateTime _applicationDate, _publicationDate;
        private string _country;

        // Inventors

        // Country

        public string RegistrationNumber
        {
            get; set;
        }

        public DateTime ApplicationDate
        {
            get
            {
                return _applicationDate;
            }

            set
            {
                if (_applicationDate.Year < 1474)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _applicationDate = value;
            }
        }

        public override DateTime PublicationDate
        {
            get
            {
                return _publicationDate;
            }

            set
            {
                if (_publicationDate.Year < 1474 || _applicationDate > value)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _publicationDate = value;
            }
        }

        public Patent(string name, string objectNotes, int pagesCount, DateTime publicatoinDate) : base(name, objectNotes, pagesCount, publicatoinDate)
        {

        }
    }
}
