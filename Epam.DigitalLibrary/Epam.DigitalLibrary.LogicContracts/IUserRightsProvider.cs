using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Epam.DigitalLibrary.Entities;

namespace Epam.DigitalLibrary.LogicContracts
{
    public interface IUserRightsProvider
    {
        public bool IsInRole(Guid userId, string role);

        public bool IsCredentialRight();

        public User GetUser(Guid id);

        public User GetUser(string login);

        public bool RegisterUser(User user);

        public List<string> GetRoles(Guid userId);

        public List<User> GetUsers();

        public bool SetUserToRole(Guid userId, Guid roleId);

        public bool SetUserToRole(Guid userId, string roleName);

        public bool RemoveRoleFromUser(Guid userId, Guid roleId);
    }
}
