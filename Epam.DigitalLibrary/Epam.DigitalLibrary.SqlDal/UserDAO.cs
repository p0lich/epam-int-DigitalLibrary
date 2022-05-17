using Epam.DigitalLibrary.DalContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.CustomExeptions;

namespace Epam.DigitalLibrary.SqlDal
{
    public class UserDAO : IUserDAO
    {
        private string connectionString;
        private SqlConnection _connection;

        public UserDAO(string connString)
        {
            connectionString = connString;
        }

        public User GetUser(Guid id)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.User_GetById";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return new User(
                                id: (Guid)reader["Id"],
                                login: reader["Login"] as string,
                                password: reader["Password"] as string
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

        public User GetUser(string login)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.User_GetByLogin";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@login", login);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            return new User(
                                id: (Guid)reader["Id"],
                                login: reader["Login"] as string,
                                password: reader["Password"] as string
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

        public List<string> GetUserRoles(Guid userId)
        {
            try
            {
                List<string> roles = new List<string>();

                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Get_UserRoles";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_User", userId);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            roles.Add(reader["RoleName"] as string);
                        }
                    }
                }

                return roles;
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public List<User> GetUsers()
        {
            try
            {
                List<User> users = new List<User>();

                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.User_GetAll";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            users.Add(new User(
                                id: (Guid)reader["Id"],
                                login: reader["Login"] as string,
                                password: reader["Password"] as string
                                ));
                        }

                        return users;
                    }
                }
            }

            catch (Exception e)
            {
                _connection.Close();
                throw new DataAccessException(e.Message, e.InnerException);
            }
        }

        public bool IsConnectionAllowed()
        {
            try
            {
                _connection = new SqlConnection(connectionString);

                _connection.Open();
                _connection.Close();

                return true;
            }

            catch (SqlException)
            {
                _connection.Close();
                return false;
            }
        }

        public bool RegisterUser(User user)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Add_User";
                    Guid userId;

                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter outId = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outId);
                        command.Parameters.AddWithValue("@login", user.Login);
                        command.Parameters.AddWithValue("@password", user.Password);

                        _connection.Open();
                        var result = command.ExecuteScalar();
                        _connection.Close();

                        if (!Guid.TryParse(outId.Value.ToString(), out userId))
                        {
                            throw new InvalidCastException();
                        }

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

        public bool RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.User_RemoveRole";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_User", userId);
                        command.Parameters.AddWithValue("@id_Role", roleId);

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

        public bool SetUserToRole(Guid userId, Guid roleId)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Set_UserRoles";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_User", userId);
                        command.Parameters.AddWithValue("@id_Role", roleId);

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

        public bool SetUserToRole(Guid userId, string roleName)
        {
            try
            {
                using (_connection = new SqlConnection(connectionString))
                {
                    string stProc = "dbo.Set_UserRoles_ByRoleName";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_User", userId);
                        command.Parameters.AddWithValue("@roleName", roleName);

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
    }
}
