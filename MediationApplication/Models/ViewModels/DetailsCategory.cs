using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediationApplication.Models.ViewModels
{
    public class DetailsCategory
    {
        public CategoryDto SelectedCategory { get; set; }
        public IEnumerable<MantraDto> RelatedMantras { get; set; }
    }
}