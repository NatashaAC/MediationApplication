
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediationApplication.Models.ViewModels
{
    public class DetailsSession
    {
        // This ViewModel is a Class which stores info that we need to present to /MeditationSession/DetialsSession/{id}

        // Existing Mantra Information
        public MantraDto SelectedMantra { get; set; }

        // Related Sessions
        public IEnumerable<MeditationSessionDto> RelatedSessions { get; set; }
    }
}