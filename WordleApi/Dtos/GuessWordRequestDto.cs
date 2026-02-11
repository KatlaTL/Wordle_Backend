using System.ComponentModel.DataAnnotations;

public class GuessWordRequestDto
{
    [Required]
    [StringLength(5, MinimumLength = 5)]
    public string Word { get; set; } = string.Empty;
}