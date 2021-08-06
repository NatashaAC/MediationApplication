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

        [Required]
        public string MantraName { get; set; }

        // A Mantra can fall under many Categories
        public ICollection<Category> Categories { get; set; }
    }

    public class MantraDto
    {
        public int MantraID { get; set; }

        [Required(ErrorMessage = "Please type in a mantra!")]
        public string MantraName { get; set; }
    }
}