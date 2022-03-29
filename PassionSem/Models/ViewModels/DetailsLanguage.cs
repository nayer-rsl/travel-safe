using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionSem.Models.ViewModels
{
    public class DetailsLanguage
    {
        public LanguageDto SelectedLanguage { get; set; }

        //all of the countries for this language
        public IEnumerable<CountryDto> SpokenCountries { get; set; }
    }
}