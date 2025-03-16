using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Models;

public class ExerciseProgram
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public int Week { get; set; }

    [Required]
    public int Day { get; set; }

    [Required]
    [MaxLength(50)]
    public string Split { get; set; } // e.g., "Upper", "Lower", "Push"

    public string Notes { get; set; } // Optional

    // Now references ExerciseSets instead of direct exercises
    public ICollection<ExerciseSet> ExerciseSets { get; set; } = new List<ExerciseSet>();

    public ICollection<ExerciseSchedule> ExerciseSchedules { get; set; } = new List<ExerciseSchedule>();
}