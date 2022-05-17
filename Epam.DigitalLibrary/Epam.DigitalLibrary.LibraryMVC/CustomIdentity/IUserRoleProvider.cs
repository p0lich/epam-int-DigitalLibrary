using Epam.DigitalLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.CustomIdentity
{
    public interface IUserRoleProvider
    {
        public int RegisterUser(string login, string password);

        public int AuthenticateUser(string login, string password);

        public IEnumerable<Claim> GetUserClaims(string login);

        public ClaimsPrincipal GetPrincipals(IEnumerable<Claim> claims);
    }
}
