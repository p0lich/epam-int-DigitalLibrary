using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.SqlDal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.CustomExeptions;

namespace Epam.DigitalLibrary.Logic
{
    public class UserLogic : IUserRightsProvider
    {
        private IUserDAO _userDAO;
        private SqlCredential _userCredential;
        private string _login;

        public UserLogic(string login, SecureString password)
        {
            _login = login;
            _userCredential = new SqlCredential(login, password);
            _userDAO = new UserDAO(_userCredential);
        }

        public bool IsInRole(Guid userId, string targetRole)
        {
            List<string> roles = _userDAO.GetUserRoles(userId);

            foreach (string role in roles)
            {
                if (role == targetRole)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCredentialRight()
        {
            return _userDAO.IsConnectionAllowed();
        }

        public List<string> GetRoles(Guid userId)
        {
            return _userDAO.GetUserRoles(userId);
        }

        public User GetUser(Guid id)
        {
            try
            {
                return _userDAO.GetUser(id);
            }

            catch (Exception e) when (e is not DataAccessException)
            {

                throw;
            }
        }

        public User GetUser(string login)
        {
            try
            {
                return _userDAO.GetUser(login);
            }

            catch (Exception e) when (e is not DataAccessException)
            {

                throw;
            }
        }

        public bool RegisterUser(User user)
        {
            try
            {
                return _userDAO.RegisterUser(user);
            }

            catch (Exception e) when (e is not DataAccessException)
            {

                throw;
            }
        }
    }
}
