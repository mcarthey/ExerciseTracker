using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Models;

public class Exercise
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public string VideoUrl { get; set; } // Optional

    [Required]
    [MaxLength(50)]
    public string MuscleGroup { get; set; }

    public string Icon { get; set; } // New: Stores API Image URL

    public ICollection<ExerciseProgram> ExercisePrograms { get; set; } = new List<ExerciseProgram>();
}
