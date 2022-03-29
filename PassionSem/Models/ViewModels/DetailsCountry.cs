using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionSem.Models.ViewModels
{
    public class DetailsCountry
    {
        public CountryDto SelectedCountry { get; set; }
        public IEnumerable<DocumentDto> NecessaryDocuments { get; set; }

        public IEnumerable<DocumentDto> AvailableDocuments { get; set; }
    }
}