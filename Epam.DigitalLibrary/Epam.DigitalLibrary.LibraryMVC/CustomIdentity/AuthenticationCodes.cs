using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.CustomIdentity
{
    public static class AuthenticationCodes
    {
        public const int NotExist = -2;
        public const int PasswordMismatch = -1;
        public const int AuthSuccess = 0;

        public const int UnableToRegister = -2;
        public const int UserAlreadyExist = -1;
        public const int RegisterSuccess = 0;
    }
}
