using Epam.DigitalLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryWebApi.Models
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public UserEntity(User user)
        {
            Id = user.ID;
            Login = user.Login;
            Password = user.Password;
        }
    }
}
