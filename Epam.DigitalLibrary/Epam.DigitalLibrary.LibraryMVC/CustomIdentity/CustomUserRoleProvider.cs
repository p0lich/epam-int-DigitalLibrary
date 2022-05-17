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
using Epam.DigitalLibrary.LibraryMVC.CustomEncryption;

namespace Epam.DigitalLibrary.LibraryMVC.CustomIdentity
{
    public class CustomUserRoleProvider : IUserRoleProvider
    {
        private readonly IUserRightsProvider _userLogic;
        private readonly ISHA512HashCompute _hashCompute;

        public CustomUserRoleProvider(IUserRightsProvider userLogic, ISHA512HashCompute hashCompute)
        {
            _userLogic = userLogic;
            _hashCompute = hashCompute;
        }

        public int AuthenticateUser(string login, string password)
        {
            User realUser = _userLogic.GetUser(login);

            if (realUser is null)
            {
                return AuthenticationCodes.NotExist;
            }

            string inputPassword = _hashCompute.EncryptString(password);

            if (realUser.Password == inputPassword)
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

            if (existedUser is not null)
            {
                return AuthenticationCodes.UserAlreadyExist;
            }

            string encPass = _hashCompute.EncryptString(password);

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
