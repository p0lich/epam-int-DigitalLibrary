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
        private static INoteLogic logic = new LibraryLogic();

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

                int.TryParse(Console.ReadLine(), out int option);

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Input note properties:");
                        AddNote();
                        break;

                    case 2:
                        Console.WriteLine("");
                        DeleteNote();
                        break;

                    case 3:
                        ShowLibrary();
                        break;

                    case 4:
                        Console.WriteLine("Input note name:");
                        SearchByName(Console.ReadLine());
                        break;

                    case 5:
                        ForwardByYearSort();
                        break;

                    case 6:
                        ReverseByYearSort();
                        break;

                    case 7:
                        Console.WriteLine("Input author full name:");
                        SearchBookByAuthor();
                        break;

                    case 8:
                        Console.WriteLine("Input inventor full name:");
                        SearchPatentsByInventor();
                        break;

                    case 9:
                        Console.WriteLine("Input author full name:");
                        SearchBookAndPatentsByAuthor();
                        break;

                    case 10:
                        Console.WriteLine("Input charset:");
                        SearchByCharset(Console.ReadLine());
                        break;

                    case 11:
                        Console.WriteLine("Input year:");
                        GroupByYear();
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Wrong option, try again:");
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
                    ShowNote(note);
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
                    ShowNote(book);
                }
            }
        }

        private static void SearchBookAndPatentsByAuthor()
        {
            List<Note> notes = logic.SearchBooksAndPatensByAuthor(new Author(Console.ReadLine(), Console.ReadLine()));

            if (notes.Count == 0)
            {
                Console.WriteLine("Can't find this author notes");
                return;
            }

            for (int i = 0; i < notes.Count; i++)
            {
                ShowNote(notes[i]);
            }
        }

        private static void SearchPatentsByInventor()
        {
            List<Patent> patents = logic.SearchPatentByInventor(new Author(Console.ReadLine(), Console.ReadLine()));

            if (patents.Count == 0)
            {
                Console.WriteLine("Can't find this inventor patents");
                return;
            }

            for (int i = 0; i < patents.Count; i++)
            {
                ShowNote(patents[i]);
            }
        }

        private static void SearchBookByAuthor()
        {
            List<Book> books = logic.SearchBooksByAuthor(new Author(Console.ReadLine(), Console.ReadLine()));

            if (books.Count == 0)
            {
                Console.WriteLine("Can't find this author books");
                return;
            }

            for (int i = 0; i < books.Count; i++)
            {
                ShowNote(books[i]);
            }
        }

        private static void ReverseByYearSort()
        {
            List<Note> sortedLib = logic.SortInReverseOrder();

            for (int i = 0; i < sortedLib.Count; i++)
            {
                ShowNote(sortedLib[i]);
            }
        }

        private static void ForwardByYearSort()
        {
            List<Note> sortedLib = logic.SortInOrder();

            for (int i = 0; i < sortedLib.Count; i++)
            {
                ShowNote(sortedLib[i]);
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

            ShowNote(note);
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
                ShowNote(notes[i]);
            }
        }

        // Currently remove first note in catalog
        private static void DeleteNote()
        {
            if (logic.GetCatalog().Count > 0)
            {
                logic.RemoveNote();
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
                int.TryParse(Console.ReadLine(), out int option);

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

            try
            {
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
                DateTime inputPublicationDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Pages count: ");
                int inputPagesCount = int.Parse(Console.ReadLine());

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
                }

                if (addResult == -1)
                {
                    Console.WriteLine("Same patent already exist");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error was occured during patent adding");
                return;
            }
        }

        private static void AddNewspaper()
        {
            try
            {
                Console.WriteLine("Leave unnecessary field empty if you don't want fill them");

                Console.Write("Newspaper name: ");
                string inputName = Console.ReadLine();

                Console.Write("Publication place: ");
                string inputPublicationPlace = Console.ReadLine();

                Console.Write("Publisher: ");
                string inputPublisher = Console.ReadLine();

                Console.Write("Publication date: ");
                DateTime inputPublicationDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Pages count: ");
                int inputPagesCount = int.Parse(Console.ReadLine());

                Console.Write("Newspaper notes: ");
                string inputObjectNotes = Console.ReadLine();

                Console.Write("Number: ");
                string inputNumber = Console.ReadLine();

                Console.Write("Release date: ");
                DateTime inputReleaseDate = DateTime.Parse(Console.ReadLine());

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
                }

                if (addResult == -1)
                {
                    Console.WriteLine("Same newspaper already exist");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error was occured during newspaper adding");
                return;
            }
        }

        private static void AddBook()
        {
            Console.WriteLine("Leave unnecessary field empty if you don't want fill them");

            try
            {
                Console.Write("Book name: ");
                string inputName = Console.ReadLine();

                Console.WriteLine("Authors:");
                List<Author> inputAuthors = AddAuthors();

                Console.Write("Publication place: ");
                string inputPublicationPlace = Console.ReadLine();

                Console.Write("Publisher: ");
                string inputPublisher = Console.ReadLine();

                Console.Write("Publication date: ");
                DateTime inputPublicationDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Pages count: ");
                int inputPagesCount = int.Parse(Console.ReadLine());

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
                }

                if (addResult == -1)
                {
                    Console.WriteLine("Same book already exist");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error was occured during book adding");
                return;
            }
        }

        private static List<Author> AddAuthors()
        {
            List<Author> authors = new List<Author>();

            Console.WriteLine(
                "1 - add author\n" +
                "2 - finish");

            while (true)
            {
                int.TryParse(Console.ReadLine(), out int option);

                switch (option)
                {
                    case 1:
                        authors.Add(new Author(Console.ReadLine(), Console.ReadLine()));
                        break;

                    case 2:
                        if (authors.Count > 0)
                        {
                            return authors;
                        }
                        break;

                    default:
                        Console.WriteLine("Wrong input. Try again.");
                        break;
                }
            }
        }

        private static void ShowNote(Note note)
        {
            if (note is Book)
            {
                Book book = note as Book;
                StringBuilder authors = new StringBuilder();

                for (int i = 0; i < book.Authors.Count; i++)
                {
                    authors.Append(book.Authors[i].ToString() + "\n");
                }

                Console.WriteLine(
                    $"Name: {book.Name};\n" +
                    $"Authors:\n{authors};\n" +
                    $"Publication place: {book.PublicationPlace};\n" +
                    $"Publisher: {book.Publisher};\n" +
                    $"Publication date: {book.PublicationDate};\n" +
                    $"Page count: {book.PagesCount};\n" +
                    $"Book notes: {book.ObjectNotes ?? "N/A"};\n" +
                    $"ISBN: {book.ISBN ?? "N/A"};\n"
                    );

                return;
            }

            if (note is Newspaper)
            {
                Newspaper newspaper = note as Newspaper;

                Console.WriteLine(
                    $"Name: {newspaper.Name};\n" +
                    $"Publication place: {newspaper.PublicationPlace};\n" +
                    $"Publisher: {newspaper.Publisher};\n" +
                    $"Publication date: {newspaper.PublicationDate};\n" +
                    $"Page count: {newspaper.PagesCount};\n" +
                    $"Newspaper notes: {newspaper.ObjectNotes ?? "N/A"};\n" +
                    $"Number: {newspaper.Number ?? "N/A"};\n" +
                    $"Release date: {newspaper.ReleaseDate};\n" +
                    $"ISBN: {newspaper.ISSN ?? "N/A"};\n"
                    );

                return;
            }

            Patent patent = note as Patent;
            StringBuilder inventors = new StringBuilder();

            for (int i = 0; i < patent.Authors.Count; i++)
            {
                inventors.Append(patent.Authors[i].ToString() + "\n");
            }

            Console.WriteLine(
                $"Name: {patent.Name};\n" +
                $"Inventors:\n{inventors};\n" +
                $"Country: {patent.Country};\n" +
                $"Registration number: {patent.RegistrationNumber};\n" +
                $"Application date: {(patent.ApplicationDate is null ? "N/A" : patent.ApplicationDate.ToString())};\n" +
                $"Publication date: {patent.PublicationDate};\n" +
                $"Page count: {patent.PagesCount};\n" +
                $"Patent notes: {patent.ObjectNotes ?? "N/A"};\n"
                );

            return;
        }
    }
}
