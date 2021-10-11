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
    public class AuthorTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentNullExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReturnArgumentNullExeptionCorrectly(string firstName, string lastName)
        {
            Author incorrectAuthor = new Author(firstName, lastName);
        }

        private static IEnumerable<object[]> GetTestDataForArgumentNullExeption()
        {
            return new[]
            {
                new object[]{ null, "Ivanov"},
                new object[]{ "Ivan", null},
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForArgumentExeption), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentException))]
        public void ReturnArgumentExeptionCorrectly(string firstName, string lastName)
        {
            Author incorrectAuthor = new Author(firstName, lastName);
        }

        private static IEnumerable<object[]> GetTestDataForArgumentExeption()
        {
            return new []
            {
                new object[]{ "", "Ivanov"},
                new object[]{ "Ivan", ""},
                new object[]{ "Ivaн", "Ivanov" },
                new object[]{ "Ivan", "Ivaнov" },
                new object[]{ "I" + new string('v', 50), "Ivanov" },
                new object[]{ "Ivan", "I" + new string('v', 200) },
                new object[]{ "ivan", "Ivanov" },
                new object[]{ "Ivan", "ivanov" },
                new object[]{ "-Ivan", "Ivanov" },
                new object[]{ "Ivan", "-Ivanov" },
                new object[]{ "Ivan", "van-Ivanov" },
                new object[]{ "Ivan", "'Ivanov" },
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForProperAuthorCreation), DynamicDataSourceType.Method)]
        public void ReturnObjectOfTypeAuthorCorrectly(string firstName, string lastName)
        {
            Author author = new Author(firstName, lastName);

            Assert.IsInstanceOfType(author, typeof(Author));
        }

        private static IEnumerable<object[]> GetTestDataForProperAuthorCreation()
        {
            return new[]
            {
                new object[] {"Ivan", "Ivanov"},
                new object[] { "Ivan-Ivanovich", "Ivanov" },
                new object[] { "Ivan", "van Ivanov" },
                new object[] { "Ivan", "van Ivanov-Greatest" },
                new object[] { "Ivan", "I'vanov" },

                new object[] {"Иван", "Иванов"},
                new object[] { "Иван-Иванович", "Иванов" },
                new object[] { "Иван", "фон Иванов" },
                new object[] { "Иван", "фон Иванов-Величайший" },
                new object[] { "Иван", "И'ванов" }
            };
        }
    }
}
