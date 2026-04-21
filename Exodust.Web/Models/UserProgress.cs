using System.ComponentModel.DataAnnotations;

namespace Exodust.Web.Models;

public class UserProgress
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Level { get; set; } = 1;

    [Range(0, int.MaxValue)]
    public int ExperiencePoints { get; set; }

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
