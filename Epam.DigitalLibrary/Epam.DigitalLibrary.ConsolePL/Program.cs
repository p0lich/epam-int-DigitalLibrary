using System;
using System.Text.RegularExpressions;

namespace Epam.DigitalLibrary.ConsolePL
{
    public class Program
    {
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

                _ = int.TryParse(Console.ReadLine(), out int option);

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Input note prperties:");
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
                        SearchByName();
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
                        SearchByCharset();
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
            throw new NotImplementedException();
        }

        private static void SearchByCharset()
        {
            throw new NotImplementedException();
        }

        private static void SearchBookAndPatentsByAuthor()
        {
            throw new NotImplementedException();
        }

        private static void SearchPatentsByInventor()
        {
            throw new NotImplementedException();
        }

        private static void SearchBookByAuthor()
        {
            throw new NotImplementedException();
        }

        private static void ReverseByYearSort()
        {
            throw new NotImplementedException();
        }

        private static void ForwardByYearSort()
        {
            throw new NotImplementedException();
        }

        private static void SearchByName()
        {
            throw new NotImplementedException();
        }

        private static void ShowLibrary()
        {
            throw new NotImplementedException();
        }

        private static void DeleteNote()
        {
            throw new NotImplementedException();
        }

        private static void AddNote()
        {
            throw new NotImplementedException();
        }      
    }
}
