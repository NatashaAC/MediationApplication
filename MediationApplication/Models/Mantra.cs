using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediationApplication.Models
{
    public class Mantra
    {
        [Key]
        public int MantraID { get; set; }
        public string MantraName { get; set; }

        // Add foriegnkey for categories
    }

    public class MantraDto
    {
        public int MantraID { get; set; }
        public string MantraName { get; set; }
    }
}