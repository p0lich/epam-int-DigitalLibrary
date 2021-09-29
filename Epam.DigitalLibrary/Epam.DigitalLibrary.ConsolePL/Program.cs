using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Logic;
using Epam.DigitalLibrary.LogicContracts;
using System;
using System.Collections.Generic;
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
                authors: new List<Author> { new Author("Ivan", "Karasev") },
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
                        GroupByYear(Console.ReadLine());
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Wrong option, try again:");
                        break;
                }
            }
        }

        private static void GroupByYear(string strYear)
        {
            if (!int.TryParse(strYear, out int year))
            {
                Console.WriteLine("Wrong input");
                return;
            }

            Dictionary<int, Note> groupedNotes = logic.GroupByYear(year);

            foreach (KeyValuePair<int, Note> note in groupedNotes)
            {
                Console.WriteLine("Year: " + note.Key);
                ShowNote(note.Value);
            }
        }

        private static void SearchByCharset(string charSet)
        {
            Dictionary<string, Book> searchResults = logic.SearchBooksByCharset(charSet);

            foreach (KeyValuePair<string, Book> book in searchResults)
            {
                Console.WriteLine("Publisher: " + book.Key);
                ShowNote(book.Value);
            }
        }

        private static void SearchBookAndPatentsByAuthor()
        {
            List<Note> notes = logic.SearchBooksAndPatensByAuthor(new Author(Console.ReadLine(), Console.ReadLine()));

            for (int i = 0; i < notes.Count; i++)
            {
                ShowNote(notes[i]);
            }
        }

        private static void SearchPatentsByInventor()
        {
            List<Patent> patents = logic.SearchPatentByInventor(new Author(Console.ReadLine(), Console.ReadLine()));

            for (int i = 0; i < patents.Count; i++)
            {
                ShowNote(patents[i]);
            }
        }

        private static void SearchBookByAuthor()
        {
            List<Book> books = logic.SearchBooksByAuthor(new Author(Console.ReadLine(), Console.ReadLine()));

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
            ShowNote(logic.SearchByName(name));
        }

        private static void ShowLibrary()
        {
            List<Note> notes = logic.GetCatalog();

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
                logic.AddNote(new Patent(
                name: Console.ReadLine(),
                authors: AddAuthors(),
                country: Console.ReadLine(),
                registrationNumber: Console.ReadLine(),
                applicationDate: DateTime.Parse(Console.ReadLine()),
                publicationDate: DateTime.Parse(Console.ReadLine()),
                pagesCount: int.Parse(Console.ReadLine()),
                objectNotes: Console.ReadLine()
                ));
                Console.WriteLine("Book was added");
            }
            catch (Exception)
            {
                Console.WriteLine("Error was occured during book creation");
                return;
            }
        }

        private static void AddNewspaper()
        {
            Console.WriteLine("Leave unnecessary field empty if you don't want fill them");

            try
            {
                logic.AddNote(new Newspaper(
                name: Console.ReadLine(),
                publicationPlace: Console.ReadLine(),
                publisher: Console.ReadLine(),
                publicationDate: DateTime.Parse(Console.ReadLine()),
                pagesCount: int.Parse(Console.ReadLine()),
                objectNotes: Console.ReadLine(),
                number: Console.ReadLine(),
                releaseDate: DateTime.Parse(Console.ReadLine()),
                iSSN: Console.ReadLine()
                ));
                Console.WriteLine("Book was added");
            }
            catch (Exception)
            {
                Console.WriteLine("Error was occured during book creation");
                return;
            }
        }

        private static void AddBook()
        {
            Console.WriteLine("Leave unnecessary field empty if you don't want fill them");

            try
            {
                logic.AddNote(new Book(
                name: Console.ReadLine(),
                authors: AddAuthors(),
                publicationPlace: Console.ReadLine(),
                publisher: Console.ReadLine(),
                publicationDate: DateTime.Parse(Console.ReadLine()),
                pagesCount: int.Parse(Console.ReadLine()),
                objectNotes: Console.ReadLine(),
                iSBN: Console.ReadLine()
                ));
                Console.WriteLine("Book was added");
            }
            catch (Exception)
            {
                Console.WriteLine("Error was occured during book creation");
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
                    $"Book notes: {book.ObjectNotes};\n" +
                    $"ISBN: {book.ISBN};\n"
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
                    $"Newspaper notes: {newspaper.ObjectNotes};\n" +
                    $"Number: {newspaper.Number};\n" +
                    $"Release date: {newspaper.ReleaseDate};\n" +
                    $"ISBN: {newspaper.ISSN};\n"
                    );

                return;
            }

            Patent patent = note as Patent;
            StringBuilder inventors = new StringBuilder();

            for (int i = 0; i < patent.Authors.Count; i++)
            {
                inventors.Append(patent.Authors[i].ToString() + ",\n");
            }

            Console.WriteLine(
                $"Name: {patent.Name};\n" +
                $"Inventors:\n{inventors};\n" +
                $"Country: {patent.Country};\n" +
                $"Registration number: {patent.RegistrationNumber};\n" +
                $"Application date: {patent.ApplicationDate};\n" +
                $"Publication date: {patent.PublicationDate};\n" +
                $"Page count: {patent.PagesCount};\n" +
                $"Patent notes: {patent.ObjectNotes};\n"
                );

            return;
        }
    }
}
