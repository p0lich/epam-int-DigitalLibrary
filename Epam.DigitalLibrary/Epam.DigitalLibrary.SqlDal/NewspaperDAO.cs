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
    public class NewspaperDAO : INoteDAO
    {
        //private string connectionString = ConfigurationManager.ConnectionStrings["SSPIConnString"].ConnectionString;
        //private string connectionString = @"Data Source=DESKTOP-83KP24G;Initial Catalog=LibraryDb;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private string connectionString;
        private SqlCredential _credential;
        private SqlConnection _connection;

        public NewspaperDAO(SqlCredential userCredential)
        {
            _credential = userCredential;
        }

        public NewspaperDAO(string connString, SqlCredential credential)
        {
            connectionString = connString;
            _credential = credential;
        }

        public bool DeleteNote(Guid noteId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.CompleteDelete_Newspaper";
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
                    string stProc = "dbo.Get_AllNewspapers";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            notes.Add(new Newspaper(
                            id: (Guid)reader["Id"],
                            name: reader["Name"] as string,
                            publicationPlace: reader["PublicationPlace"] as string,
                            publisher: reader["Publisher"] as string,
                            publicationDate: (DateTime)reader["PublicationDate"],
                            pagesCount: (short)reader["PagesCount"],
                            objectNotes: reader["ObjectNotes"] as string,
                            number: reader["Number"] as string,
                            releaseDate: (DateTime)reader["ReleaseDate"],
                            iSSN: reader["ISSN"] as string,
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
                throw new Exception("Error. Unable to get newspapers from server\n" + e.Message);
            }
        }

        public bool InsertNote(Guid noteId, Note note)
        {
            try
            {
                Dictionary<string, object> newspaperData = note.ToObjectDict();

                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.Add_Newspaper";

                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_Note", noteId);
                        command.Parameters.AddWithValue("@publicationPlace", newspaperData["PublicationPlace"]);
                        command.Parameters.AddWithValue("@publisher", newspaperData["Publisher"]);
                        command.Parameters.AddWithValue("@iSSN", newspaperData["ISSN"] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@number", newspaperData["Number"] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@releaseDate", newspaperData["ReleaseDate"]);

                        SqlParameter outId = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outId);

                        _connection.Open();
                        var res = command.ExecuteScalar();
                        _connection.Close();
                    }
                }

                return ResultCodes.SuccessfullInsert;
            }

            catch (Exception)
            {
                _connection.Close();
                return ResultCodes.ErrorInsert;
            }
        }

        public bool MarkForDelete(Guid noteId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.MarkForDelete_Newspaper";
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

        public bool UpdateNote(Guid noteId, Note updatedNote)
        {
            try
            {
                Dictionary<string, object> newspaperData = updatedNote.ToObjectDict();

                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.Update_Newspaper";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", noteId);
                        command.Parameters.AddWithValue("@publicationPlace", newspaperData["PublicationPlace"]);
                        command.Parameters.AddWithValue("@publisher", newspaperData["Publisher"]);
                        command.Parameters.AddWithValue("@iSSN", newspaperData["ISSN"] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@number", newspaperData["Number"] ?? DBNull.Value);
                        command.Parameters.AddWithValue("@releaseDate", newspaperData["ReleaseDate"]);

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

        public Guid GetMainNoteId(Guid noteId)
        {
            string stProc = "dbo.Newspaper_GetNoteId";

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
                    string stProc = "dbo.Get_AllNotMarked_Newspapers";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            notes.Add(new Newspaper(
                            id: (Guid)reader["Id"],
                            name: reader["Name"] as string,
                            publicationPlace: reader["PublicationPlace"] as string,
                            publisher: reader["Publisher"] as string,
                            publicationDate: (DateTime)reader["PublicationDate"],
                            pagesCount: (short)reader["PagesCount"],
                            objectNotes: reader["ObjectNotes"] as string,
                            number: reader["Number"] as string,
                            releaseDate: (DateTime)reader["ReleaseDate"],
                            iSSN: reader["ISSN"] as string,
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
                throw new Exception("Error. Unable to get newspapers from server\n" + e.Message);
            }
        }

        public Note GetById(Guid id)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
                {
                    string stProc = "dbo.GetById_NewspaperInfo";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return new Newspaper(
                                id: (Guid)reader["ID"],
                                name: reader["Name"] as string,
                                publicationPlace: reader["PublicationPlace"] as string,
                                publisher: reader["Publisher"] as string,
                                publicationDate: (DateTime)reader["PublicationDate"],
                                pagesCount: (short)reader["PagesCount"],
                                objectNotes: reader["ObjectNotes"] as string,
                                number: reader["Number"] as string,
                                releaseDate: (DateTime)reader["ReleaseDate"],
                                iSSN: reader["ISSN"] as string,
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
