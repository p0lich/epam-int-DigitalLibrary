using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public class User
    {
        public Guid ID { get; }
        public string Login { get; set; }
        public string Password { get; private set; }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public User(Guid id, string login, string password)
        {
            ID = id;
            Login = login;
            Password = password;
        }
    }
}
