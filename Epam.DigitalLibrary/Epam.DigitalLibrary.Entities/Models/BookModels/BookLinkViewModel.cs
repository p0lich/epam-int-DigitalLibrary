using System;
using System.Collections.Generic;
using System.Text;

namespace Epam.DigitalLibrary.Entities.Models.BookModels
{
    public class BookLinkViewModel
    {
        public Guid ID { get; private set; }
        public List<Author> Authors { get; set; }

        public string Name { get; set; }

        public DateTime PublicationDate { get; set; }

        public BookLinkViewModel(){ }

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
