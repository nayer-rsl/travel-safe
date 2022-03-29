using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionSem.Models
{
    public class Language
    {
        [Key]
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }

    }
    public class LanguageDto
    {
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }

    }
}