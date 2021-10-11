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
    public class PatentTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentNullExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReturnArgumentNullExeptionCorrectly(
            string testName, List<Author> testAuthors, string testCountry,
            string testRegistrationNumber, DateTime? testApplicationDate,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes)
        {
            Patent incorrectPatent = new Patent(
                name: testName,
                authors: testAuthors,
                country: testCountry,
                registrationNumber: testRegistrationNumber,
                applicationDate: testApplicationDate,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentNullExeption()
        {
            return new[]
            {
                new object[]{ null, new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", null,
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    null, "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", null, new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" }
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentOutOfRangeExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReturnArgumentOutOfRangeExeptionCorrectly(
            string testName, List<Author> testAuthors, string testCountry,
            string testRegistrationNumber, DateTime? testApplicationDate,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes)
        {
            Patent incorrectPatent = new Patent(
                name: testName,
                authors: testAuthors,
                country: testCountry,
                registrationNumber: testRegistrationNumber,
                applicationDate: testApplicationDate,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentOutOfRangeExeption()
        {
            return new[]
            {
                new object[]{ "", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ new string('a', 350), new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(1473, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", null,
                    new DateTime(1473, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", null,
                    new DateTime(2030, 01, 01), 50, "hi" },

                
                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), -10, "hi" },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, new string('a', 2010) },
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentException))]
        public void ReturnArgumentExeptionCorrectly(
            string testName, List<Author> testAuthors, string testCountry,
            string testRegistrationNumber, DateTime? testApplicationDate,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes)
        {
            Patent incorrectPatent = new Patent(
                name: testName,
                authors: testAuthors,
                country: testCountry,
                registrationNumber: testRegistrationNumber,
                applicationDate: testApplicationDate,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes
                );
        }

        private static IEnumerable<object[]> GetTestDataForArgumentExeption()
        {
            return new[]
            {
                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "R" + new string('u', 200), "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Рussиa", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "RUSs", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "12356789123456", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123a56b8", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(1999, 01, 01), 50, "hi" },
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForProperPatentCreation), DynamicDataSourceType.Method)]
        public void ReturnObjectOfPatentCorrectly(
            string testName, List<Author> testAuthors, string testCountry,
            string testRegistrationNumber, DateTime? testApplicationDate,
            DateTime testPublicationDate, int testPagesCount, string testObjectNotes)
        {
            Patent patent = new Patent(
                name: testName,
                authors: testAuthors,
                country: testCountry,
                registrationNumber: testRegistrationNumber,
                applicationDate: testApplicationDate,
                publicationDate: testPublicationDate,
                pagesCount: testPagesCount,
                objectNotes: testObjectNotes
                );

            Assert.IsInstanceOfType(patent, typeof(Patent));
        }

        private static IEnumerable<object[]> GetTestDataForProperPatentCreation()
        {
            return new[]
            {
                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", null,
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, null },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Russia", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "RUS", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },


                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "Россия", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },

                new object[]{ "patent", new List<Author> { new Author("Ivanv", "Ivanov")},
                    "РУС", "123", new DateTime(2000, 01, 01),
                    new DateTime(2001, 01, 01), 50, "hi" },
            };
        }
    }
}
