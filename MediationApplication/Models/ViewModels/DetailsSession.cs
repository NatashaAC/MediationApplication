using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediationApplication.Models.ViewModels
{
    public class DetailsSession
    {
        // This ViewModel is a Class which stores info that we need to present to /MeditationSession/DetialsSession/{id}

        // Exisiting Session Information
        public MeditationSessionDto SelectedSession { get; set; }

        // Related Journal Entry
        public IEnumerable<JournalEntryDto> RelatedEntry { get; set; }
    }
}