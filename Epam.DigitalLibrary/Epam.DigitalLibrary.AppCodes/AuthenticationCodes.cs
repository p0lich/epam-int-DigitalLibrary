using System;

namespace Epam.DigitalLibrary.AppCodes
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
