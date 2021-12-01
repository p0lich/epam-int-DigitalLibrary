using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.DalConfig
{
    public static class SqlConfig
    {
        public static readonly IConfigurationRoot configuration;
        public static readonly string connString;

        public static readonly string connectionLogin;
        public static readonly SecureString connectionCredentials;

        static SqlConfig()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            connString = configuration.GetConnectionString("SSPIConnString");

            connectionLogin = "lib_admin";

            connectionCredentials = new SecureString();

            connectionCredentials.AppendChar('1');
            connectionCredentials.AppendChar('2');
            connectionCredentials.AppendChar('3');

            connectionCredentials.MakeReadOnly();
        }
    }
}
