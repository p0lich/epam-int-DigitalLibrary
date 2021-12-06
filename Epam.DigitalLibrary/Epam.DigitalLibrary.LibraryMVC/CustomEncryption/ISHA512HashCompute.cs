using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.CustomEncryption
{
    public interface ISHA512HashCompute
    {
        public byte[] Get512Hash(string initialString);

        public string ByteArrayToString(byte[] hashedByteArray);

        public string EncryptString(string initialString);
    }
}
