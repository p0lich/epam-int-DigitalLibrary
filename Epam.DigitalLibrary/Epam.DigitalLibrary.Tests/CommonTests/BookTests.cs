using Epam.DigitalLibrary.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Tests.CommonTests
{
    [TestClass]
    public class BookTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentNullExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReturnArgumentNullExeptionCorrectly(
            string testName, List<Author> testAuthors, string testPublicationPlace,
            string testPublisher, DateTime testPublicationDate, int testPagesCount,
            string testObjectNotes, string testISBN)
        {
            Book incorrectBook = new Book(
                name: testName,
                authors: testAuthors,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                iSBN: testISBN
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentNullExeption()
        {
            return new[]
            {
                new object[]{ null, new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", null,
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    null, "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", null, new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentOutOfRangeExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReturnArgumentOutOfRangeExeptionCorrectly(
            string testName, List<Author> testAuthors, string testPublicationPlace,
            string testPublisher, DateTime testPublicationDate, int testPagesCount,
            string testObjectNotes, string testISBN)
        {
            Book incorrectBook = new Book(
                name: testName,
                authors: testAuthors,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                iSBN: testISBN
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentOutOfRangeExeption()
        {
            return new[]
            {
                new object[]{ "", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ new string('a', 350), new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", new string('a', 350), new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(1390, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2030, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), -1,
                    "hi", "ISBN 1-56389-668-0" },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    new string('a', 2010), "ISBN 1-56389-668-0" },
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentException))]
        public void ReturnArgumentExeptionCorrectly(
            string testName, List<Author> testAuthors, string testPublicationPlace,
            string testPublisher, DateTime testPublicationDate, int testPagesCount,
            string testObjectNotes, string testISBN)
        {
            Book incorrectBook = new Book(
                name: testName,
                authors: testAuthors,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                iSBN: testISBN
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentExeption()
        {
            return new[]
            {
                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "S" + new string('a', 200), "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saрaтoв", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "-Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Sara--tov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Sara  tov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-66132131321238-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBB 1-56389-668-0" },
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForProperBookCreation), DynamicDataSourceType.Method)]
        public void ReturnObjectOfBookCorrectly(
            string testName, List<Author> testAuthors, string testPublicationPlace,
            string testPublisher, DateTime testPublicationDate, int testPagesCount,
            string testObjectNotes, string testISBN)
        {
            Book book = new Book(
                name: testName,
                authors: testAuthors,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                iSBN: testISBN
                );

            Assert.IsInstanceOfType(book, typeof(Book));
        }

        private static IEnumerable<object[]> GetTestDataForProperBookCreation()
        {
            return new[]
            {
                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    null, "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", null },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "San-Diego", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Rostov-na-Donu", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "New York", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Saratov junior", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },


                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Саратов", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Сан-Диего", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Ростов-на-Дону", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Нью Йорк", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },

                new object[]{ "book", new List<Author> { new Author("Ivan", "Ivanov") },
                    "Саратов младший", "booker", new DateTime(2000, 01, 01), 50,
                    "hi", "ISBN 1-56389-668-0" },
            };
        }
    }
}
