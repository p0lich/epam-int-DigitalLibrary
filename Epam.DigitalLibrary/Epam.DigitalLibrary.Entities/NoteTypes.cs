using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NoteTypes : short
    {
        [Description("None")]
        None = 0,

        [Description("Book")]
        Book = 1,

        [Description("Newspaper")]
        Newspaper = 2,

        [Description("Patent")]
        Patent = 3,
    }
}
