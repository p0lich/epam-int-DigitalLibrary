using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Logic;
using Epam.DigitalLibrary.LogicContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Epam.DigitalLibrary.ConsolePL
{
    public class Program
    {
        private static readonly INoteLogic logic = new LibraryLogic();

        static void Main(string[] args)
        {
            // Prepared data

            logic.AddNote(new Book(
                name: "book1",
                authors: new List<Author> { new Author("Ivan", "Karasev"), new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "booker",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 1-56389-668-0"
                ));

            logic.AddNote(new Newspaper(
                name: "newspaper1",
                publicationPlace: "Samara",
                publisher: "samaraNews",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 5,
                objectNotes: "hi",
                number: "123",
                releaseDate: new DateTime(1900, 01, 01),
                iSSN: "ISSN1284-5678"
                ));

            logic.AddNote(new Patent(
                name: "patent1",
                authors: new List<Author> { new Author("Kiril", "Ivanov") },
                country: "Russia",
                registrationNumber: "11123",
                applicationDate: new DateTime(1890, 01, 01),
                publicationDate: new DateTime(1891, 01, 01),
                pagesCount: 20,
                objectNotes: "da"
                ));

            logic.AddNote(new Book(
                name: "book2",
                authors: new List<Author> { new Author("Ivan", "Karasev") },
                publicationPlace: "Saratov",
                publisher: "superSaratov",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: "ISBN 1-56379-668-0"
                ));

            //List<Author> a1 = new List<Author>
            //{
            //    new Author("Ivan", "Karasev"),
            //    new Author("Jhon", "Piterson"),
            //    new Author("Stepan", "Stepanich")
            //};

            //List<Author> a2 = new List<Author>
            //{
            //    new Author("Ivan", "Karasev"),
            //    new Author("Jhon", "Piterson"),
            //    new Author("Stepan", "Stepanich")
            //};

            //Console.WriteLine(a1.SequenceEqual(a2));

            //Console.WriteLine(a1.Contains(new Author("Jhona", "Piterson")));

            while (true)
            {
                Console.WriteLine(
                    "1: Add note;\n" +
                    "2: Delete note;\n" +
                    "3: Browse catalog;\n" +
                    "4: Search by name;\n" +
                    "5: Sort by year(in forward order);\n" +
                    "6: Sort by year(in reverse order);\n" +
                    "7: Search all books by author;\n" +
                    "8: Search all patents by inventor;\n" +
                    "9: Searhc all books and patents by author;\n" +
                    "10: Show books by given character set(group by publisher);\n" +
                    "11: Group by publication year;\n" +
                    "0: Exit."
                    );

                if (!int.TryParse(Console.ReadLine(), out int option))
                {
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("Input type must be integer. Try again");
                    Console.WriteLine("------------------------------------------");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input note properties:");
                        AddNote();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 2:
                        Console.WriteLine("------------------------------------------");
                        DeleteNote();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 3:
                        Console.WriteLine("------------------------------------------");
                        ShowLibrary();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 4:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input note name:");
                        SearchByName(Console.ReadLine());
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 5:
                        Console.WriteLine("------------------------------------------");
                        ForwardByYearSort();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 6:
                        Console.WriteLine("------------------------------------------");
                        ReverseByYearSort();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 7:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input author full name:");
                        SearchBookByAuthor();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 8:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input inventor full name:");
                        SearchPatentsByInventor();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 9:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input author full name:");
                        SearchBookAndPatentsByAuthor();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 10:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input charset:");
                        SearchByCharset(Console.ReadLine());
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 11:
                        Console.WriteLine("------------------------------------------");
                        GroupByYear();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Wrong option, try again:");
                        Console.WriteLine("------------------------------------------");
                        break;
                }
            }
        }

        private static void GroupByYear()
        {
            IEnumerable<IGrouping<int, Note>> groupedNotes = logic.GroupByYear();

            foreach (var group in groupedNotes)
            {
                Console.WriteLine("Year: {0}\n---------------", group.Key);

                foreach (var note in group)
                {
                    Console.WriteLine(note.ToString());
                }
            }
        }

        private static void SearchByCharset(string charSet)
        {
            IEnumerable<IGrouping<string, Book>> searchResults = logic.SearchBooksByCharset(charSet);

            foreach (var group in searchResults)
            {
                Console.WriteLine("Publisher: {0}\n---------------", group.Key);

                foreach (var book in group)
                {
                    Console.WriteLine(book.ToString());
                }
            }
        }

        private static void SearchBookAndPatentsByAuthor()
        {
            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            List<Note> notes = logic.SearchBooksAndPatensByAuthor(new Author(firstName, lastName));

            if (notes.Count == 0)
            {
                Console.WriteLine("Can't find this author notes");
                return;
            }

            for (int i = 0; i < notes.Count; i++)
            {
                Console.WriteLine(notes[i].ToString());
            }
        }

        private static void SearchPatentsByInventor()
        {
            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            List<Patent> patents = logic.SearchPatentByInventor(new Author(firstName, lastName));

            if (patents.Count == 0)
            {
                Console.WriteLine("Can't find this inventor patents");
                return;
            }

            for (int i = 0; i < patents.Count; i++)
            {
                Console.WriteLine(patents[i].ToString());
            }
        }

        private static void SearchBookByAuthor()
        {
            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            List<Book> books = logic.SearchBooksByAuthor(new Author(firstName, lastName));

            if (books.Count == 0)
            {
                Console.WriteLine("Can't find this author books");
                return;
            }

            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine(books[i].ToString());
            }
        }

        private static void ReverseByYearSort()
        {
            List<Note> sortedLib = logic.SortInReverseOrder();

            for (int i = 0; i < sortedLib.Count; i++)
            {
                Console.WriteLine(sortedLib[i].ToString());
            }
        }

        private static void ForwardByYearSort()
        {
            List<Note> sortedLib = logic.SortInOrder();

            for (int i = 0; i < sortedLib.Count; i++)
            {
                Console.WriteLine(sortedLib[i].ToString());
            }
        }

        private static void SearchByName(string name)
        {
            Note note = logic.SearchByName(name);

            if (note is null)
            {
                Console.WriteLine("Can't find this note");
                return;
            }

            Console.WriteLine(note.ToString());
        }

        private static void ShowLibrary()
        {
            List<Note> notes = logic.GetCatalog();

            if (notes.Count == 0)
            {
                Console.WriteLine("Library is empty");
            }

            for (int i = 0; i < notes.Count; i++)
            {
                Console.WriteLine(notes[i].ToString());
            }
        }

        private static void DeleteNote()
        {
            if (logic.GetCatalog().Count > 0)
            {
                logic.RemoveNote();
                Console.WriteLine("Note was deleted");
                return;
            }

            Console.WriteLine("Catalog is empty");
        }

        private static void AddNote()
        {
            Console.WriteLine(
                "Choose type of note:\n" +
                "1 - Book;\n" +
                "2 - Newspaper;\n" +
                "3 - Patent;\n" +
                "0 - Cancel.");

            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out int option))
                {
                    Console.WriteLine("Input type must be integer. Try again");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        AddBook();
                        return;

                    case 2:
                        AddNewspaper();
                        return;

                    case 3:
                        AddPatent();
                        return;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Wrong input. Try again.");
                        break;
                }
            }
        }

        private static void AddPatent()
        {
            Console.WriteLine("Leave unnecessary field empty if you don't want fill them");

            Console.Write("Patent name: ");
            string inputName = Console.ReadLine();

            Console.WriteLine("Inventors:");
            List<Author> inputAuthors = AddAuthors();

            Console.Write("Country: ");
            string inputCountry = Console.ReadLine();

            Console.Write("Registration number: ");
            string inputRegistrationNumber = Console.ReadLine();

            Console.Write("Application date: ");
            string inputApplicationDate = Console.ReadLine();

            Console.Write("Publication date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime inputPublicationDate))
            {
                Console.WriteLine("Wrong format of publication date input. No patent added");
                return;
            }

            Console.Write("Pages count: ");
            if (!int.TryParse(Console.ReadLine(), out int inputPagesCount))
            {
                Console.WriteLine("Wrong format of page count input. No patent added");
                return;
            }

            Console.Write("Patent notes: ");
            string inputObjectNotes = Console.ReadLine();

            int addResult = logic.AddNote(new Patent(
            name: inputName,
            authors: inputAuthors,
            country: inputCountry,
            registrationNumber: inputRegistrationNumber,
            applicationDate: string.IsNullOrEmpty(inputApplicationDate) ? null : DateTime.Parse(inputApplicationDate),
            publicationDate: inputPublicationDate,
            pagesCount: inputPagesCount,
            objectNotes: string.IsNullOrEmpty(inputObjectNotes) ? null : inputObjectNotes
            ));

            if (addResult == 0)
            {
                Console.WriteLine("Patent was added");
                return;
            }

            if (addResult == -1)
            {
                Console.WriteLine("Same patent already exist");
                return;
            }

            if (addResult == -2)
            {
                Console.WriteLine("Error adding patent");
            }
        }

        private static void AddNewspaper()
        {
            Console.WriteLine("Leave unnecessary field empty if you don't want fill them");

            Console.Write("Newspaper name: ");
            string inputName = Console.ReadLine();

            Console.Write("Publication place: ");
            string inputPublicationPlace = Console.ReadLine();

            Console.Write("Publisher: ");
            string inputPublisher = Console.ReadLine();

            Console.Write("Publication date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime inputPublicationDate))
            {
                Console.WriteLine("Wrong format of publication date input. No newspaper added");
                return;
            }

            Console.Write("Pages count: ");
            if (!int.TryParse(Console.ReadLine(), out int inputPagesCount))
            {
                Console.WriteLine("Wrong format of page count input. No newspaper added");
                return;
            }

            Console.Write("Newspaper notes: ");
            string inputObjectNotes = Console.ReadLine();

            Console.Write("Number: ");
            string inputNumber = Console.ReadLine();

            Console.Write("Release date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime inputReleaseDate))
            {
                Console.WriteLine("Wrong format of release date input. No newspaper added");
                return;
            }

            Console.Write("ISSN: ");
            string inputISSN = Console.ReadLine();

            int addResult = logic.AddNote(new Newspaper(
            name: inputName,
            publicationPlace: inputPublicationPlace,
            publisher: inputPublisher,
            publicationDate: inputPublicationDate,
            pagesCount: inputPagesCount,
            objectNotes: string.IsNullOrEmpty(inputObjectNotes) ? null : inputObjectNotes,
            number: string.IsNullOrEmpty(inputNumber) ? null : inputNumber,
            releaseDate: inputReleaseDate,
            iSSN: string.IsNullOrEmpty(inputISSN) ? null : inputISSN
            ));

            if (addResult == 0)
            {
                Console.WriteLine("Newspaper was added");
                return;
            }

            if (addResult == -1)
            {
                Console.WriteLine("Same newspaper already exist");
                return;
            }

            if (addResult == -2)
            {
                Console.WriteLine("Error adding newspaper");
            }
        }

        private static void AddBook()
        {
            Console.WriteLine("Leave unnecessary field empty if you don't want fill them");

            Console.Write("Book name: ");
            string inputName = Console.ReadLine();

            Console.WriteLine("Authors:");
            List<Author> inputAuthors = AddAuthors();

            Console.Write("Publication place: ");
            string inputPublicationPlace = Console.ReadLine();

            Console.Write("Publisher: ");
            string inputPublisher = Console.ReadLine();

            Console.Write("Publication date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime inputPublicationDate))
            {
                Console.WriteLine("Wrong format of publication date input. No newspaper added");
                return;
            }

            Console.Write("Pages count: ");
            if (!int.TryParse(Console.ReadLine(), out int inputPagesCount))
            {
                Console.WriteLine("Wrong format of page count input. No newspaper added");
                return;
            }

            Console.Write("Book notes: ");
            string inputObjectNotes = Console.ReadLine();

            Console.Write("ISBN: ");
            string inputISBN = Console.ReadLine();

            int addResult = logic.AddNote(new Book(
            name: inputName,
            authors: inputAuthors,
            publicationPlace: inputPublicationPlace,
            publisher: inputPublisher,
            publicationDate: inputPublicationDate,
            pagesCount: inputPagesCount,
            objectNotes: string.IsNullOrEmpty(inputObjectNotes) ? null : inputObjectNotes,
            iSBN: string.IsNullOrEmpty(inputISBN) ? null : inputISBN
            ));

            if (addResult == 0)
            {
                Console.WriteLine("Book was added");
                return;
            }

            if (addResult == -1)
            {
                Console.WriteLine("Same book already exist");
                return;
            }

            if (addResult == -2)
            {
                Console.WriteLine("Error adding book");
            }
        }

        private static List<Author> AddAuthors()
        {
            List<Author> authors = new List<Author>();

            while (true)
            {
                Console.WriteLine(
                "1 - add author\n" +
                "2 - finish");

                if (!int.TryParse(Console.ReadLine(), out int option))
                {
                    Console.WriteLine("Input type must be integer. Try again");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        AddAuthor(authors);
                        break;

                    case 2:
                        if (authors.Count == 0)
                        {
                            Console.WriteLine("There must be at least 1 author");
                            break;
                        }
                        return authors;

                    default:
                        Console.WriteLine("Wrong input. Try again.");
                        break;
                }
            }
        }

        private static void AddAuthor(List<Author> authors)
        {
            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            try
            {
                authors.Add(new Author(firstName, lastName));
                return;
            }

            catch (Exception)
            {
                Console.WriteLine("Fields were filled incorrectly. No author added");
                return;
            }
        }

        private static void ShowNote(Note note)
        {
            Console.WriteLine(note.ToString());
        }
    }
}
