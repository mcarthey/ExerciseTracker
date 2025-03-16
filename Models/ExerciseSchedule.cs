using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTracker.Models
{
    public class ExerciseSchedule
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime ScheduledDate { get; set; }  // This ties a workout to a specific day

        [Required]
        [ForeignKey("ExerciseProgram")]
        public Guid ExerciseProgramId { get; set; }  // Links to an ExerciseProgram

        public ExerciseProgram ExerciseProgram { get; set; }  // Navigation property
    }

}
