using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PassionSem.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }

        public ICollection<Country> Countries { get; set; }

    }
    public class DocumentDto
    {
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
    }

}