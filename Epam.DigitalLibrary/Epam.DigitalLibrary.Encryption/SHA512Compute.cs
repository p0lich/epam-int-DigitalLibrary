using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Encryption
{
    public class SHA512Compute : ISHA512HashCompute
    {
        private SHA512 _sha512;

        public SHA512Compute()
        {
            _sha512 = new SHA512Managed();
        }

        public byte[] Get512Hash(string initialString)
        {
            byte[] data = Encoding.UTF8.GetBytes(initialString);
            return _sha512.ComputeHash(data);
        }

        public string ByteArrayToString(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }

        public string EncryptString(string initialString)
        {
            byte[] hashedString = Get512Hash(initialString);
            return ByteArrayToString(hashedString);
        }
    }
}
