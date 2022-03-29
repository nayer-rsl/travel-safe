using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionSem.Models.ViewModels
{
    public class UpdateCountry
    {
        public CountryDto SelectedCountry { get; set; }


        public IEnumerable<LanguageDto> LanguageOptions { get; set; }
    }
}