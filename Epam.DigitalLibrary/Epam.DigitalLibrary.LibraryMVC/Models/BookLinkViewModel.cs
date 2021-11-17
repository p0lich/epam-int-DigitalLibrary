using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class BookLinkViewModel
    {
        public Guid ID { get; private set; }
        public List<Author> Authors { get; set; }

        public string Name { get; set; }

        public DateTime PublicationDate { get; set; }

        public BookLinkViewModel(Book book)
        {
            ID = book.ID;
            Authors = book.Authors;
            Name = book.Name;
            PublicationDate = book.PublicationDate;
        }

        public override string ToString()
        {
            StringBuilder authorsStringFormat = new StringBuilder();

            for (int i = 0; i < Authors.Count; i++)
            {
                authorsStringFormat.Append($"{Authors[i].FirstName[0]}. {Authors[i].LastName}");

                if (i < Authors.Count - 1)
                {
                    authorsStringFormat.Append(", ");
                }
            }

            return $"{authorsStringFormat} - {Name} ({PublicationDate})";
        }
    }
}
