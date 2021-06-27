
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediationApplication.Models.ViewModels
{
    public class DetailsMantra
    {
        // This ViewModel is a Class which stores info that we need to present to /Mantra/DetialsMantra/{id}

        // Existing Mantra Information
        public MantraDto SelectedMantra { get; set; }

        // Related Sessions
        public IEnumerable<MeditationSessionDto> RelatedSessions { get; set; }

        // Related Categories
        public IEnumerable<CategoryDto> RelatedCategories { get; set; }

        // Not Assigned Categories
        public IEnumerable<CategoryDto> UnassignedCategories { get; set; }
    }
}