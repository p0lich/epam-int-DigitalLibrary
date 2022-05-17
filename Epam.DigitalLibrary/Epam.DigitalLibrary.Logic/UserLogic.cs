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

        public UserLogic(string connString)
        {
            _userDAO = new UserDAO(connString);
        }

        public bool IsInRole(Guid userId, string targetRole)
        {
            try
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

            catch (Exception e) when (e is not DataAccessException)
            {
                throw new BusinessLogicException(e.Message, e.InnerException);
            }
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
            return _userDAO.GetUser(id);
        }

        public User GetUser(string login)
        {
            return _userDAO.GetUser(login);
        }

        public bool RegisterUser(User user)
        {
            return _userDAO.RegisterUser(user);
        }

        public List<User> GetUsers()
        {
            return _userDAO.GetUsers();
        }

        public bool SetUserToRole(Guid userId, Guid roleId)
        {
            return _userDAO.SetUserToRole(userId, roleId);
        }

        public bool SetUserToRole(Guid userId, string roleName)
        {
            return _userDAO.SetUserToRole(userId, roleName);
        }

        public bool RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            return _userDAO.RemoveRoleFromUser(userId, roleId);
        }
    }
}
