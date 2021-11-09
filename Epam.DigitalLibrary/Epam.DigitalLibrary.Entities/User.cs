using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class User
    {
        private SecureString _password;

        public string Login { get; private set; }

        public User(string login, SecureString password)
        {
            login = Login;
            _password = password;
        }
    }
}
