using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;

namespace Epam.DigitalLibrary.DalContracts
{
    public interface IUserDAO
    {
        public bool IsConnectionAllowed();

        public User GetUser(Guid id);

        public User GetUser(string login);

        public bool RegisterUser(User user);

        public List<string> GetUserRoles(Guid userId);

        public List<User> GetUsers();

        public bool SetUserToRole(Guid userId, Guid roleId);

        public bool RemoveRoleFromUser(Guid userId, Guid roleId);
    }
}
