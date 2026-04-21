using System.ComponentModel.DataAnnotations;

namespace Exodust.Web.Models;

public class PlayerOptions
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public bool IsPaid { get; set; }
}
