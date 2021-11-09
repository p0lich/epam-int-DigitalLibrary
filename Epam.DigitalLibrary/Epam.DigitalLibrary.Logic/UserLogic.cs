using Epam.DigitalLibrary.DalContracts;
using Epam.DigitalLibrary.LogicContracts;
using Epam.DigitalLibrary.SqlDal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

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

        public bool IsInRole(string targetRole)
        {
            List<string> roles = _userDAO.GetUserRoles(_login);

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
    }
}
