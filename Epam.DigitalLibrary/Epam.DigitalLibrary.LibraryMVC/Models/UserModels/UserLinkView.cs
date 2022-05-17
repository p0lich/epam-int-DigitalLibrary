using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public class UserLinkView
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public List<string> Roles { get; set; }
    }
}
