using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.SqlDal
{
    public class BookDAO : INoteDAO
    {
        // private string connectionString = ConfigurationManager.ConnectionStrings["SSPIConnString"].ConnectionString;
        private string connectionString = @"Data Source=DESKTOP-83KP24G;Initial Catalog=LibraryDb;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlCredential _credential;
        private SqlConnection _connection;

        public BookDAO(SqlCredential userCredential)
        {
            _credential = userCredential;
        }

        public bool DeleteNote(Guid noteId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.CompleteDelete_Book";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", noteId);

                        _connection.Open();
                        command.ExecuteScalar();
                    }
                }

                return ResultCodes.SuccessfullDelete;
            }

            catch (Exception e)
            {
                _connection.Close();
                return ResultCodes.ErrorDelete;
            }
        }

        public bool MarkForDelete(Guid noteId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.MarkForDelete_Book";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", noteId);

                        _connection.Open();
                        command.ExecuteScalar();
                    }
                }

                return ResultCodes.SuccessfullDelete;
            }

            catch (Exception e)
            {
                _connection.Close();
                return ResultCodes.ErrorDelete;
            }
        }

        public List<Note> GetNotes()
        {
            List<Note> notes = new List<Note>();

            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.Get_AllBooks";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            notes.Add(new Book(
                            id: (Guid)reader["Id"],
                            name: reader["Name"] as string,
                            authors: SqlObjectToAuthors(reader["Authors"]),
                            publicationPlace: reader["PublicationPlace"] as string,
                            publisher: reader["Publisher"] as string,
                            publicationDate: (DateTime)reader["PublicationDate"],
                            pagesCount: (short)reader["PagesCount"],
                            objectNotes: reader["ObjectNotes"] as string,
                            iSBN: reader["ISBN"] as string,
                            isDeleted: (bool)reader["IsDeleted"]
                            ));
                        }
                    }
                }

                return notes;
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new Exception("Unable to get books from server\n" + e.Message);
            }
        }

        public bool InsertNote(Guid noteId, Note note)
        {
            try
            {
                Guid bookId;
                Dictionary<string, object> bookData = note.ToObjectDict();

                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.Add_Book";

                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_Note", noteId);
                        command.Parameters.AddWithValue("@publicationPlace", bookData["PublicationPlace"]);
                        command.Parameters.AddWithValue("@publisher", bookData["Publisher"]);
                        command.Parameters.AddWithValue("@iSBN", bookData["ISBN"] ?? DBNull.Value);

                        SqlParameter outId = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outId);

                        _connection.Open();
                        var res = command.ExecuteScalar();
                        _connection.Close();

                        if (!Guid.TryParse(outId.Value.ToString(), out bookId))
                        {
                            throw new InvalidCastException();
                        }
                    }
                }

                SetAuthorsToNote(bookId, bookData["Authors"] as List<Author>);

                return ResultCodes.SuccessfullInsert;
            }

            catch (Exception e)
            {
                _connection.Close();
                return ResultCodes.ErrorInsert;
            }
        }

        public bool UpdateNote(Guid noteId, Note updatedNote)
        {
            try
            {
                Dictionary<string, object> bookData = updatedNote.ToObjectDict();

                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.Update_Book";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", noteId);
                        command.Parameters.AddWithValue("@publicationPlace", bookData["PublicationPlace"]);
                        command.Parameters.AddWithValue("@publisher", bookData["Publisher"]);
                        command.Parameters.AddWithValue("@iSBN", bookData["ISBN"] ?? DBNull.Value);

                        _connection.Open();
                        var result = command.ExecuteScalar();
                    }
                }

                return ResultCodes.SuccessfullUpdate;
            }

            catch (Exception)
            {
                _connection.Close();
                return ResultCodes.ErrorUpdate;
            }
        }

        private List<Author> SqlObjectToAuthors(object objAuthors)
        {
            string[] separatedAuthors = objAuthors.ToString().Split('|');

            List<Author> authors = new List<Author>();

            foreach (var stringAuthor in separatedAuthors)
            {
                string[] authorProperties = stringAuthor.Split(',');

                authors.Add(new Author(
                    firstName: authorProperties[0],
                    lastName: authorProperties[1]
                    ));
            }

            return authors;
        }

        private bool SetAuthorsToNote(Guid noteId, List<Author> authors)
        {
            string stAddAuthorProc = "dbo.Add_Author";
            string stSetAuthorProc = "dbo.Book_Set_Author";

            Guid authorId;

            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    _connection.Open();

                    foreach (var author in authors)
                    {
                        using (SqlCommand addAtuhorCommand = new SqlCommand(stAddAuthorProc, _connection),
                            setAuthorCommand = new SqlCommand(stSetAuthorProc, _connection))
                        {
                            addAtuhorCommand.CommandType = CommandType.StoredProcedure;
                            setAuthorCommand.CommandType = CommandType.StoredProcedure;

                            addAtuhorCommand.Parameters.AddWithValue("@firstName", author.FirstName);
                            addAtuhorCommand.Parameters.AddWithValue("@lastName", author.LastName);

                            SqlParameter outId = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                            {
                                Direction = ParameterDirection.Output
                            };
                            addAtuhorCommand.Parameters.Add(outId);

                            addAtuhorCommand.ExecuteScalar();

                            if (!Guid.TryParse(outId.Value.ToString(), out authorId))
                            {
                                _connection.Close();
                                throw new InvalidCastException();
                            }

                            setAuthorCommand.Parameters.AddWithValue("@id_Author", authorId);
                            setAuthorCommand.Parameters.AddWithValue("@id_Note", noteId);

                            setAuthorCommand.ExecuteScalar();
                        }
                    }
                }

                return true;
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new Exception("Error has occurred during author set to Note\n" + e.Message);
            }
        }

        public Guid GetMainNoteId(Guid noteId)
        {
            string stProc = "dbo.Book_GetNoteId";

            Guid mainNoteId;

            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", noteId);

                        SqlParameter outNoteId = new SqlParameter("@id_Note", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outNoteId);

                        _connection.Open();
                        var result = command.ExecuteScalar();
                        _connection.Close();

                        if (!Guid.TryParse(outNoteId.Value.ToString(), out mainNoteId))
                        {
                            throw new InvalidCastException();
                        }
                    }
                }

                return mainNoteId;
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new Exception("Can't find note with such id\n" + e.Message);
            }
        }

        public List<Note> GetUnmarkedNotes()
        {
            List<Note> notes = new List<Note>();

            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.Get_AllNotMarked_Books";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            notes.Add(new Book(
                            id: (Guid)reader["Id"],
                            name: reader["Name"] as string,
                            authors: SqlObjectToAuthors(reader["Authors"]),
                            publicationPlace: reader["PublicationPlace"] as string,
                            publisher: reader["Publisher"] as string,
                            publicationDate: (DateTime)reader["PublicationDate"],
                            pagesCount: (short)reader["PagesCount"],
                            objectNotes: reader["ObjectNotes"] as string,
                            iSBN: reader["ISBN"] as string,
                            isDeleted: (bool)reader["IsDeleted"]
                            ));
                        }
                    }
                }

                return notes;
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new Exception("Error. Unable to get books from server\n" + e.Message);
            }
        }

        public Note GetById(Guid id)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.GetById_BookInfo";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return new Book(
                                id: (Guid)reader["ID"],
                                name: reader["Name"] as string,
                                authors: SqlObjectToAuthors(reader["Authors"]),
                                publicationPlace: reader["PublicationPlace"] as string,
                                publisher: reader["Publisher"] as string,
                                publicationDate: (DateTime)reader["PublicationDate"],
                                pagesCount: (short)reader["PagesCount"],
                                objectNotes: reader["ObjectNotes"] as string,
                                iSBN: reader["ISBN"] as string,
                                isDeleted: (bool)reader["IsDeleted"]
                                );
                        }

                        return null;
                    }
                }
            }

            catch (Exception e)
            {
                throw;
            }
        }
    }
}