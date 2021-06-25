using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediationApplication.Models.ViewModels
{
    public class UpdateEntry
    {
        // Existing Journal Entry Info
        public JournalEntryDto SelectedEntry { get; set; }

        // List of Meditation Sessions to choose from
        public IEnumerable<MeditationSessionDto> SessionOptions { get; set; }
    }
}