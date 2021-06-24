using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediationApplication.Models
{
    public class JournalEntry
    {
        [Key]
        public int JournalEntryID { get; set; }
        public string Location { get; set; }
        public string MoodBefore { get; set; }
        public string MoodAfter { get; set; }
        public string Thoughts { get; set; }

        // A Journal Entry reflects on one specific mediation session
        // A meditation session should have one Journal Entry about it
        [ForeignKey("MeditationSession")]
        public int SessionID { get; set; }
        public virtual MeditationSession MeditationSession { get; set; }
    }

    public class JournalEntryDto
    {
        public int JournalEntryID { get; set; }
        public string Location { get; set; }
        public string MoodBefore { get; set; }
        public string MoodAfter { get; set; }
        public string Thoughts { get; set; }

        public int SessionID { get; set; }
        public DateTime SessionDate { get; set; }
        public int SessionDuration { get; set; }
    }
}