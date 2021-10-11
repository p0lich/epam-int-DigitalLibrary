using Epam.DigitalLibrary.ConsolePL;
using Epam.DigitalLibrary.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Tests.PresentationLayerTests
{
    [TestClass]
    public class ProgramTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetDataForCorrectAuthorCreation), DynamicDataSourceType.Method)]
        public void InputAuthor_OneAuthor_ReturnsAthhorWithSameProperties(string[] testData)
        {
            InputParametersForConsole(testData);

            string firstName = testData[0];
            string lastName = testData[1];

            Author inputedAuthor = Program.InputAuthor();

            bool isAuthorWasGeneratedCorrectly =
                inputedAuthor.FirstName == firstName && inputedAuthor.LastName == lastName;

            Assert.IsTrue(isAuthorWasGeneratedCorrectly);
        }

        private static IEnumerable<object[]> GetDataForCorrectAuthorCreation()
        {
            return new[]
            {
                new object[] { new string[] {"Ivan", "Ivanov" } },
                new object[] { new string[] {"Иван", "Иванов" } }
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDataForIncorrectAuthorCreation), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(Exception))]
        public void InputAuthor_Exeption_ThrowExeption(string[] testData)
        {
            InputParametersForConsole(testData);

            Author inputedAuthor = Program.InputAuthor();
        }

        private static IEnumerable<object[]> GetDataForIncorrectAuthorCreation()
        {
            return new[]
            {
                new object[] { new string[] { "", "Ivanov" } },
                new object[] { new string[] { "Ivan", "" } },

                new object[] { new string[] { "\n\r", "Ivanov" } },
                new object[] { new string[] { "Ivan", "\n\r" } },
            };
        }



        private static void InputParametersForConsole(string[] stringTestData)
        {
            var inputData = string.Join(Environment.NewLine, stringTestData);

            Console.SetIn(new System.IO.StringReader(inputData));
        }
    }
}
