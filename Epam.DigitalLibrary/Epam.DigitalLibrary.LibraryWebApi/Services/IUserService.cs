using Epam.DigitalLibrary.LibraryWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        AuthenticateResponse Register(AuthenticateRequest model);

        IEnumerable<UserEntity> GetUsers();

        UserEntity GetUser(Guid id);
    }
}
