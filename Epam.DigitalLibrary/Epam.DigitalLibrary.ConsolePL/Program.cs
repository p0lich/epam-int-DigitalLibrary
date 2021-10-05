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

                        int addResult = AddNote();
                        PrintAddResult(addResult);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 2:
                        Console.WriteLine("------------------------------------------");
                        DeleteNote();
                        Console.WriteLine("------------------------------------------");
                        break;

                    case 3:
                        Console.WriteLine("------------------------------------------");

                        List<Note> allNotes = logic.GetCatalog();
                        PrintNotes(allNotes);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 4:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input note name:");

                        Note note = logic.SearchByName(Console.ReadLine());
                        PrintNote(note);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 5:
                        Console.WriteLine("------------------------------------------");

                        List<Note> sortedLib = logic.SortInOrder();
                        PrintNotes(sortedLib);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 6:
                        Console.WriteLine("------------------------------------------");

                        List<Note> reverseSortedLib = logic.SortInReverseOrder();
                        PrintNotes(reverseSortedLib);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 7:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input author full name:");

                        Author bookAuthor = InputAuthor();
                        List<Book> books = logic.SearchBooksByAuthor(bookAuthor);
                        PrintNotes(books);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 8:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input inventor full name:");

                        Author patentInventor = InputAuthor();
                        List<Patent> patents = logic.SearchPatentByInventor(patentInventor);
                        PrintNotes(patents);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 9:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input author full name:");

                        Author author = InputAuthor();
                        List<Note> foundNotes = logic.SearchBooksAndPatensByAuthor(author);
                        PrintNotes(foundNotes);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 10:
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine("Input charset:");

                        string charSet = Console.ReadLine();
                        IEnumerable<IGrouping<string, Book>> groupedBooks = logic.SearchBooksByCharset(charSet);
                        PrintGroup(groupedBooks);

                        Console.WriteLine("------------------------------------------");
                        break;

                    case 11:
                        Console.WriteLine("------------------------------------------");

                        IEnumerable<IGrouping<int, Note>> groupedNotes = logic.GroupByYear();
                        PrintGroup(groupedNotes);

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

        private static int AddNote()
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

                try
                {
                    switch (option)
                    {
                        case 1:
                            Book book = InputBook();
                            return logic.AddNote(book);

                        case 2:
                            Newspaper newspaper = InputNewspaper();
                            return logic.AddNote(newspaper);

                        case 3:
                            Patent patent = InputPatent();
                            return logic.AddNote(patent);

                        default:
                            Console.WriteLine("Wrong input. Try again.");
                            break;
                    }
                }

                catch (InvalidCastException)
                {
                    return -3;
                }
            }
        }

        private static void PrintAddResult(int addResult)
        {
            switch (addResult)
            {
                case 0:
                    Console.WriteLine("Note was added");
                    return;

                case -1:
                    Console.WriteLine("Same note already exist");
                    return;

                case -2:
                    Console.WriteLine("Error durring note adding");
                    return;

                case -3:
                    Console.WriteLine("Some fields have wrong format");
                    return;

                default:
                    Console.WriteLine("Unexpected output. Something went wrong");
                    break;
            }
        }

        private static Patent InputPatent()
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
            if (!string.IsNullOrEmpty(inputApplicationDate))
            {
                if (!DateTime.TryParse(inputApplicationDate, out _))
                {
                    throw new InvalidCastException();
                }
            }

            Console.Write("Publication date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime inputPublicationDate))
            {
                throw new InvalidCastException();
            }

            Console.Write("Pages count: ");
            if (!int.TryParse(Console.ReadLine(), out int inputPagesCount))
            {
                throw new InvalidCastException();
            }

            Console.Write("Patent notes: ");
            string inputObjectNotes = Console.ReadLine();

            return new Patent(
                name: inputName,
                authors: inputAuthors,
                country: inputCountry,
                registrationNumber: inputRegistrationNumber,
                applicationDate: string.IsNullOrEmpty(inputApplicationDate) ? null : DateTime.Parse(inputApplicationDate),
                publicationDate: inputPublicationDate,
                pagesCount: inputPagesCount,
                objectNotes: string.IsNullOrEmpty(inputObjectNotes) ? null : inputObjectNotes
                );
        }

        private static Newspaper InputNewspaper()
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
                throw new InvalidCastException();
            }

            Console.Write("Pages count: ");
            if (!int.TryParse(Console.ReadLine(), out int inputPagesCount))
            {
                Console.WriteLine("Wrong format of page count input. No newspaper added");
                throw new InvalidCastException();
            }

            Console.Write("Newspaper notes: ");
            string inputObjectNotes = Console.ReadLine();

            Console.Write("Number: ");
            string inputNumber = Console.ReadLine();

            Console.Write("Release date: ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime inputReleaseDate))
            {
                Console.WriteLine("Wrong format of release date input. No newspaper added");
                throw new InvalidCastException();
            }

            Console.Write("ISSN: ");
            string inputISSN = Console.ReadLine();

            return new Newspaper(
                name: inputName,
                publicationPlace: inputPublicationPlace,
                publisher: inputPublisher,
                publicationDate: inputPublicationDate,
                pagesCount: inputPagesCount,
                objectNotes: string.IsNullOrEmpty(inputObjectNotes) ? null : inputObjectNotes,
                number: string.IsNullOrEmpty(inputNumber) ? null : inputNumber,
                releaseDate: inputReleaseDate,
                iSSN: string.IsNullOrEmpty(inputISSN) ? null : inputISSN
                );
        }

        private static Book InputBook()
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
                throw new InvalidCastException();
            }

            Console.Write("Pages count: ");
            if (!int.TryParse(Console.ReadLine(), out int inputPagesCount))
            {
                Console.WriteLine("Wrong format of page count input. No newspaper added");
                throw new InvalidCastException();
            }

            Console.Write("Book notes: ");
            string inputObjectNotes = Console.ReadLine();

            Console.Write("ISBN: ");
            string inputISBN = Console.ReadLine();

            return new Book(
                name: inputName,
                authors: inputAuthors,
                publicationPlace: inputPublicationPlace,
                publisher: inputPublisher,
                publicationDate: inputPublicationDate,
                pagesCount: inputPagesCount,
                objectNotes: string.IsNullOrEmpty(inputObjectNotes) ? null : inputObjectNotes,
                iSBN: string.IsNullOrEmpty(inputISBN) ? null : inputISBN
                );
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
                        try
                        {
                            Author author = InputAuthor();
                            authors.Add(author);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + ". No author added");
                        }
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

        private static void PrintNote(Note note)
        {
            if (note is null)
            {
                Console.WriteLine("Such note is note exist");
                return;
            }

            Console.WriteLine(note.ToString());
        }

        private static void PrintNotes(List<Note> notes)
        {
            if (notes.Count == 0)
            {
                Console.WriteLine("There is no notes");
            }

            for (int i = 0; i < notes.Count; i++)
            {
                Console.WriteLine(notes[i].ToString());
            }
        }

        private static void PrintNotes(List<Book> books)
        {
            if (books.Count == 0)
            {
                Console.WriteLine("There is no books");
            }

            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine(books[i].ToString());
            }
        }

        private static void PrintNotes(List<Patent> patents)
        {
            if (patents.Count == 0)
            {
                Console.WriteLine("There is no patents");
            }

            for (int i = 0; i < patents.Count; i++)
            {
                Console.WriteLine(patents[i].ToString());
            }
        }

        private static void PrintGroup(IEnumerable<IGrouping<string, Book>> gropedBooks)
        {
            foreach (var group in gropedBooks)
            {
                Console.WriteLine("Publisher: {0}\n---------------", group.Key);

                foreach (var book in group)
                {
                    Console.WriteLine(book.ToString());
                }
            }
        }

        private static void PrintGroup(IEnumerable<IGrouping<int, Note>> groupedNotes)
        {
            foreach (var group in groupedNotes)
            {
                Console.WriteLine("Year: {0}\n---------------", group.Key);

                foreach (var note in group)
                {
                    Console.WriteLine(note.ToString());
                }
            }
        }

        private static Author InputAuthor()
        {
            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            try
            {
                return new Author(firstName, lastName);
            }

            catch (Exception)
            {
                throw new Exception("Fields were filled incorrectly");
            }
        }
    }
}
