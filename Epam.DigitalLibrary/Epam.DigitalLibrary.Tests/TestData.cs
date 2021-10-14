using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Tests
{
    public class TestData
    {
        #region TEST_CHARSETS
        public static readonly string charSetOfExistBook = "boo";
        public static readonly string charSetOfNonExistBook = "ops";
        public static readonly string emptyCharSet = "";
        public static readonly string nullRefCharSet = null;
        #endregion

        #region TEST_NOTES_NAMES
        public static readonly string existName = "book1";
        public static readonly string nonExistName = "book-nonExist";
        public static readonly string nameWithNullRef = null;
        #endregion

        #region AUTHORS_TEST_VALUES
        public static readonly Author existAuthor1 = new Author("Ivan", "Karasev");
        public static readonly Author existAuthor2 = new Author("Aleksei", "Ivanov");
        public static readonly Author nonExistAuthor = new Author("Karabas", "Barabas");
        public static readonly Author nullRefAuthor = null;
        #endregion

        #region BOOKS_TEST_VALUES
        public static readonly Book uniqueBook1 = new Book(
                name: "book1",
                authors: new List<Author> { new Author("Ivan", "Karasev"), new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "booker",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 1-56389-668-0"
                );

        public static readonly Book uniqueBook11 = new Book(
                name: "book1",
                authors: new List<Author> { new Author("Ivan", "Karasev"), new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "booker",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 3-56389-668-0"
                );

        public static readonly Book uniqueBook2 = new Book(
                name: "book2",
                authors: new List<Author> { new Author("Ivan", "Karasev") },
                publicationPlace: "Saratov",
                publisher: "booker",
                publicationDate: new DateTime(1920, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: null
                );

        public static readonly Book uniqueBook3 = new Book(
                name: "book3",
                authors: new List<Author> { new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "booker2",
                publicationDate: new DateTime(1930, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 2-56389-668-0"
                );

        public static readonly Book uniqueBook4 = new Book(
                name: "another-book",
                authors: new List<Author> { new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "booker3",
                publicationDate: new DateTime(1930, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 3-56389-668-0"
                );

        public static readonly Book bookWithSameISBN = new Book(
                name: "book11",
                authors: new List<Author> { new Author("Ivan", "Karasev") },
                publicationPlace: "Samara",
                publisher: "booker",
                publicationDate: new DateTime(1950, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 1-56389-668-0"
                );

        public static readonly Book sameBookWithoutISBN = new Book(
                name: "book1",
                authors: new List<Author> { new Author("Ivan", "Karasev"), new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "booker2",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: null
                );
        #endregion

        #region NEWSPAPER_TEST_VALUES
        public static readonly Newspaper uniqueNewspaper1 = new Newspaper(
                name: "newspaper1",
                publicationPlace: "Samara",
                publisher: "samaraNews",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 5,
                objectNotes: "hi",
                number: "123",
                releaseDate: new DateTime(1900, 01, 01),
                iSSN: "ISSN1284-5678"
                );

        public static readonly Newspaper uniqueNewspaper2 = new Newspaper(
                name: "newspaper2",
                publicationPlace: "Saratov",
                publisher: "saratovNews",
                publicationDate: new DateTime(1910, 01, 01),
                pagesCount: 5,
                objectNotes: "hi",
                number: "123",
                releaseDate: new DateTime(1910, 01, 01),
                iSSN: null
                );

        public static readonly Newspaper uniqueNewspaper3 = new Newspaper(
                name: "newspaper3",
                publicationPlace: "San-Diego",
                publisher: "sandiegoNews",
                publicationDate: new DateTime(1920, 01, 01),
                pagesCount: 5,
                objectNotes: "hi",
                number: "123",
                releaseDate: new DateTime(1920, 01, 01),
                iSSN: "ISSN2284-5678"
                );

        public static readonly Newspaper newspaperWithSameISSN = new Newspaper(
                name: "newspaper11",
                publicationPlace: "Samara",
                publisher: "samaraNews2",
                publicationDate: new DateTime(1920, 01, 01),
                pagesCount: 5,
                objectNotes: "hi",
                number: "123",
                releaseDate: new DateTime(1920, 01, 01),
                iSSN: "ISSN1284-5678"
                );

        public static readonly Newspaper sameNewspaperWithoutISSN = new Newspaper(
                name: "newspaper1",
                publicationPlace: "Samara",
                publisher: "samaraNews",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 5,
                objectNotes: "hi",
                number: "123",
                releaseDate: new DateTime(1900, 01, 01),
                iSSN: null
                );
        #endregion

        #region PATENTS_TEST_VALUES
        public static readonly Patent uniquePatent1 = new Patent(
                name: "patent1",
                authors: new List<Author> { new Author("Aleksei", "Ivanov") },
                country: "Russia",
                registrationNumber: "11123",
                applicationDate: new DateTime(1890, 01, 01),
                publicationDate: new DateTime(1891, 01, 01),
                pagesCount: 20,
                objectNotes: "da"
                );

        public static readonly Patent uniquePatent2 = new Patent(
                name: "patent2",
                authors: new List<Author> { new Author("Aleksei", "Ivanov"), new Author("Vasya", "Pupkin") },
                country: "China",
                registrationNumber: "21123",
                applicationDate: new DateTime(1990, 01, 01),
                publicationDate: new DateTime(1991, 01, 01),
                pagesCount: 20,
                objectNotes: "da"
                );

        public static readonly Patent uniquePatent3 = new Patent(
                name: "patent3",
                authors: new List<Author> { new Author("Vasya", "Pupkin") },
                country: "USA",
                registrationNumber: "31123",
                applicationDate: null,
                publicationDate: new DateTime(1691, 01, 01),
                pagesCount: 20,
                objectNotes: "da"
                );

        public static readonly Patent samePatent = new Patent(
                name: "patent11",
                authors: new List<Author> { new Author("Aleksei", "Ivanov") },
                country: "Russia",
                registrationNumber: "11123",
                applicationDate: new DateTime(1890, 01, 01),
                publicationDate: new DateTime(1891, 01, 01),
                pagesCount: 20,
                objectNotes: "da"
                );
        #endregion
    }
}
