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
    public class NewspaperTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentNullExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReturnArgumentNullExeptionCorrectly(
            string testName, string testPublicationPlace, string testPublisher,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes,
            string testNumber, DateTime testReleaseDate, string testISSN)
        {
            Newspaper incorrectNewspaper = new Newspaper(
                name: testName,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                number: testNumber,
                releaseDate: testReleaseDate,
                iSSN: testISSN
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentNullExeption()
        {
            return new[]
            {
                new object[] {null, "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", null, "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Saratov", null,
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentOutOfRangeExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReturnArgumentOutOfRangeExeptionCorrectly(
            string testName, string testPublicationPlace, string testPublisher,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes,
            string testNumber, DateTime testReleaseDate, string testISSN)
        {
            Newspaper incorrectNewspaper = new Newspaper(
                name: testName,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                number: testNumber,
                releaseDate: testReleaseDate,
                iSSN: testISSN
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentOutOfRangeExeption()
        {
            return new[]
            {
                new object[] {"", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {new string('a', 350), "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Saratov", "",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Saratov", new string('a', 350),
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(1390, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2030, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), -10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, new string('a', 2010), "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentException))]
        public void ReturnArgumentExeptionCorrectly(
            string testName, string testPublicationPlace, string testPublisher,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes,
            string testNumber, DateTime testReleaseDate, string testISSN)
        {
            Newspaper incorrectNewspaper = new Newspaper(
                name: testName,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                number: testNumber,
                releaseDate: testReleaseDate,
                iSSN: testISSN
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentExeption()
        {
            return new[]
            {
                new object[] {"newspaper", "", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "S" + new string('a', 200), "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Saрaтoв", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "-Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Rostov-na-Donu", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Sar  atov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2001, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN12345678"},

                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN12340-5678"},

                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-56780"},
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForProperNewspaperCreation), DynamicDataSourceType.Method)]
        public void ReturnObjectOfNewspaperCorrectly(
            string testName, string testPublicationPlace, string testPublisher,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes,
            string testNumber, DateTime testReleaseDate, string testISSN)
        {
            Newspaper newspaper = new Newspaper(
                name: testName,
                publicationPlace: testPublicationPlace,
                publisher: testPublisher,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes,
                number: testNumber,
                releaseDate: testReleaseDate,
                iSSN: testISSN
                );

            Assert.IsInstanceOfType(newspaper, typeof(Newspaper));
        }

        private static IEnumerable<object[]> GetTestDataForProperNewspaperCreation()
        {
            return new[]
            {
                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, null, "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", null,
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Saratov", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), null},


                new object[] {"newspaper", "Saratov-City", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "New york", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Saratov junior", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},


                new object[] {"newspaper", "Саратов", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Саратов-Сити", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Нью Йорк", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},

                new object[] {"newspaper", "Саратов младший", "saratovNews",
                    new DateTime(2000, 01, 01), 10, "hi", "123",
                    new DateTime(2000, 01, 01), "ISSN1234-5678"},
            };
        }
    }
}
