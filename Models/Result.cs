using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ExerciseTracker.Models
{
    public class Result
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("ExerciseProgram")]
        public Guid ExerciseProgramId { get; set; }
        public ExerciseProgram ExerciseProgram { get; set; }

        [Required]
        [ForeignKey("Exercise")]
        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        [Required]
        public int Set { get; set; }

        public int Reps { get; set; }
        public decimal Weight { get; set; } // DECIMAL(5,2)

        public int Rpe { get; set; } // 1-10 scale

        [Required]
        [MaxLength(50)]
        public string SetType { get; set; } // e.g., "Warmup", "Working"

        public string Notes { get; set; } // Optional

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
