using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionSem.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryContinent { get; set; }

        [ForeignKey("Language")]
        public int LanguageID { get; set; }
        public virtual Language Language { get; set; }

        public ICollection<Document> Documents { get; set; }

    }
    public class CountryDto
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryContinent { get; set; }

        public int LanguageID { get; set; }
        public string LanguageName { get; set; }

    }
}