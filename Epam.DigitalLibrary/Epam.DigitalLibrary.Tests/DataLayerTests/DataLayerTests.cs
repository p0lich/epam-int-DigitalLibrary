using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.DalMemory;
using Epam.DigitalLibrary.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Tests.DataLayerTests
{
    [TestClass]
    public class DataLayerTests
    {
        private IDataLayer dao;

        [TestInitialize]
        public void InitializeDAO()
        {
            dao = new DataLayer();
        }

        #region AddNote_TESTS
        [TestMethod]
        public void AddNote_ErrorCode_ReturnsCodeOfErrorForIncorrectElementCorrectly()
        {
            int codeOfAddNoteMethod = dao.AddNote(null, out Guid noteId);

            Assert.AreEqual(-2, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ZeroReturn_ReturnsZeroCodeAfterAddingBookCorrectly()
        {
            int codeOfAddNoteMethod = dao.AddNote(TestData.uniqueBook1, out Guid noteId);

            Assert.AreEqual(0, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ZeroReturn_ReturnsZeroCodeAfterAddingNewspaperCorrectly()
        {
            int codeOfAddNoteMethod = dao.AddNote(TestData.uniqueNewspaper1, out Guid noteId);

            Assert.AreEqual(0, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ZeroReturn_ReturnsZeroCodeAfterAddingPatentCorrectly()
        {
            int codeOfAddNoteMethod = dao.AddNote(TestData.uniquePatent1, out Guid noteId);

            Assert.AreEqual(0, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ErrorCode_ReturnsCodeForNonUniqueISBNBookCorrectly()
        {
            dao.AddNote(TestData.uniqueBook1, out Guid noteId);

            int codeOfAddNoteMethod = dao.AddNote(TestData.bookWithSameISBN, out Guid testNoteId);

            Assert.AreEqual(-1, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ErrorCode_ReturnsCodeForNonUniquePropertiesBookCorrectly()
        {
            dao.AddNote(TestData.uniqueBook1, out Guid noteId);

            int codeOfAddNoteMethod = dao.AddNote(TestData.sameBookWithoutISBN, out Guid testNoteId);

            Assert.AreEqual(-1, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ErrorCode_ReturnsCodeOfErrorForNonUniqueISSNNewspaperCorrectly()
        {
            dao.AddNote(TestData.uniqueNewspaper1, out Guid noteId);

            int codeOfAddNoteMethod = dao.AddNote(TestData.newspaperWithSameISSN, out Guid testNoteId);

            Assert.AreEqual(-1, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ErrorCode_ReturnsCodeOfErrorNonUniquePropertiesNewspaperCorrectly()
        {
            dao.AddNote(TestData.uniqueNewspaper1, out Guid noteId);

            int codeOfAddNoteMethod = dao.AddNote(TestData.sameNewspaperWithoutISSN, out Guid testNoteId);

            Assert.AreEqual(-1, codeOfAddNoteMethod);
        }

        [TestMethod]
        public void AddNote_ErrorCode_ReturnsCodeOfErrorForNonUniquePatentCorrectly()
        {
            dao.AddNote(TestData.uniquePatent1, out Guid noteId);

            int codeOfAddNoteMethod = dao.AddNote(TestData.samePatent, out Guid testNoteId);

            Assert.AreEqual(-1, codeOfAddNoteMethod);
        }
        #endregion

        #region GetAllNotes_TESTS
        [TestMethod]
        public void GetAllNotes_ZeroElementsCount_ReturnsZeroCountOfElementsInCreatedDAO()
        {
            int emptyCollectionCount = dao.GetAllNotes().Count;
            Assert.AreEqual(0, emptyCollectionCount);
        }

        [TestMethod]
        public void GetAllNotes_ElemetsCount_ReturnsElementsCountInDAO()
        {
            dao.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            dao.AddNote(TestData.uniqueBook2, out Guid testNoteId2);

            int elementsCount = dao.GetAllNotes().Count;

            Assert.AreEqual(2, elementsCount);
        }
        #endregion

        #region RemoveNote_TESTS
        [TestMethod]
        public void RemoveNote_TrueDeleteResult_ReturnsTrueAfterElementRemove()
        {
            dao.AddNote(TestData.uniqueBook1, out Guid noteId);

            bool isNoteWasRemoved = dao.RemoveNote();

            Assert.IsTrue(isNoteWasRemoved);
        }

        [TestMethod]
        public void RemoveNote_FalseDeleteResult_ReturnsFalseAfterDeleteTryFromEmptyDAO()
        {
            bool isNoteWasRemoved = dao.RemoveNote();

            Assert.IsFalse(isNoteWasRemoved);
        }
        #endregion
    }
}