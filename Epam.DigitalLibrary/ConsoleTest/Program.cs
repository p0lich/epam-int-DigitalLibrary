using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Logic;
using Epam.DigitalLibrary.LogicContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Security;

namespace ConsoleTest
{
    public class Program
    {
        private static readonly INoteLogic logic;
        private static IUserRightsProvider userRightsLogic;

        static void Main(string[] args)
        {
            #region PARAMETERS_FOR_ADD
            Book uniqueBook1 = new Book(
                name: "bookADO15",
                authors: new List<Author> { new Author("Ivan", "Karasev"), new Author("Aleksei", "Ivanov") },
                publicationPlace: "Saratov",
                publisher: "bookerADO1",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 50,
                objectNotes: "aoaoaoaoa",
                iSBN: null
                );

            Newspaper uniqueNewspaper1 = new Newspaper(
                name: "newspaperADO2",
                publicationPlace: "Samara",
                publisher: "samaraNewsADO1",
                publicationDate: new DateTime(1900, 01, 01),
                pagesCount: 5,
                objectNotes: "hi",
                number: "123",
                releaseDate: new DateTime(1900, 01, 01),
                iSSN: "ISSN1284-5678"
                );

            Patent uniquePatent1 = new Patent(
                name: "patentADO2",
                authors: new List<Author> { new Author("Artem", "Artemov") },
                country: "Russia",
                registrationNumber: "11123",
                applicationDate: new DateTime(1890, 01, 01),
                publicationDate: new DateTime(1891, 01, 01),
                pagesCount: 20,
                objectNotes: "daADO1"
                );
            #endregion

            Dictionary<string, object> ub1 = uniqueBook1.ToObjectDict();

            Console.WriteLine(ub1["ID"]);

            logic.AddNote(uniqueBook1);
            //logic.AddNote(uniqueNewspaper1);
            //logic.AddNote(uniquePatent1);

            //List<Note> notes = logic.GetCatalog();

            //foreach (var note in notes)
            //{
            //    Console.WriteLine(note);
            //}

            //DataTable table = new DataTable();

            //using (_connection = new SqlConnection(connectionString))
            //{
            //    string stProc = "dbo.Get_AllBooks";
            //    using (SqlCommand command = new SqlCommand(stProc, _connection))
            //    {
            //        command.CommandType = CommandType.StoredProcedure;

            //        _connection.Open();
            //        var reader = command.ExecuteReader();
            //        table.Load(reader);
            //    }
            //}

            //foreach (DataRow row in table.Rows)
            //{
            //    Console.WriteLine(row["Id"]);
            //    Console.WriteLine(row["Name"]);

            //    Console.WriteLine("\n<================>");
            //}
        }

        private static bool Authorize()
        {
            string userLogin;
            SecureString password;

            while (true)
            {
                userLogin = InputLogin();
                password = InputPassword();

                IUserRightsProvider rightsProvider = new UserLogic(userLogin, password);

                if (rightsProvider.IsCredentialRight())
                {
                    userRightsLogic = rightsProvider;
                    return true;
                }

                Console.WriteLine("Cannot find user with such login/password. Try again");
            }
        }

        private static string InputLogin()
        {
            Console.WriteLine("Input login:");
            return Console.ReadLine();
        }

        private static SecureString InputPassword()
        {
            SecureString password = new SecureString();
            ConsoleKeyInfo key;

            Console.WriteLine("Input password:");

            do
            {
                key = Console.ReadKey(intercept: true);
                if (key.Key != ConsoleKey.Enter)
                {
                    password.AppendChar(key.KeyChar);
                }

            } while (key.Key != ConsoleKey.Enter);

            password.MakeReadOnly();

            return password;
        }
    }
}
