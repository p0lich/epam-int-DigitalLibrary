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

namespace Epam.DigitalLibrary.SqlDal
{
    public class SqlDataAccessObject : IDataLayer
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SSPIConnString"].ConnectionString;
        private SqlCredential _credential;
        private SqlConnection _connection;

        private INoteDAO _bookDAO, _newspaperDAO, _patentDAO;

        public SqlDataAccessObject(SqlCredential userCredential)
        {
            _credential = userCredential;

            _bookDAO = new BookDAO(userCredential);
            _newspaperDAO = new NewspaperDAO(userCredential);
            _patentDAO = new PatentDAO(userCredential);
        }

        public int AddNote(Note note)
        {
            try
            {
                if (!note.IsUnique(GetAllNotes()))
                {
                    return ResultCodes.NoteExist;
                }

                InsertInitialNoteData(note, out Guid noteId);

                if (note is Book)
                {
                    return _bookDAO.InsertNote(noteId, note) ?
                        ResultCodes.Successfull : ResultCodes.Error;
                }

                if (note is Newspaper)
                {
                    return _newspaperDAO.InsertNote(noteId, note) ?
                        ResultCodes.Successfull : ResultCodes.Error;
                }

                return _patentDAO.InsertNote(noteId, note) ?
                    ResultCodes.Successfull : ResultCodes.Error;
            }

            catch (Exception)
            {
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

        public int UpdateNote(Guid noteId, Note updatedNote)
        {
            try
            {
                if (!updatedNote.IsUnique(GetAllNotes()))
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
                using (_connection = new SqlConnection(connectionString, _credential))
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
                throw new Exception("Error has occurred during note insert\n" + e.Message);
            }
        }

        private bool UpdateInitialNoteData(Guid mainNoteId, Note note)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
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
                throw new Exception("Error has occurred during note update\n" + e.Message);
            }
        }

        private bool DeleteInitialNoteData(Guid mainNoteId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString, _credential))
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
                throw new Exception("Cannot delete this note\n" + e.Message);
            }
        }
    }
}