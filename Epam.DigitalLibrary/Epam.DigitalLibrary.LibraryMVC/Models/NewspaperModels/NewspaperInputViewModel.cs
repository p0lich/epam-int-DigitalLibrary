﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.DigitalLibrary.LibraryMVC.Models.NewspaperModels
{
    public class NewspaperInputViewModel
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string PublicationPlace { get; set; }

        [Required]
        [StringLength(300)]
        public string Publisher { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [StringLength(2000)]
        public string ObjectNotes { get; set; }

        public string ISSN { get; set; }
    }
}