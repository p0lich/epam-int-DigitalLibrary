using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperFullDetails
    {
        public NewspaperDetailsViewModel NewspaperDetails { get; set; }
        public NewspaperReleaseDetailsViewModel ReleaseDetails { get; set; }
    }
}
