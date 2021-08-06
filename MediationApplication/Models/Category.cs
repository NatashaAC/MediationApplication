using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MediationApplication.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string CategoryDescription { get; set; }
        public string CategoryColour { get; set; }

        // A Category can have many Mantras
        public ICollection<Mantra> Mantras { get; set; }
    }

    public class CategoryDto
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Please type a category name!")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please type in a description!")]
        public string CategoryDescription { get; set; }
        public string CategoryColour { get; set; }
    }
}