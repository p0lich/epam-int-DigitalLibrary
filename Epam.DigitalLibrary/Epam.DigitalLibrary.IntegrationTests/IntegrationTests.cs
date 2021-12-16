using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Logic;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.IntegrationTests
{
    [TestClass]
    public class IntegrationTests
    {
        private INoteLogic logic;

        [TestInitialize]
        public void InitializeLogic()
        {
            logic = new LibraryLogic();
        }

        [TestMethod]
        public void AddNote_CheckNoteInCollection_CompareNoteBeforeAddAndAfter()
        {
            int initialLibraryCount = logic.GetCatalog().Count;

            Book bookForAdd = TestData.uniqueBook1;
            Guid addedBookGuid = bookForAdd.ID;

            logic.AddNote(bookForAdd, out Guid noteId);
            int countAfterAdd = logic.GetCatalog().Count;

            Note foundedNote = logic.SearchByName(bookForAdd.Name);
            Guid foundedBookGuid = foundedNote.ID;

            Assert.AreEqual(addedBookGuid, foundedBookGuid);
        }

        [TestMethod]
        public void AddNote_SameCollection_ReturnCollectionWithoutChangesAfterTryAddingSameNoteCorrectly()
        {
            int initialLibraryCount = logic.GetCatalog().Count;

            Book uniqueBookForAdd = TestData.uniqueBook1;

            int addResult = logic.AddNote(uniqueBookForAdd, out Guid noteId);
            int noteCountAfterAdd = logic.GetCatalog().Count;

            Book sameBook = TestData.sameBookWithoutISBN;

            int addSameresult = logic.AddNote(sameBook, out Guid sameNoteId);
            int noteCountAfterAddSame = logic.GetCatalog().Count;

            Assert.AreEqual(noteCountAfterAdd, noteCountAfterAddSame);
        }

        [TestMethod]
        public void AddNote_SameCollection_ReturnsCollectionWithoutChangesAfterTryAddingIncorrectNote()
        {
            int initialLibraryCount = logic.GetCatalog().Count;

            Note incorrectNote = null;

            int addResult = logic.AddNote(incorrectNote, out Guid noteId);
            int noteCountAfterAdd = logic.GetCatalog().Count;

            Assert.AreEqual(initialLibraryCount, noteCountAfterAdd);
        }

        [TestMethod]
        public void RemoveNote_DecreasedCollectionCount_ReturnCollectionWithoutOneSpecificNoteAfterRemoveCorrectly()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);

            int noteCountAfterAdd = logic.GetCatalog().Count;

            Book removedBook = TestData.uniqueBook1;

            bool removeResult = logic.RemoveNote();

            int collectionCountAfterNoteRemove = logic.GetCatalog().Count;

            Note nullNote = logic.SearchByName(removedBook.Name);

            Assert.IsNull(nullNote);
        }

        [TestMethod]
        public void RemoveNote_SameCollection_ReturnsSameEmptyCollectionAfterDeleteTryCorectly()
        {
            int initialLibraryCount = logic.GetCatalog().Count;

            bool removeresult = logic.RemoveNote();

            int libraryCountAfterRemove = logic.GetCatalog().Count;

            Assert.AreEqual(initialLibraryCount, libraryCountAfterRemove);
        }

        [TestMethod]
        public void AddNote_SameCollection_ReturnsSameCollectionAfterNoteReAddCorrectly()
        {
            int initialLibraryCount = logic.GetCatalog().Count;

            int addResult = logic.AddNote(TestData.uniqueBook1, out Guid noteId);
            int libraryCountAfterAdd = logic.GetCatalog().Count;

            bool removeResult = logic.RemoveNote();
            int libraryCountAfterRemove = logic.GetCatalog().Count;

            int reAddResult = logic.AddNote(TestData.uniqueBook1, out Guid reAddedNoteId);
            int libraryCountAfterReAdd = logic.GetCatalog().Count;

            Assert.AreEqual(libraryCountAfterAdd, libraryCountAfterReAdd);
        }

        [TestMethod]
        public void SortInOrder_SameCollection_SortDoesNotAffectCatalogTest()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId3);

            int collectionCount = logic.GetCatalog().Count;

            List<Note> library = logic.GetCatalog();
            List<Note> sortedLibrary = logic.SortInOrder();

            bool isCollectionsSame = true;

            for (int i = 0; i < collectionCount; i++)
            {
                if (library[i].ID != sortedLibrary[i].ID)
                {
                    isCollectionsSame = false;
                    break;
                }
            }

            Assert.IsFalse(isCollectionsSame);
        }

        [TestMethod]
        public void SortInReverseOrder_SameCollection_SortDoesNotAffectCatalogTest()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId3);

            int collectionCount = logic.GetCatalog().Count;

            List<Note> library = logic.GetCatalog();
            List<Note> sortedLibrary = logic.SortInReverseOrder();

            bool isCollectionsSame = true;

            for (int i = 0; i < collectionCount; i++)
            {
                if (library[i].ID != sortedLibrary[i].ID)
                {
                    isCollectionsSame = false;
                    break;
                }
            }

            Assert.IsFalse(isCollectionsSame);
        }

        [TestMethod]
        public void SortInOrder_SameSortedCollection_SortedCollectionDoesNotAffectByLibrary()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId3);

            List<Note> sortedLibrary = logic.SortInOrder();
            int sortedLibraryCount = sortedLibrary.Count;

            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNewspaperId);

            int catalogCount = logic.GetCatalog().Count;

            bool isCollectionsSame = sortedLibraryCount == catalogCount;

            Assert.IsFalse(isCollectionsSame);
        }

        [TestMethod]
        public void SortInReverseOrder_SameSortedCollection_SortedCollectionDoesNotAffectByLibrary()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId3);

            List<Note> sortedLibrary = logic.SortInReverseOrder();
            int sortedLibraryCount = sortedLibrary.Count;

            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNewspaperId);

            int catalogCount = logic.GetCatalog().Count;

            bool isCollectionsSame = sortedLibraryCount == catalogCount;

            Assert.IsFalse(isCollectionsSame);
        }

        [TestMethod]
        public void SearchByName_FirstWithName_ReturnsFirstNoteWithSameNameCorrectly()
        {
            Book book = TestData.uniqueBook1;
            string searchName = book.Name;
            logic.AddNote(book, out Guid noteId);

            Note firstTimeFoundedNote = logic.SearchByName(searchName);

            Book uniqueBookWithSameName = TestData.uniqueBook11;
            logic.AddNote(uniqueBookWithSameName, out Guid sameNoteId);

            Note secondTimeFoundedNote = logic.SearchByName(searchName);

            Assert.AreEqual(firstTimeFoundedNote, secondTimeFoundedNote);
        }

        [TestMethod]
        public void GroupByYear_CheckYear_CompareYearsInGroup()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);
            logic.AddNote(TestData.uniqueNewspaper1, out Guid testNewspaperId1);
            logic.AddNote(TestData.uniqueNewspaper3, out Guid testNewspaperId2);

            IEnumerable<IGrouping<int, Note>> groupedByYearNotes = logic.GroupByYear();

            bool isYearMismatch = false;

            foreach (var group in groupedByYearNotes)
            {
                foreach (var note in group)
                {
                    if (note.PublicationDate.Year != group.Key)
                    {
                        isYearMismatch = true;
                    }
                }
            }

            Assert.IsFalse(isYearMismatch);
        }

        [TestMethod]
        public void SearchBooksByAuthor_BooksHaveSameAuthor_CheckAuthorForEachBook()
        {
            Author authorForSearch = TestData.existAuthor1;

            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook3, out Guid testNoteId3);

            List<Book> foundedBooks = logic.SearchBooksByAuthor(authorForSearch);

            bool isAuthorMismatch = false;

            foreach (var note in foundedBooks)
            {
                if (!note.Authors.Contains(authorForSearch))
                {
                    isAuthorMismatch = true;
                    break;
                }
            }

            Assert.IsFalse(isAuthorMismatch);
        }

        [TestMethod]
        public void SearchPatentByInventor_PatentsHaveSameAuthor_CheckAuthorForEachPatent()
        {
            Author authorForSearch = TestData.existAuthor2;

            logic.AddNote(TestData.uniquePatent1, out Guid testNoteId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testNoteId2);
            logic.AddNote(TestData.uniquePatent3, out Guid testNoteId3);

            List<Patent> foundedNotes = logic.SearchPatentByInventor(authorForSearch);

            IEnumerable<Patent> foundedPatents = foundedNotes.OfType<Patent>();

            bool isAuthorMismatch = false;

            foreach (var note in foundedPatents)
            {
                if (!note.Authors.Contains(authorForSearch))
                {
                    isAuthorMismatch = true;
                    break;
                }
            }

            Assert.IsFalse(isAuthorMismatch);
        }

        [TestMethod]
        public void SearchBooksAndPatensByAuthor_NotesHaveSameAuthor_CheckAuthorForEachNote()
        {
            Author authorForSearch = TestData.existAuthor2;

            logic.AddNote(TestData.uniqueBook1, out Guid testBookId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testBookId2);
            logic.AddNote(TestData.uniquePatent1, out Guid testPatentId1);
            logic.AddNote(TestData.uniquePatent2, out Guid testPatentId2);

            List<Note> foundedNotes = logic.SearchBooksAndPatensByAuthor(authorForSearch);

            IEnumerable<Book> foundedBooks = foundedNotes.OfType<Book>();
            IEnumerable<Patent> foundedPatents = foundedNotes.OfType<Patent>();

            bool isAuthorMismatch = false;

            foreach (var note in foundedBooks)
            {
                if (!note.Authors.Contains(authorForSearch))
                {
                    isAuthorMismatch = true;
                    break;
                }
            }

            if (!isAuthorMismatch)
            {
                foreach (var note in foundedPatents)
                {
                    if (!note.Authors.Contains(authorForSearch))
                    {
                        isAuthorMismatch = true;
                        break;
                    }
                }
            }

            Assert.IsFalse(isAuthorMismatch);
        }

        [TestMethod]
        public void SearchBooksByCharset_CheckPublisher_CheckAllGroupsHaveSamePublisher()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook4, out Guid testNoteId3);

            string testCharset = TestData.charSetOfExistBook;

            IEnumerable<IGrouping<string, Book>> foundedBooks = logic.SearchBooksByCharset(testCharset);

            bool isPublisherMismatch = false;

            foreach (var group in foundedBooks)
            {
                foreach (var note in group)
                {
                    if (note.Publisher != group.Key)
                    {
                        isPublisherMismatch = true;
                    }
                }
            }

            Assert.IsFalse(isPublisherMismatch);
        }

        [TestMethod]
        public void SearchBooksByCharset_CheckBookName_CheckAllBooksNamesForMismatch()
        {
            logic.AddNote(TestData.uniqueBook1, out Guid testNoteId1);
            logic.AddNote(TestData.uniqueBook2, out Guid testNoteId2);
            logic.AddNote(TestData.uniqueBook4, out Guid testNoteId3);

            string testCharset = TestData.charSetOfExistBook;
            Regex charSetReg = new Regex($@"^{testCharset}");

            IEnumerable<IGrouping<string, Book>> foundedBooks = logic.SearchBooksByCharset(testCharset);

            bool isPublisherMismatch = false;

            foreach (var group in foundedBooks)
            {
                foreach (var note in group)
                {
                    if (!charSetReg.IsMatch(note.Name))
                    {
                        isPublisherMismatch = true;
                    }
                }
            }

            Assert.IsFalse(isPublisherMismatch);
        }
    }
}