using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperFullInput
    {
        public NewspaperInputViewModel NewspaperInput { get; set; }

        public NewspaperReleaseInputViewModel ReleaseInput { get; set; }
    }
}
