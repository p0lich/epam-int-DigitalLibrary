using Epam.DigitalLibrary.CustomExeptions;
using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.Entities.Models.NewspaperModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.SqlDal
{
    public class NewspaperReleaseDAO : INewspaperReleaseDAO
    {
        private string _connectionString;
        private SqlConnection _connection;

        public NewspaperReleaseDAO(string connString)
        {
            _connectionString = connString;
        }

        public int AddNewspaperRelease(NewspaperInputViewModel newspaperModel, out Guid newspaperReleaseId)
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.NewspaperRelease_Add";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter outId = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outId);
                        command.Parameters.AddWithValue("@name", newspaperModel.Name);
                        command.Parameters.AddWithValue("@publisher", newspaperModel.Publisher);
                        command.Parameters.AddWithValue("@publicationPlace", newspaperModel.PublicationPlace);
                        command.Parameters.AddWithValue("@publicationDate", newspaperModel.PublicationDate);

                        command.Parameters.AddWithValue("@objectNotes",
                            string.IsNullOrEmpty(newspaperModel.ObjectNotes) ?
                            DBNull.Value : newspaperModel.ObjectNotes);

                        command.Parameters.AddWithValue("@ISSN",
                            string.IsNullOrEmpty(newspaperModel.ISSN) ?
                            DBNull.Value : newspaperModel.ISSN);

                        _connection.Open();
                        command.ExecuteScalar();
                        _connection.Close();

                        if (!Guid.TryParse(outId.Value.ToString(), out newspaperReleaseId))
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
                newspaperReleaseId = new Guid();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public List<NewspaperDetailsViewModel> GetAllNewspapers()
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.NewspaperRelease_GetAll";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        List<NewspaperDetailsViewModel> newspapersDetails = new List<NewspaperDetailsViewModel>();

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            newspapersDetails.Add(new NewspaperDetailsViewModel()
                            {
                                Id = (Guid)reader["Id"],
                                Name = reader["Name"] as string,
                                Publisher = reader["Publisher"] as string,
                                PublicationPlace = reader["PublicationPlace"] as string,
                                PublicationDate = (DateTime)reader["PublicationDate"],
                                ObjectNotes = reader["ObjectNotes"] as string,
                                ISSN = reader["ISSN"] as string,
                            });
                        }

                        return newspapersDetails;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public List<Newspaper> GetAllNewspaperReleases(Guid newspaperId)
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.Newspaper_GetReleases";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_Newspaper", newspaperId);

                        List<Newspaper> newspapers = new List<Newspaper>();

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            newspapers.Add(new Newspaper(
                            id: (Guid)reader["Id"],
                            releaseId: reader["Id_Release"] as Guid?,
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

                        return newspapers;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public NewspaperDetailsViewModel GetNewspaperRelease(Guid id)
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.NewspaperRelease_GetById";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return new NewspaperDetailsViewModel()
                            {
                                Id = (Guid)reader["Id"],
                                Name = reader["Name"] as string,
                                Publisher = reader["Publisher"] as string,
                                PublicationPlace = reader["PublicationPlace"] as string,
                                PublicationDate = (DateTime)reader["PublicationDate"],
                                ObjectNotes = reader["ObjectNotes"] as string,
                                ISSN = reader["ISSN"] as string,
                            };
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

        public bool MarkForDeleteNewspaperRelease(Guid id)
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.NewspaperRelease_MarkForDelete";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);

                        _connection.Open();
                        command.ExecuteScalar();

                        return true;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public int UpdateNewspaperRelease(Guid id, NewspaperInputViewModel newspaperModel)
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.NewspaperRelease_Update";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@name", newspaperModel.Name);
                        command.Parameters.AddWithValue("@publisher", newspaperModel.Publisher);
                        command.Parameters.AddWithValue("@publicationPlace", newspaperModel.PublicationPlace);
                        command.Parameters.AddWithValue("@publicationDate", newspaperModel.PublicationDate);

                        command.Parameters.AddWithValue("@objectNotes",
                            string.IsNullOrEmpty(newspaperModel.ObjectNotes) ?
                            DBNull.Value : newspaperModel.ObjectNotes);

                        command.Parameters.AddWithValue("@ISSN",
                            string.IsNullOrEmpty(newspaperModel.ISSN) ?
                            DBNull.Value : newspaperModel.ISSN);

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

        public bool SetRelease(Guid newspaperId, Guid releaseId)
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.Newspaper_SetRelease";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_Newspaper", newspaperId);
                        command.Parameters.AddWithValue("@id_Release", releaseId);

                        _connection.Open();
                        command.ExecuteScalar();

                        return true;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public List<Newspaper> GetReleaseNewspapers(Guid newspaperReleaseId)
        {
            try
            {
                using (_connection = new SqlConnection(_connectionString))
                {
                    string stProc = "dbo.NewspaperRelease_GetReleaseNewspapers";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_Release", newspaperReleaseId);

                        List<Newspaper> newspapers = new List<Newspaper>();

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            newspapers.Add(new Newspaper(
                                id: (Guid)reader["Id"],
                                releaseId: (Guid)reader["Id_Release"],
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

                        return newspapers;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }
    }
}
