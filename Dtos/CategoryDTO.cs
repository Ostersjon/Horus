using System.ComponentModel.DataAnnotations;

namespace Castle.Dtos;

public class CategoryDTO
{
    [Required]
    [MinLength(3, ErrorMessage = "must be more than 3 chars")]
    [MaxLength(25, ErrorMessage = "must be less than 25 chars")]
    public string Name { get; set; } = string.Empty;
}
