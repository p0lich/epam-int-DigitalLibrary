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

namespace Epam.DigitalLibrary.SqlDal
{
    public class UserDAO : IUserDAO
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SSPIConnString"].ConnectionString;
        private SqlCredential _userCredential;
        private SqlConnection _connection;

        public UserDAO(SqlCredential userCredential)
        {
            _userCredential = userCredential;
        }

        public List<string> GetUserRoles(string userLogin)
        {
            try
            {
                List<string> roles = new List<string>();

                using (_connection = new SqlConnection(connectionString, _userCredential))
                {
                    string stProc = "dbo.Get_UserRoles";
                    using (SqlCommand command = new SqlCommand(stProc, _connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@login", userLogin);

                        _connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            roles.Add(reader["Role"] as string);
                        }
                    }
                }

                return roles;
            }
            catch (Exception e)
            {
                _connection.Close();
                throw new Exception("Cannot get user roles\n" + e.Message);
            }
        }

        public bool IsConnectionAllowed()
        {
            try
            {
                _connection = new SqlConnection(connectionString, _userCredential);

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
    }
}
