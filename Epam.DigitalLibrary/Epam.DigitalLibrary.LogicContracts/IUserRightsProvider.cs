using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LogicContracts
{
    public interface IUserRightsProvider
    {
        public bool IsInRole(string role);

        public bool IsCredentialRight();
    }
}
