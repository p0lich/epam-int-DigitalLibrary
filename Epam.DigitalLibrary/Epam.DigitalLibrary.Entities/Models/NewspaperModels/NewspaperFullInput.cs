using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.Entities.Models.NewspaperModels
{
    public class NewspaperFullInput
    {
        public Guid NewspaperReleaseInputId { get; set; }

        public NewspaperReleaseInputViewModel ReleaseInput { get; set; }
    }
}
