using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.Entities;
using System.Configuration;
using Epam.DigitalLibrary.DalConfig;
using Epam.DigitalLibrary.CustomExeptions;

namespace Epam.DigitalLibrary.SqlDal
{
    public class SqlDataAccessObject : IDataLayer
    {
        private string connectionString;
        private SqlConnection _connection;

        private INoteDAO _bookDAO, _newspaperDAO, _patentDAO;

        public SqlDataAccessObject(string connString)
        {
            connectionString = connString;

            _bookDAO = new BookDAO(connString);
            _newspaperDAO = new NewspaperDAO(connString);
            _patentDAO = new PatentDAO(connString);
        }

        public int AddNote(Note note, out Guid noteId)
        {
            try
            {
                if (!note.IsUnique(GetAllNotes(), note.ID))
                {
                    noteId = new Guid();
                    return ResultCodes.NoteExist;
                }

                InsertInitialNoteData(note, out Guid rootNoteId);

                if (note is Book)
                {
                    return _bookDAO.InsertNote(rootNoteId, note, out noteId) ?
                        ResultCodes.Successfull : ResultCodes.Error;
                }

                if (note is Newspaper)
                {
                    return _newspaperDAO.InsertNote(rootNoteId, note, out noteId) ?
                        ResultCodes.Successfull : ResultCodes.Error;
                }

                return _patentDAO.InsertNote(rootNoteId, note, out noteId) ?
                    ResultCodes.Successfull : ResultCodes.Error;
            }

            catch (Exception)
            {
                noteId = new Guid();
                return ResultCodes.Error;
            }
        }

        public List<Note> GetAllNotes()
        {
            List<Note> notes = new List<Note>();

            notes.AddRange(_bookDAO.GetNotes());
            notes.AddRange(_newspaperDAO.GetNotes());
            notes.AddRange(_patentDAO.GetNotes());

            return notes;
        }

        public List<Note> GetAllUnmarkedNotes()
        {
            List<Note> notes = new List<Note>();

            notes.AddRange(_bookDAO.GetUnmarkedNotes());
            notes.AddRange(_newspaperDAO.GetUnmarkedNotes());
            notes.AddRange(_patentDAO.GetUnmarkedNotes());

            return notes;
        }

        public bool RemoveNote()
        {
            return false;
        }

        public bool RemoveNote(Note note)
        {
            Guid mainNoteId;

            try
            {
                if (note is Book)
                {
                    mainNoteId = _bookDAO.GetMainNoteId(note.ID);
                    _bookDAO.DeleteNote(note.ID);
                    return DeleteInitialNoteData(mainNoteId);
                }

                if (note is Newspaper)
                {
                    mainNoteId = _newspaperDAO.GetMainNoteId(note.ID);
                    _newspaperDAO.DeleteNote(note.ID);
                    return DeleteInitialNoteData(mainNoteId);
                }

                mainNoteId = _patentDAO.GetMainNoteId(note.ID);
                _patentDAO.DeleteNote(note.ID);
                return DeleteInitialNoteData(mainNoteId);
            }

            catch (Exception e)
            {
                return false;
            }
        }

        public bool MarkNote(Note note)
        {
            if (note is Book)
            {
                return _bookDAO.MarkForDelete(note.ID);
            }

            if (note is Newspaper)
            {
                return _newspaperDAO.MarkForDelete(note.ID);
            }

            return _patentDAO.MarkForDelete(note.ID);
        }

        public Note GetById(Guid id)
        {
            return GetAllNotes().FirstOrDefault(n => n.ID == id);
        }

        public Book GetBookById(Guid id)
        {
            return _bookDAO.GetById(id) as Book;
        }

        public Newspaper GetNewspaperById(Guid id)
        {
            return _newspaperDAO.GetById(id) as Newspaper;
        }

        public Patent GetPatentById(Guid id)
        {
            return _patentDAO.GetById(id) as Patent;
        }

        public Author GetAuthor(Guid id)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Author_GetById";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return new Author(
                                id: (Guid)reader["Id"],
                                firstName: reader["FirstName"] as string,
                                lastName: reader["LastName"] as string
                                );
                        }

                        return null;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public List<Author> GetAvailableAuthors()
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Get_AvailableAuthors";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        List<Author> authors = new List<Author>();
                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            authors.Add(new Author(
                                id: (Guid)reader["Id"],
                                firstName: reader["FirstName"] as string,
                                lastName: reader["LastName"] as string
                                ));
                        }

                        return authors;
                    }
                }
            }

            catch (Exception e)
            {
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public int UpdateAuthor(Guid id, Author updatedAuthor)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Author_Update";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@firstName", updatedAuthor.FirstName);
                        command.Parameters.AddWithValue("@lastName", updatedAuthor.LastName);

                        _connection.Open();
                        command.ExecuteScalar();

                        return ResultCodes.Successfull;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public int UpdateNote(Guid noteId, Note updatedNote)
        {
            try
            {
                if (!updatedNote.IsUnique(GetAllNotes(), noteId))
                {
                    return ResultCodes.NoteExist;
                }

                Guid mainNoteId;

                if (updatedNote is Book)
                {
                    mainNoteId = _bookDAO.GetMainNoteId(noteId);
                    UpdateInitialNoteData(mainNoteId, updatedNote);

                    return _bookDAO.UpdateNote(noteId, updatedNote) ? 
                        ResultCodes.Successfull : ResultCodes.Error;
                }

                if (updatedNote is Newspaper)
                {
                    mainNoteId = _newspaperDAO.GetMainNoteId(noteId);
                    UpdateInitialNoteData(mainNoteId, updatedNote);

                    return _newspaperDAO.UpdateNote(noteId, updatedNote) ?
                        ResultCodes.Successfull : ResultCodes.Error;
                }

                mainNoteId = _patentDAO.GetMainNoteId(noteId);
                UpdateInitialNoteData(mainNoteId, updatedNote);

                return _patentDAO.UpdateNote(noteId, updatedNote) ?
                    ResultCodes.Successfull : ResultCodes.Error;
            }

            catch (Exception)
            {
                return -2;
            }
        }

        private bool InsertInitialNoteData(Note note, out Guid noteId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Add_Note";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@name", note.Name);
                        command.Parameters.AddWithValue("@publicationDate", note.PublicationDate);
                        command.Parameters.AddWithValue("@pagesCount", note.PagesCount);
                        command.Parameters.AddWithValue("@objectNotes", string.IsNullOrEmpty(note.ObjectNotes) ? DBNull.Value : note.ObjectNotes);

                        SqlParameter outId = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outId);

                        _connection.Open();
                        command.ExecuteScalar();
                        _connection.Close();

                        if (!Guid.TryParse(outId.Value.ToString(), out noteId))
                        {
                            throw new InvalidCastException();
                        }

                        return ResultCodes.SuccessfullInsert;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        private bool UpdateInitialNoteData(Guid mainNoteId, Note note)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Update_Note";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", mainNoteId);
                        command.Parameters.AddWithValue("@name", note.Name);
                        command.Parameters.AddWithValue("@publicationDate", note.PublicationDate);
                        command.Parameters.AddWithValue("@pagesCount", note.PagesCount);
                        command.Parameters.AddWithValue("@objectNotes", string.IsNullOrEmpty(note.ObjectNotes) ? DBNull.Value : note.ObjectNotes);

                        _connection.Open();
                        command.ExecuteScalar();
                    }
                }

                return ResultCodes.SuccessfullUpdate;
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        private bool DeleteInitialNoteData(Guid mainNoteId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Delete_Note";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", mainNoteId);

                        _connection.Open();
                        command.ExecuteScalar();
                    }
                }

                return ResultCodes.SuccessfullDelete;
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public int AddAuthor(Author author, out Guid id)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Add_Author";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@firstName", author.FirstName);
                        command.Parameters.AddWithValue("@lastName", author.LastName);

                        SqlParameter outId = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outId);

                        _connection.Open();
                        command.ExecuteScalar();
                        _connection.Close();

                        if (!Guid.TryParse(outId.Value.ToString(), out id))
                        {
                            throw new ArgumentException();
                        }

                        return ResultCodes.Successfull;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                id = new Guid();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }
    }
}