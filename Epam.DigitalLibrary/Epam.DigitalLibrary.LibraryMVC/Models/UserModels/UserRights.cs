using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models
{
    public static class UserRights
    {
        public const string Reader = "library_reader";
        public const string Librarian = "librarian";
        public const string Admin = "library_admin";

        public static readonly Guid ReaderRoleId = new Guid("8B9CD7EC-AE68-43A0-8F52-C26783FAC5B9");
        public static readonly Guid LibrarianRoleId = new Guid("A3523E11-AFFD-4492-94C4-0824D87DB6C5");
        public static readonly Guid AdminRoleId = new Guid("3E3DE5F4-BED1-4EE7-B9B4-82A65E52FE48");
    }
}
