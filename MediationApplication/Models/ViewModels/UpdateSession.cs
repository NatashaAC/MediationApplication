using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediationApplication.Models.ViewModels
{
    public class UpdateSession
    {
        // This ViewModel is a Class which stores info that we need to present to /MeditationSession/UpdateSession/{id}

        // Existing Session Info
        public MeditationSessionDto SelectedSession { get; set; }

        // List of Mantras to choose from
         public IEnumerable<MantraDto> MantraOptions { get; set; }
    }
}