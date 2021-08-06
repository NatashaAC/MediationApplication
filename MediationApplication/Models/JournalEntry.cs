using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MediationApplication.Models
{
    public class JournalEntry
    {
        [Key]
        public int JournalEntryID { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string MoodBefore { get; set; }

        [Required]
        public string MoodAfter { get; set; }

        [AllowHtml]
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

        [Required(ErrorMessage = "Please select a location!")]
        public string Location { get; set; }

        [Required (ErrorMessage = "Please type in your Mood!")]
        public string MoodBefore { get; set; }

        [Required(ErrorMessage = "Please type in your Mood!")]
        public string MoodAfter { get; set; }

        [AllowHtml]
        public string Thoughts { get; set; }

        public int SessionID { get; set; }
        public DateTime SessionDate { get; set; }
        public int SessionDuration { get; set; }
    }
}