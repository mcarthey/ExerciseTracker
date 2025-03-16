using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExerciseTracker.Models;

public class ExerciseSet
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
    [MaxLength(50)]
    public string SetType { get; set; } // e.g., "Strength", "Hypertrophy"

    [Required]
    public int Sets { get; set; }

    [Required]
    public int RepsMin { get; set; }

    [Required]
    public int RepsMax { get; set; }

    [MaxLength(10)]
    public string RpeRange { get; set; } // e.g., "7-9"

    [MaxLength(20)]
    public string RestInterval { get; set; } // e.g., "~3 min"

    public string Notes { get; set; } // Optional

    [ForeignKey("AlternateExercise1")]
    public Guid? Alternate1Id { get; set; }
    public Exercise AlternateExercise1 { get; set; }

    [ForeignKey("AlternateExercise2")]
    public Guid? Alternate2Id { get; set; }
    public Exercise AlternateExercise2 { get; set; }

    public string RepsDisplay
    {
        get
        {
            if (RepsMin == RepsMax)
            {
                return $"Reps: {RepsMin}";
            }
            return $"Reps: {RepsMin}-{RepsMax}";
        }
    }


    public ICollection<Result> Results { get; set; } = new List<Result>();
}