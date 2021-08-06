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

        [Required]
        public DateTime SessionDate { get; set; }

        [Required]
        public DateTime SessionStartTime { get; set; }

        [Required]
        public DateTime SessionEndTime { get; set; }

        [Required]
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

        [Required(ErrorMessage = "Please select a date!")]
        public DateTime SessionDate { get; set; }

        [Required(ErrorMessage = "Please select a start time!")]
        public DateTime SessionStartTime { get; set; }

        [Required(ErrorMessage = "Please select a end time!")]
        public DateTime SessionEndTime { get; set; }

        [Required(ErrorMessage = "Please select a duration!")]
        public int SessionDuration { get; set; }

        public int MantraID { get; set; }
        public string MantraName { get; set; }
    }
}