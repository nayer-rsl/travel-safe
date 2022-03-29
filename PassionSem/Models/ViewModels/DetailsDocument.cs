using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionSem.Models.ViewModels
{
    public class DetailsDocument
    {
        public DocumentDto SelectedDocument { get; set; }
        public IEnumerable<CountryDto> NeedCountries { get; set; }
    }
}