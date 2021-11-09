using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.DalContracts
{
    public interface IUserDAO
    {
        public bool IsConnectionAllowed();

        public List<string> GetUserRoles(string userLogin);
    }
}
