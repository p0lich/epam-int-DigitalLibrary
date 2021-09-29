﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class Patent : Note
    {
        private DateTime _applicationDate, _publicationDate;
        private string _country, _registrationNumber;

        // At the moment inventors are authors
        public List<Author> Authors { get; set; }

        public string Country
        {
            get
            {
                return _country;
            }

            set
            {
                Regex regex = new Regex(@"^([A-Z][a-z]+|[A-Z]{2,})$"); // For EN language

                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _country = value;
            }
        }

        public string RegistrationNumber
        {
            get
            {
                return _registrationNumber;
            }

            set
            {
                if (!new Regex(@"^[0-9]{1,9}$").IsMatch(value))
                {
                    throw new ArgumentException();
                }

                _registrationNumber = value;
            }
        }

        public DateTime ApplicationDate
        {
            get
            {
                return _applicationDate;
            }

            set
            {
                if (value.Year < 1474)
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
                if (value.Year < 1474 || _applicationDate > value)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _publicationDate = value;
            }
        }

        public Patent(string name, string objectNotes, int pagesCount,
            List<Author> authors, string country, string registrationNumber, DateTime applicationDate, DateTime publicationDate) :
            base(name, objectNotes, pagesCount, publicationDate)
        {
            Authors = authors;
            Country = country;
            RegistrationNumber = registrationNumber;
            ApplicationDate = applicationDate;
            PublicationDate = publicationDate;
        }
    }
}
