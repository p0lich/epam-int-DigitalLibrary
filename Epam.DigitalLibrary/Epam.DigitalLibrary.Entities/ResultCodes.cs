using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    public static class ResultCodes
    {
        public const int Successfull = 0;
        public const int NoteExist = -1;
        public const int Error = -2;

        public const bool SuccessfullInsert = true;
        public const bool ErrorInsert = false;

        public const bool SuccessfullDelete = true;
        public const bool ErrorDelete = false;

        public const bool SuccessfullUpdate = true;
        public const bool ErrorUpdate = false;
    }
}
