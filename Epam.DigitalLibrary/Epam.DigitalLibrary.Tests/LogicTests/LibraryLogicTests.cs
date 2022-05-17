using Moq;
using Epam.DigitalLibrary.DalContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.Logic;

namespace Epam.DigitalLibrary.Tests.LogicTests
{
    [TestClass]
    public class LibraryLogicTests
    {
        private INoteLogic logic;

        [TestInitialize]
        public void InitializeLogic()
        {
            logic = new LibraryLogic();
        }

        #region AddNote_TESTS
        [TestMethod]
        public void AddNote_ZeroReturn_ReturnsZeroCodeForAddingBookCorrectly()
        {
            Book bookForAdd = TestData.uniqueBook1;

            int addResult = logic.AddNote(bookForAdd, out Guid noteId);

            Assert.AreEqual(0, addResult);
        }

        [TestMethod]
        public void AddNote_ErrorCode_ReturnsErrorCodeForIncorrectNoteCorrectly()
        {
            int addNullResult = logic.AddNote(null, out Guid noteId);

            Assert.AreEqual(-2, addNullResult);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForNotUniqueBooks), DynamicDataSourceType.Method)]
        public void AddNote_NotUniqueCode_ReturnsMinusOneAfterAddingSameBookCorrectly(Book testBook)
        {
            Book uniqueBook = TestData.uniqueBook1;

            logic.AddNote(uniqueBook, out Guid noteId);

            int addSameNoteResult = logic.AddNote(testBook, out Guid sameNoteId);

            Assert.AreEqual(-1, addSameNoteResult);
        }

        private static IEnumerable<object[]> GetTestDataForNotUniqueBooks()
        {
            return new[]
            {
                new object[] {TestData.bookWithSameISBN},
                new object[] {TestData.sameBookWithoutISBN},
            };
        }

        [TestMethod]
        public void AddNote_ZeroReturn_ReturnsZeroCodeForAddingNewspaperCorrectly()
        {
            Newspaper uniqueNewspaper = TestData.uniqueNewspaper1;

            int addResult = logic.AddNote(uniqueNewspaper, out Guid noteId);

            Assert.AreEqual(0, addResult);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestDataForNotUniqueNewspapers), DynamicDataSourceType.Method)]
        public void AddNote_NotUniqueCode_ReturnsMinusOneAfterAddingSameNewspaperCorrectly(Newspaper testNewspaper)
        {
            Newspaper uniqueNewspaper = TestData.uniqueNewspaper1;

            logic.AddNote(uniqueNewspaper, out Guid noteId);

            int addSamenoteResult = logic.AddNote(testNewspaper, out Guid sameNoteId);

            Assert.AreEqual(-1, addSamenoteResult);
        }

        private static IEnumerable<object[]> GetTestDataForNotUniqueNewspapers()
        {
            return new[]
            {
                new object[] {TestData.newspaperWithSameISSN},
                new object[] {TestData.sameNewspaperWithoutISSN},
            };
        }

        [TestMethod]
        public void AddNote_ZeroReturn_ReturnsZeroCodeForAddingPatentCorrectly()
        {
            Patent uniquePatent = TestData.uniquePatent1;

            int addResult = logic.AddNote(uniquePatent, out Guid noteId);

            Assert.AreEqual(0, addResult);
        }

        [TestMethod]
        public void AddNote_NotUniqueCode_ReturnsMinusOneAfterAddingNotUniquePatentCorrectly()
        {
            Patent uniquePatent = TestData.uniquePatent1;
            Patent samePatent = TestData.samePatent;

            logic.AddNote(uniquePatent, out Guid noteId);

            int addSamenoteResult = logic.AddNote(samePatent, out Guid sameNoteId);

            Assert.AreEqual(-1, addSamenoteResult);
        }
        #endregion

        #region GroupByYear_TESTS
        [TestMethod]
        public void GroupByYear_GroupsCount_ReturnsGroupsByNoteYearCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId3);

            IEnumerable<IGrouping<int, Note>> groupResult = logic.GroupByYear();

            int countOfGroups = groupResult.Count();

            Assert.AreEqual(2, countOfGroups);
        }

        [TestMethod]
        public void GroupByYear_NotesCount_ReturnCountOfNotesCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId3);

            IEnumerable<IGrouping<int, Note>> groupResult = logic.GroupByYear();

            int notesCount = 0;

            foreach (var group in groupResult)
            {
                notesCount += group.Count();
            }

            Assert.AreEqual(3, notesCount);
        }

        [TestMethod]
        public void GroupByYear_GroupsCount_GroupingEmptycatalogByYearCorrectly()
        {
            IEnumerable<IGrouping<int, Note>> groupResult = logic.GroupByYear();

            int countOfEmptyGroups = groupResult.Count();

            Assert.AreEqual(0, countOfEmptyGroups);
        }
        #endregion

        #region RemoveNote_TESTS
        [TestMethod]
        public void RemoveNote_IsNoteRemoved_RemoveNoteCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid noteId);

            bool positiveRemoveResult = logic.RemoveNote();

            Assert.IsTrue(positiveRemoveResult);
        }

        [TestMethod]
        public void RemoveNote_IsNoteRemoved_TryRemoveNoteFromEmptyListCorrectly()
        {
            bool negativeRemoveResult = logic.RemoveNote();

            Assert.IsFalse(negativeRemoveResult);
        }
        #endregion

        #region SearchBooksAndPatensByAuthor_TESTS
        [TestMethod]
        public void SearchBooksAndPatensByAuthor_NotesCount_ReturnsCountOfFoundBooksAndPatentsByAuthorCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testBookId1);
            logic.AddNote(TestData.uniqueBook3, out Guid testBookId2);
            logic.AddNote(TestData.uniquePatent1, out Guid testPatentId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testPatentId2);

            Author author = TestData.existAuthor2;

            int searchResultCount = logic.SearchBooksAndPatensByAuthor(author).Count;

            Assert.AreEqual(4, searchResultCount);
        }

        [TestMethod]
        public void SearchBooksAndPatensByAuthor_NotesCount_ReturnsCountOfFoundBooksAndPatentsInCollectionWithoutPatentsByAuthorCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId2);

            Author author = TestData.existAuthor2;

            int searchResultCount = logic.SearchBooksAndPatensByAuthor(author).Count;

            Assert.AreEqual(2, searchResultCount);
        }

        [TestMethod]
        public void SearchBooksAndPatensByAuthor_NotesCount_ReturnsCountOfFoundBooksAndPatentsInCollectionWithoutBooksByAuthorCorrectly()
        {
            logic.AddNote(TestData.uniquePatent1, out Guid testNoteId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testNoteId2);

            Author author = TestData.existAuthor2;

            int searchResultCount = logic.SearchBooksAndPatensByAuthor(author).Count;

            Assert.AreEqual(2, searchResultCount);
        }

        [TestMethod]
        public void SearchBooksAndPatensByAuthor_NotesCount_ReturnsCountOfFoundBooksAndPatentsByNullReffAuthorCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testBookId1);
            logic.AddNote(TestData.uniqueBook3, out Guid testBookId2);
            logic.AddNote(TestData.uniquePatent1, out Guid testPatentId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testPatentId2);

            Author authorWithNullRef = TestData.nullRefAuthor;

            int searchResultCount = logic.SearchBooksAndPatensByAuthor(authorWithNullRef).Count;

            Assert.AreEqual(0, searchResultCount);
        }

        [TestMethod]
        public void SearchBooksAndPatensByAuthor_NotesCount_ReturnsCountOfFoundBooksAndPatentsInEmptyLibraryCorrectly()
        {
            Author author = TestData.existAuthor2;

            int searchResultInEmptyLibraryCount = logic.SearchBooksAndPatensByAuthor(author).Count;

            Assert.AreEqual(0, searchResultInEmptyLibraryCount);
        }
        #endregion

        #region SearchBooksByAuthor_TESTS
        [TestMethod]
        public void SearchBooksByAuthor_BooksCount_ReturnsCountOfFoundBooksByAuthorCorrectly()
        {

            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);

            Author author = TestData.existAuthor1;

            int searchResultCount = logic.SearchBooksByAuthor(author).Count;

            Assert.AreEqual(2, searchResultCount);
        }

        [TestMethod]
        public void SearchBooksByAuthor_ZeroBooksCount_ReturnsCountOfFoundBooksByNullRefAuthorCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);

            Author authorWithNullRef = TestData.nullRefAuthor;

            int searchResultCount = logic.SearchBooksByAuthor(authorWithNullRef).Count;

            Assert.AreEqual(0, searchResultCount);
        }

        [TestMethod]
        public void SearchBooksByAuthor_ZeroBooksCount_ReturnsCountOfFoundBooksByAuthorInEmptyLibraryCorrectly()
        {
            Author author = TestData.existAuthor1;

            int searhcResultInEmptyLibraryCount = logic.SearchBooksByAuthor(author).Count;

            Assert.AreEqual(0, searhcResultInEmptyLibraryCount);
        }
        #endregion

        #region SearchBooksByCharset_TESTS
        [TestMethod]
        public void SearchBooksByCharset_GroupsCount_ReturnsCountOfGroupsOfFoundedBooksByCharSetCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);
            logic.AddNote(TestData.uniqueBook4, out Guid testNoteId4);

            string charSet = TestData.charSetOfExistBook;

            int foundedGroupsCount = logic.SearchBooksByCharset(charSet).Count();

            Assert.AreEqual(2, foundedGroupsCount);
        }

        [TestMethod]
        public void SearchBooksByCharset_BooksCount_ReturnsCountOfFoundedBooksByCharSetCorrect()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);
            logic.AddNote(TestData.uniqueBook4, out Guid testNoteId4);

            string charSet = TestData.charSetOfExistBook;

            int foundedBooksCount = 0;

            IEnumerable<IGrouping<string, Book>> foundedBooks = logic.SearchBooksByCharset(charSet);
            foreach (var group in foundedBooks)
            {
                foundedBooksCount += group.Count();
            }

            Assert.AreEqual(3, foundedBooksCount);
        }

        [TestMethod]
        public void SearchBooksByCharset_AllBooksCount_ReturnsCountOfFoundedBooksByEmptyCharSetCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);
            logic.AddNote(TestData.uniqueBook4, out Guid testNoteId4);

            string emptyCharSet = TestData.emptyCharSet;

            int foundedBooksCount = 0;

            IEnumerable<IGrouping<string, Book>> foundedBooks = logic.SearchBooksByCharset(emptyCharSet);
            foreach (var group in foundedBooks)
            {
                foundedBooksCount += group.Count();
            }

            Assert.AreEqual(4, foundedBooksCount);
        }

        [TestMethod]
        public void SearchBooksByCharset_AllGroupsCount_ReturnsAllGroupsOfFoundedBooksByNullRefCharSetCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);
            logic.AddNote(TestData.uniqueBook4, out Guid testNoteId4);

            string nullCharSet = TestData.nullRefCharSet;

            int foundedGroupsCount = logic.SearchBooksByCharset(nullCharSet).Count();

            Assert.AreEqual(3, foundedGroupsCount);
        }

        [TestMethod]
        public void SearchBooksByCharset_ZeroGroupsCount_ReturnsEmptyCollectionOfGroupsOfFoundedBooksByCharSetInEmptyCollectionCorrectly()
        {
            string charSet = TestData.charSetOfExistBook;

            int foundedInEmptyCollectionGroupsCount = logic.SearchBooksByCharset(charSet).Count();

            Assert.AreEqual(0, foundedInEmptyCollectionGroupsCount);
        }
        #endregion

        #region SearchByName_TESTS
        [TestMethod]
        public void SearchByName_NotNullNote_ReturnsNoteByNameCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid noteId);

            string noteName = TestData.existName;
            Note foundNoteResult = logic.SearchByName(noteName);

            Assert.IsNotNull(foundNoteResult);
        }

        [TestMethod]
        public void SearchByName_Null_ReturnsNullByNonExistingNameCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid noteId);

            string wrongName = TestData.nonExistName;
            Note notFoundedNoteResult = logic.SearchByName(wrongName);

            Assert.IsNull(notFoundedNoteResult);
        }

        [TestMethod]
        public void SearchByName_Null_ReturnsNullByNullRefNameCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid noteId);

            string nullName = TestData.nameWithNullRef;
            Note notFoundedNoteResult = logic.SearchByName(nullName);

            Assert.IsNull(notFoundedNoteResult);
        }
        #endregion

        #region SearchPatentByInventor_TESTS
        [TestMethod]
        public void SearchPatentByInventor_PatentsCount_ReturnsCountOfFoundPatentsByAuthorCorrectly()
        {

            logic.AddNote(TestData.uniquePatent1, out Guid testNoteId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testNoteId2);
            logic.AddNote(TestData.uniquePatent3, out Guid testNoteId3);

            Author author = TestData.existAuthor2;

            int searchResultCount = logic.SearchPatentByInventor(author).Count;

            Assert.AreEqual(2, searchResultCount);
        }

        [TestMethod]
        public void SearchPatentByInventor_ZeroPatentsCount_ReturnsCountOfFoundPatentsByNullRefPatentsCorrectly()
        {
            logic.AddNote(TestData.uniquePatent1, out Guid testNoteId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testNoteId2);
            logic.AddNote(TestData.uniquePatent3, out Guid testNoteId3);

            Author authorWithNullRef = TestData.nullRefAuthor;

            int searchResultCount = logic.SearchPatentByInventor(authorWithNullRef).Count;

            Assert.AreEqual(0, searchResultCount);
        }

        [TestMethod]
        public void SearchPatentByInventor_ZeroPatentsCount_ReturnsCountOfFoundPatentsByAuthorInEmptyLibraryCorrectly()
        {
            Author author = TestData.existAuthor2;

            int searhcResultInEmptyLibraryCount = logic.SearchPatentByInventor(author).Count;

            Assert.AreEqual(0, searhcResultInEmptyLibraryCount);
        }
        #endregion

        #region GetCatalog_TESTS
        [TestMethod]
        public void GetCatalog_ZeroNotesCount_ReturnsZeroNotesCountAfterInitializingDAOCorrectly()
        {
            int emptyDataCount = logic.GetCatalog().Count;

            Assert.AreEqual(0, emptyDataCount);
        }

        [TestMethod]
        public void GetCatalog_AllNotes_ReturnsAllNotesCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);

            int searchResultCount = logic.GetCatalog().Count;

            Assert.AreEqual(2, searchResultCount);
        }
        #endregion

        #region SortInOrder_TESTS
        [TestMethod]
        public void SortInOrder_ZeroNotesCount_ReturnsEmptyListAfterForwardSortOfEmptyLibraryCorrectly()
        {
            int emptyListCount = logic.SortInOrder().Count;

            Assert.AreEqual(0, emptyListCount);
        }

        [TestMethod]
        public void SortInOrder_AllNotes_ReturnsSameCountOfNotesAfterForwardSortCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testBookId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testBookId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testBookId3);
            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNewspaperId1);
            logic.AddNote(TestData.uniqueNewspaper2, out Guid testNewspaperId2);
            logic.AddNote(TestData.uniqueNewspaper3, out Guid testNewspaperId3);
            logic.AddNote(TestData.uniquePatent1, out Guid testPatentId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testPatentId2);
            logic.AddNote(TestData.uniquePatent3, out Guid testPatentId3);

            int sortedElementsCount = logic.SortInOrder().Count;

            Assert.AreEqual(9, sortedElementsCount);
        }

        [TestMethod]
        public void SortInOrder_IsOrderRight_CheckIsAllNotesAfterForwardSortInCorrectOrder()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testBookId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testBookId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testBookId3);
            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNewspaperId1);
            logic.AddNote(TestData.uniqueNewspaper2, out Guid testNewspaperId2);
            logic.AddNote(TestData.uniqueNewspaper3, out Guid testNewspaperId3);
            logic.AddNote(TestData.uniquePatent1, out Guid testPatentId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testPatentId2);
            logic.AddNote(TestData.uniquePatent3, out Guid testPatentId3);

            List<Note> forwardSortedLibrary = logic.SortInOrder();
            bool isOrderCorrect = true;

            for (int i = 1; i < forwardSortedLibrary.Count; i++)
            {
                if (forwardSortedLibrary[i - 1].PublicationDate.Year > forwardSortedLibrary[i].PublicationDate.Year)
                {
                    isOrderCorrect = false;
                    break;
                }
            }

            Assert.IsTrue(isOrderCorrect);
        }
        #endregion

        #region SortInReverseOrder_TESTS
        [TestMethod]
        public void SortInReverseOrder_ZeroNotesCount_ReturnsEmptyListAfterReverseSortOfEmptyLibraryCorrectly()
        {
            int emptyListCount = logic.SortInReverseOrder().Count;

            Assert.AreEqual(0, emptyListCount);
        }

        [TestMethod]
        public void SortInReverseOrder_AllNotes_ReturnsSameCountOfNotesAfterReverseSortCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testBookId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testBookId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testBookId3);
            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNewspaperId1);
            logic.AddNote(TestData.uniqueNewspaper2, out Guid testNewspaperId2);
            logic.AddNote(TestData.uniqueNewspaper3, out Guid testNewspaperId3);
            logic.AddNote(TestData.uniquePatent1, out Guid testPatentId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testPatentId2);
            logic.AddNote(TestData.uniquePatent3, out Guid testPatentId3);

            int reverseSortedElementsCount = logic.SortInReverseOrder().Count;

            Assert.AreEqual(9, reverseSortedElementsCount);
        }

        [TestMethod]
        public void SortInReverseOrder_IsOrderRight_CheckIsAllNotesAfterReverseSortInCorrectOrder()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testBookId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testBookId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testBookId3);
            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNewspaperId1);
            logic.AddNote(TestData.uniqueNewspaper2, out Guid testNewspaperId2);
            logic.AddNote(TestData.uniqueNewspaper3, out Guid testNewspaperId3);
            logic.AddNote(TestData.uniquePatent1, out Guid testPatentId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testPatentId2);
            logic.AddNote(TestData.uniquePatent3, out Guid testPatentId3);

            List<Note> reversedSortedLibrary = logic.SortInReverseOrder();
            bool isReverseOrderCorrect = true;

            for (int i = 1; i < reversedSortedLibrary.Count; i++)
            {
                if (reversedSortedLibrary[i - 1].PublicationDate.Year < reversedSortedLibrary[i].PublicationDate.Year)
                {
                    isReverseOrderCorrect = false;
                    break;
                }
            }

            Assert.IsTrue(isReverseOrderCorrect);
        }
        #endregion
    }
}