using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Epam.DigitalLibrary.LibraryMVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace Epam.DigitalLibrary.LibraryMVC.CustomIdentity
{
    public class CustomUserRoleProvider : IUserRoleProvider
    {
        private readonly IUserRightsProvider _userLogic;

        public CustomUserRoleProvider(IUserRightsProvider userLogic)
        {
            _userLogic = userLogic;
        }

        public int AuthenticateUser(string login, string password)
        {
            User realUser = _userLogic.GetUser(login);

            if (realUser is null)
            {
                return AuthenticationCodes.NotExist;
            }
  
            byte[] storedHash = Encoding.ASCII.GetBytes(realUser.Password);
            string realPassword = Encoding.ASCII.GetString(storedHash);

            SHA512 sha512 = new SHA512Managed();
            byte[] inputHash = sha512.ComputeHash(Encoding.ASCII.GetBytes(password));
            string inputPassword = @$"{Encoding.ASCII.GetString(inputHash)}";

            if (realPassword == inputPassword)
            {
                return AuthenticationCodes.PasswordMismatch;
            }

            return AuthenticationCodes.AuthSuccess;
        }

        public ClaimsPrincipal GetPrincipals(IEnumerable<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public IEnumerable<Claim> GetUserClaims(string login)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, login));

            User user = _userLogic.GetUser(login);

            var userRoles = _userLogic.GetRoles(user.ID);

            foreach (string role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public int RegisterUser(string login, string password)
        {
            User existedUser = _userLogic.GetUser(login);

            SHA512 sha512 = new SHA512Managed();
            byte[] encBytePass = sha512.ComputeHash(Encoding.ASCII.GetBytes(password));
            string encPass = @$"{Encoding.ASCII.GetString(encBytePass)}";

            if (existedUser is not null)
            {
                return AuthenticationCodes.UserAlreadyExist;
            }

            User user = new User(login, encPass);

            if (_userLogic.RegisterUser(user))
            {
                return _userLogic.SetUserToRole(_userLogic.GetUser(user.Login).ID, UserRights.Reader)?
                    AuthenticationCodes.RegisterSuccess : AuthenticationCodes.UnableToRegister;
            }

            return AuthenticationCodes.UnableToRegister;
        }
    }
}
