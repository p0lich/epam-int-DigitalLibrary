using Epam.DigitalLibrary.AppCodes;
using Epam.DigitalLibrary.Encryption;
using Epam.DigitalLibrary.Entities;
using Epam.DigitalLibrary.LibraryWebApi.Models;
using Epam.DigitalLibrary.LogicContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRightsProvider _userLogic;
        private readonly ISHA512HashCompute _hashCompute;
        private readonly IConfiguration _config;

        public UserService(IUserRightsProvider userLogic, ISHA512HashCompute hashCompute, IConfiguration configuration)
        {
            _userLogic = userLogic;
            _hashCompute = hashCompute;
            _config = configuration;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            User realUser = _userLogic.GetUser(model.Login);

            if (realUser is null)
            {
                return null;
            }

            string inputPassword = _hashCompute.EncryptString(model.Password);

            if (realUser.Password != inputPassword)
            {
                return null;
            }

            var user = new UserEntity(_userLogic.GetUser(model.Login));

            if (user is null)
            {
                return null;
            }

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public UserEntity GetUser(Guid id)
        {
            return new UserEntity(_userLogic.GetUser(id));
        }

        public IEnumerable<UserEntity> GetUsers()
        {
            return _userLogic.GetUsers().Select(u => new UserEntity(u));
        }

        public bool LogOut(string userName)
        {
            throw new NotImplementedException();
        }

        public AuthenticateResponse Register(AuthenticateRequest model)
        {
            User existedUser = _userLogic.GetUser(model.Login);

            if (existedUser is not null)
            {
                return null;
            }

            string encPass = _hashCompute.EncryptString(model.Password);

            User user = new User(model.Login, encPass);

            if (_userLogic.RegisterUser(user))
            {
                var regResult = _userLogic.SetUserToRole(_userLogic.GetUser(user.Login).ID, UserRights.Reader);

                if (regResult)
                {
                    UserEntity userEntity = new UserEntity(_userLogic.GetUser(model.Login));
                    return new AuthenticateResponse(userEntity, GenerateJwtToken(userEntity));
                }
            }

            return null;
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                GetUserClaims(user),
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IEnumerable<Claim> GetUserClaims(UserEntity userEntity)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, userEntity.Login));

            User user = _userLogic.GetUser(userEntity.Login);

            var userRoles = _userLogic.GetRoles(user.ID);

            foreach (string role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
