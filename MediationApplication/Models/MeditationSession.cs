using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediationApplication.Models
{
    public class MeditationSession
    {
        [Key]
        public int SessionID { get; set; }

        public DateTime SessionDate { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime SessionEndTime { get; set; }

        public int SessionDuration { get; set; }

        // A meditation session uses one mantra
        // A mantra can be used for various meditation sessions
        [ForeignKey("Mantra")]
        public int MantraID { get; set; }
        public virtual Mantra Mantra { get; set; }
    }

    public class MeditationSessionDto
    {
        public int SessionID { get; set; }

        public DateTime SessionDate { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime SessionEndTime { get; set; }

        public int SessionDuration { get; set; }

        public string MantraName { get; set; }
    }
}